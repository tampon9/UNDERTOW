using UnityEngine;

/// <summary>
/// Большой триггер вокруг озера. Фраза появляется по нажатию Z.
/// reinteractCooldown не даёт повторным нажатиям бесконечно продлевать показ
/// одной и той же (особенно последней, когда approachLines кончились) фразы —
/// раньше это и вызывало эффект "текст завис на экране" при частых нажатиях.
/// </summary>
public class WaterProximityResponder : MonoBehaviour, IInteractable
{
    public DialogueTextUI dialogueUI;

    [TextArea]
    [Tooltip("Фразы по порядку подходов: [0] — первый раз, [1] — второй и т.д. " +
             "Когда подходов больше, чем фраз, используется последняя в списке.")]
    public string[] approachLines;

    [Tooltip("Текст подсказки, который видит игрок")]
    public string promptText = "Осмотреть озеро";

    [Tooltip("Минимальный интервал между повторными взаимодействиями, сек. " +
             "Не даёт спамом Z продлевать показ одной и той же реплики бесконечно.")]
    public float reinteractCooldown = 3.5f;

    private int approachCount = 0;
    private float lastInteractTime = -999f;

    public string GetPrompt() => promptText;

    public bool CanInteract() =>
        GameFlowManager.Instance != null && !GameFlowManager.Instance.inputLocked
        && approachLines != null && approachLines.Length > 0
        && Time.time - lastInteractTime >= reinteractCooldown;

    public void Interact(GameObject interactor)
    {
        if (approachLines == null || approachLines.Length == 0) return;

        lastInteractTime = Time.time;

        int index = Mathf.Min(approachCount, approachLines.Length - 1);
        dialogueUI?.ShowLine(approachLines[index]);
        approachCount++;
    }
}