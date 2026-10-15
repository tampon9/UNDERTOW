using System.Collections;
using UnityEngine;

/// <summary>
/// Без диалога: второй персонаж доходит до кромки нового озера, садится, забрасывает
/// удочку — и на этом кадре игра заканчивается. Однозначно, без цикла и без повтора.
/// </summary>
public class EndingCutscene : MonoBehaviour
{
    [Header("Второй персонаж")]
    public Transform newSliceCharacter; // изначально выключен, стоит за левым краем кадра
    public Transform walkStartPoint;
    public Transform sitPoint;          // точка у нового озера
    public Animator newSliceAnimator;   // триггеры "Walk" -> "Sit"
    public float walkSpeed = 1.2f;

    [Header("Финал")]
    public float holdBeforeCredits = 3f;
    public GameObject creditsScreen;

    public void Play()
    {
        StartCoroutine(Sequence());
    }

    private IEnumerator Sequence()
    {
        newSliceCharacter.gameObject.SetActive(true);
        newSliceCharacter.position = walkStartPoint.position;
        newSliceAnimator.SetTrigger("Walk");

        while (Vector3.Distance(newSliceCharacter.position, sitPoint.position) > 0.05f)
        {
            newSliceCharacter.position = Vector3.MoveTowards(
                newSliceCharacter.position, sitPoint.position, walkSpeed * Time.deltaTime);
            yield return null;
        }

        newSliceAnimator.SetTrigger("Sit");

        yield return new WaitForSeconds(holdBeforeCredits);

        if (creditsScreen != null)
            creditsScreen.SetActive(true);

        GameFlowManager.Instance.FinishGame();
    }
}
