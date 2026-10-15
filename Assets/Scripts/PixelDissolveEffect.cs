using System;
using UnityEngine;

/// <summary>
/// Растекание реализовано как обычная покадровая анимация на SpriteRenderer —
/// набор заранее нарисованных пиксельных кадров, без шейдеров и без систем частиц.
/// Повесить на плоский объект (SpriteRenderer, смотрящий на камеру), позиция и масштаб
/// которого точно совпадают с силуэтом озера комнаты 1 (см. LakeSpotMirror в сцене).
///
/// Настройка Animator Controller:
/// 1. Импортировать спрайт-лист кадров, нарезать через Sprite Editor (Slice → Grid/Cell).
/// 2. Перетащить кадры по порядку в окно Animation — получится AnimationClip "Dissolve".
///    Убрать галочку Loop Time у клипа.
/// 3. На последнем кадре клипа добавить Animation Event, вызывающий NotifyFinished()
///    этого компонента (кнопка "Add Event" в Animation Window на нужном кадре).
/// 4. В Animator Controller: Any State → Dissolve по триггеру "Dissolve" (см. playTriggerName),
///    Dissolve → Exit по условию "Has Exit Time" (без transition duration, чтобы не смазывало).
/// </summary>
[RequireComponent(typeof(Animator))]
public class PixelDissolveEffect : MonoBehaviour
{
    public Animator animator;
    public string playTriggerName = "Dissolve";

    public event Action OnDissolveFinished;

    public void Play()
    {
        gameObject.SetActive(true);
        animator.SetTrigger(playTriggerName);
    }

    /// <summary>Вызывается Animation Event-ом на последнем кадре клипа — не вызывать вручную.</summary>
    public void NotifyFinished()
    {
        OnDissolveFinished?.Invoke();
    }
}
