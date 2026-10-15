using UnityEngine;

/// <summary>
/// Единственное действие во второй комнате. После срабатывания больше недоступен
/// (hasFiredGun в GameFlowManager защищает от повторного вызова).
/// </summary>
public class GunInteractable : MonoBehaviour, IInteractable
{
    public DeathAndRebirthSequence sequence;
    public string prompt = "Поднять пистолет";

    public bool CanInteract() =>
        GameFlowManager.Instance != null &&
        !GameFlowManager.Instance.inputLocked &&
        !GameFlowManager.Instance.hasFiredGun;

    public string GetPrompt() => prompt;

    public void Interact(GameObject interactor)
    {
        if (!CanInteract()) return;
        GameFlowManager.Instance.RegisterGunFired();
        sequence.Play();
    }
}
