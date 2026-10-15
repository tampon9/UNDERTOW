using UnityEngine;

/// <summary>
/// Одна и та же кнопка взаимодействия означает разное в зависимости от FishingState:
/// в покое — "закинуть", в ожидании — "бросить" (осознанный отказ), на поклёвке — "подсечь".
/// </summary>
public class WaterInteractable : MonoBehaviour, IInteractable
{
    public FishingManager fishingManager;

    public bool CanInteract() =>
        GameFlowManager.Instance != null && !GameFlowManager.Instance.inputLocked
        && fishingManager != null;

    public string GetPrompt()
    {
        if (fishingManager == null) return "";

        switch (fishingManager.State)
        {
            case FishingState.Idle: return "Закинуть удочку";
            case FishingState.Waiting: return "Смотреть на воду";
            case FishingState.Biting: return "Подсечь!";
            default: return "";
        }
    }

    public void Interact(GameObject interactor)
    {
        if (fishingManager == null) return;

        switch (fishingManager.State)
        {
            case FishingState.Idle: fishingManager.Cast(); break;
            case FishingState.Waiting: fishingManager.CancelCast(); break;
            case FishingState.Biting: fishingManager.TryHook(); break;
        }
    }
}
