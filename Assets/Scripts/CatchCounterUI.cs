using UnityEngine;
using TMPro;

/// <summary>
/// Простой TMP-текст "0/20". Повесить на объект в левом верхнем углу Canvas,
/// подписать GameFlowManager.onCatchCountChanged на UpdateText в инспекторе
/// (или это сделает OnEnable ниже — оба варианта работают, но инспекторный
/// проще снять/добавить без правки кода).
/// </summary>
public class CatchCounterUI : MonoBehaviour
{
    public TMP_Text label;
    public string format = "{0}/{1}";

    private void OnEnable()
    {
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.onCatchCountChanged.AddListener(UpdateText);
            UpdateText(GameFlowManager.Instance.totalCatches, GameFlowManager.Instance.catchesToUnlockGunRoom);
        }
    }

    private void OnDisable()
    {
        if (GameFlowManager.Instance != null)
            GameFlowManager.Instance.onCatchCountChanged.RemoveListener(UpdateText);
    }

    public void UpdateText(int current, int max)
    {
        if (label != null)
            label.text = string.Format(format, current, max);
    }
}