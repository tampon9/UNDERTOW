using System;
using System.Collections;
using UnityEngine;

public enum FishingState { Idle, Waiting, Biting }

/// <summary>
/// Заброс -> ожидание (4-14 сек) -> короткое окно поклёвки -> улов.
/// Уловы выдаются строго по порядку (nextCatchIndex), без случайности.
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

    [Header("Отладка")]
    public bool debugLogs = true;

    public FishingState State { get; private set; } = FishingState.Idle;
    public event Action<FishingState> OnStateChanged;

    /// <summary>Индекс следующей записки, которая будет выдана (0-based).</summary>
    public int NextCatchIndex { get; private set; } = 0;

    private Coroutine routine;

    public void Cast()
    {
        if (debugLogs) Debug.Log($"[Fishing] Cast() вызван, состояние = {State}");
        if (State != FishingState.Idle) return;

        SetState(FishingState.Waiting);
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(WaitForBite());
    }

    /// <summary>Игрок решает не тянуть и прерывает заброс до поклёвки.</summary>
    public void CancelCast()
    {
        if (debugLogs) Debug.Log("[Fishing] CancelCast() вызван\n" + Environment.StackTrace);
        if (State == FishingState.Idle) return;

        if (routine != null) StopCoroutine(routine);
        routine = null;
        SetState(FishingState.Idle);
    }

    public void TryHook()
    {
        if (debugLogs) Debug.Log($"[Fishing] TryHook() вызван, состояние = {State}");
        if (State != FishingState.Biting) return;

        if (routine != null) StopCoroutine(routine);
        routine = null;
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

        routine = null;
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

        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.RegisterCatch();
            if (waterVisuals != null)
                waterVisuals.OnCatchRegistered(GameFlowManager.Instance.totalCatches);
        }

        if (entry != null && catchQuoteUI != null)
            catchQuoteUI.ShowCatch(entry);
    }

    private void SetState(FishingState newState)
    {
        if (debugLogs) Debug.Log($"[Fishing] Состояние: {State} -> {newState}");
        State = newState;
        OnStateChanged?.Invoke(newState);
    }
}