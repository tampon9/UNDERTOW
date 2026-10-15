using UnityEngine;

/// <summary>
/// Для пиксельной 2D-графики вместо шейдерных параметров используем простое
/// тонирование спрайта окружения (лёгкая сепия по мере накопления поклёвок)
/// и необязательный Animator-параметр на воде — например, bool "Approached",
/// который переключает кадры ряби, если такие кадры нарисованы.
/// </summary>
public class WaterVisuals : MonoBehaviour
{
    [Header("Спрайты")]
    public SpriteRenderer environmentSpriteRenderer; // фон/пространство целиком
    public Animator waterAnimator;                   // опционально: bool "Approached"

    [Header("Дрейф окружения (X = кол-во поклёвок, Y = 0..1)")]
    public AnimationCurve tintCurve = AnimationCurve.Linear(0, 0, 30, 1);
    public Color baseTint = Color.white;
    public Color driftedTint = new Color(0.85f, 0.78f, 0.65f);

    public void OnCatchRegistered(int totalCatches)
    {
        float t = Mathf.Clamp01(tintCurve.Evaluate(totalCatches));
        if (environmentSpriteRenderer != null)
            environmentSpriteRenderer.color = Color.Lerp(baseTint, driftedTint, t);
    }

    /// <summary>0 = игрок далеко, 1 = игрок рядом с водой.</summary>
    public void PulseOnApproach(float proximity01)
    {
        if (waterAnimator != null)
            waterAnimator.SetBool("Approached", proximity01 > 0.5f);
    }
}
