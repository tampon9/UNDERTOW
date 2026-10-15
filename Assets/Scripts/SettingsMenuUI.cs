using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Меню настроек: один слайдер громкости музыки, 0-100.
/// Настройка Slider в инспекторе: Min Value = 0, Max Value = 100, Whole Numbers = true.
/// При открытии панели слайдер подхватывает текущее сохранённое значение —
/// поэтому AudioManager должен существовать в сцене раньше, чем откроют настройки
/// (он это гарантирует сам через DontDestroyOnLoad + Awake).
/// </summary>
public class SettingsMenuUI : MonoBehaviour
{
    public Slider musicVolumeSlider;
    public TMP_Text musicVolumeLabel; // опционально — "70%" рядом со слайдером

    private void Start()
    {
        musicVolumeSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnEnable()
    {
        if (AudioManager.Instance == null) return;

        int current = AudioManager.Instance.CurrentVolumePercent;
        musicVolumeSlider.SetValueWithoutNotify(current); // не триггерим onValueChanged при открытии
        UpdateLabel(current);
    }

    private void OnSliderChanged(float value)
    {
        int percent = Mathf.RoundToInt(value);
        AudioManager.Instance?.SetMusicVolume(percent);
        UpdateLabel(percent);
    }

    private void UpdateLabel(int percent)
    {
        if (musicVolumeLabel != null)
            musicVolumeLabel.text = percent + "%";
    }
}
