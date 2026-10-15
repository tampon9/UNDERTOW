using UnityEngine;

/// <summary>
/// Поиск ближайшего IInteractable. Если игрок стоит сразу внутри нескольких
/// коллайдеров одновременно (например, SmallTrigger вложен в BigTrigger) —
/// выбирается тот, что меньше по площади, как более конкретный/приоритетный.
/// Если игрок не внутри ни одного, но несколько коллайдеров попали в радиус —
/// выбирается ближайший по границе коллайдера (ClosestPoint), а не по позиции
/// объекта — именно позиция объекта (а не граница) раньше и путала Small/Big.
/// </summary>
public class PlayerInteractor : MonoBehaviour
{
    [Header("Поиск объектов")]
    public float interactRadius = 1.2f;
    public LayerMask interactableLayer = ~0;

    [Header("UI подсказки")]
    public InteractionPromptUI promptUI;

    [Header("Ввод")]
    public KeyCode interactKey = KeyCode.Z;

    private IInteractable current;

    private void Update()
    {
        FindNearestInteractable();

        if (current != null)
        {
            promptUI.Show(current.GetPrompt());
            if (Input.GetKeyDown(interactKey) && current.CanInteract())
                current.Interact(gameObject);
        }
        else
        {
            promptUI.Hide();
        }
    }

    private void FindNearestInteractable()
    {
        IInteractable containingSmallest = null;
        float smallestArea = float.MaxValue;

        IInteractable nearestOutside = null;
        float nearestDist = float.MaxValue;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRadius, interactableLayer);
        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent(out IInteractable interactable)) continue;

            if (hit.OverlapPoint(transform.position))
            {
                // Игрок физически внутри этого коллайдера — приоритет меньшему (вложенному).
                float area = hit.bounds.size.x * hit.bounds.size.y;
                if (area < smallestArea)
                {
                    smallestArea = area;
                    containingSmallest = interactable;
                }
            }
            else
            {
                // Игрок рядом, но не внутри — считаем расстояние до границы коллайдера.
                Vector2 closest = hit.ClosestPoint(transform.position);
                float d = Vector2.Distance(transform.position, closest);
                if (d < nearestDist)
                {
                    nearestDist = d;
                    nearestOutside = interactable;
                }
            }
        }

        current = containingSmallest != null ? containingSmallest : nearestOutside;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.4f, 0.7f, 0.8f, 0.4f);
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}