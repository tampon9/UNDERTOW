using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CatchCountEvent : UnityEvent<int, int> { }

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }

    [Header("Прогресс")]
    public int totalCatches = 0;
    public int catchesToUnlockGunRoom = 20;

    public bool gunRoomUnlocked = false;
    public bool hasFiredGun = false;

    [Tooltip("Блокирует ввод игрока во время катсцен/анимации растекания")]
    public bool inputLocked = false;

    [Header("События")]
    public UnityEvent onGunRoomUnlocked;   // показ надписи + отключение стены
    public UnityEvent onGunFired;          // DeathAndRebirthSequence.Play()
    public UnityEvent onGameFinished;      // показ титров

    [Tooltip("(текущее, нужное) — подписать сюда CatchCounterUI.UpdateText")]
    public CatchCountEvent onCatchCountChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>Вызывать только на реальный улов (не на пустой заброс).</summary>
    public void RegisterCatch()
    {
        totalCatches++;
        onCatchCountChanged?.Invoke(totalCatches, catchesToUnlockGunRoom);

        if (!gunRoomUnlocked && totalCatches >= catchesToUnlockGunRoom)
        {
            gunRoomUnlocked = true;
            onGunRoomUnlocked?.Invoke();
        }
    }

    public void RegisterGunFired()
    {
        if (hasFiredGun) return;
        hasFiredGun = true;
        onGunFired?.Invoke();
    }

    public void FinishGame()
    {
        onGameFinished?.Invoke();
    }
}