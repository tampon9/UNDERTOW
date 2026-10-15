using UnityEngine;
using System.Collections;

public class RoomTransitionTrigger : MonoBehaviour
{
    public GameObject blockerWall;
    public DialogueTextUI dialogueUI;
    [TextArea] public string unlockLine = "Проход открыт";

    public void OnUnlocked()
    {
        if (blockerWall != null)
            blockerWall.SetActive(false);

        if (dialogueUI != null && !string.IsNullOrEmpty(unlockLine))
            StartCoroutine(ShowUnlockLineDelayed());
    }

    private IEnumerator ShowUnlockLineDelayed()
    {
        yield return new WaitForSeconds(2.5f); // подстрой под длительность записки
        dialogueUI.ShowLine(unlockLine);
    }
}