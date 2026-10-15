using System;
using System.Collections;
using UnityEngine;

public enum FishingState { Idle, Waiting, Biting }

/// <summary>
/// Заброс -> ожидание (4-14 сек) -> короткое окно поклёвки -> улов.
/// Уловы выдаются строго по порядку (nextCatchIndex), без случайности —
/// так игрок гарантированно видит записки 1, 2, 3... N именно в этом порядке.
/// </summary>
public class FishingManager : MonoBehaviour
{
    [Header("Ссылки")]
    public CatchDatabase catchDatabase;
    public CatchQuoteUI catchQuoteUI;
    public WaterVisuals waterVisuals;

    [Header("Тайминги")]
    public float minWaitTime = 4f;
    public float maxWaitTime = 14f;
    public float biteReactionWindow = 2f;

    public FishingState State { get; private set; } = FishingState.Idle;
    public event Action<FishingState> OnStateChanged;

    /// <summary>Индекс следующей записки, которая будет выдана (0-based).</summary>
    public int NextCatchIndex { get; private set; } = 0;

    private Coroutine routine;

    public void Cast()
    {
        if (State != FishingState.Idle) return;
        SetState(FishingState.Waiting);
        routine = StartCoroutine(WaitForBite());
    }

    /// <summary>Игрок решает не тянуть и прерывает заброс до поклёвки.</summary>
    public void CancelCast()
    {
        if (State == FishingState.Idle) return;
        if (routine != null) StopCoroutine(routine);
        SetState(FishingState.Idle);
    }

    public void TryHook()
    {
        if (State != FishingState.Biting) return;
        SetState(FishingState.Idle);
        ResolveCatch();
    }

    private IEnumerator WaitForBite()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(minWaitTime, maxWaitTime));
        if (State != FishingState.Waiting) yield break;

        SetState(FishingState.Biting);

        float t = 0f;
        while (State == FishingState.Biting && t < biteReactionWindow)
        {
            t += Time.deltaTime;
            yield return null;
        }

        if (State == FishingState.Biting)
            SetState(FishingState.Idle); // не подсёк вовремя — молча ушла
    }

    private void ResolveCatch()
    {
        if (catchDatabase == null || catchDatabase.Count == 0)
        {
            Debug.LogWarning("[FishingManager] CatchDatabase пуст или не назначен.");
            return;
        }

        // Все записки уже выданы — больше не считаем и не показываем.
        if (NextCatchIndex >= catchDatabase.Count) return;

        var entry = catchDatabase.GetEntry(NextCatchIndex);
        NextCatchIndex++;

        GameFlowManager.Instance?.RegisterCatch();
        if (GameFlowManager.Instance != null)
            waterVisuals?.OnCatchRegistered(GameFlowManager.Instance.totalCatches);

        if (entry != null)
            catchQuoteUI?.ShowCatch(entry);
    }

    private void SetState(FishingState newState)
    {
        State = newState;
        OnStateChanged?.Invoke(newState);
    }
}