using UnityEngine;

/// <summary>
/// Общий интерфейс для интерактивных объектов: вода/удочка, пистолет, дверь-триггер.
/// PlayerInteractor находит ближайший IInteractable в радиусе и по нажатию кнопки
/// вызывает Interact(). Подсказка (GetPrompt) может меняться динамически —
/// это то, как одна и та же кнопка у воды означает то "закинуть", то "подсечь".
/// </summary>
public interface IInteractable
{
    /// <summary>Текст подсказки на экране ("Закинуть удочку", "Подсечь!" и т.п.).</summary>
    string GetPrompt();

    /// <summary>Доступно ли взаимодействие сейчас (во время катсцены — нет).</summary>
    bool CanInteract();

    void Interact(GameObject interactor);
}
