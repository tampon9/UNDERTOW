using UnityEngine;

/// <summary>
/// Один на всю игру. Хранит и применяет громкость музыки, сохраняет её между
/// запусками через PlayerPrefs. Громкость приходит в проценты (0-100) — так удобнее
/// вязать со слайдером в UI, внутри конвертируется в 0..1 для AudioSource.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Музыка")]
    public AudioSource musicSource;
    public AudioClip backgroundMusic; // сюда — фоновая.mp3

    [Range(0, 100)]
    public int defaultVolumePercent = 70;

    private const string VolumePrefKey = "MusicVolume";

    public int CurrentVolumePercent { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        int saved = PlayerPrefs.GetInt(VolumePrefKey, defaultVolumePercent);
        SetMusicVolume(saved);
    }

    private void Start()
    {
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    /// <summary>0..100 — основной метод, используй его из кода.</summary>
    public void SetMusicVolume(int percent)
    {
        CurrentVolumePercent = Mathf.Clamp(percent, 0, 100);
        if (musicSource != null)
            musicSource.volume = CurrentVolumePercent / 100f;

        PlayerPrefs.SetInt(VolumePrefKey, CurrentVolumePercent);
    }

    /// <summary>Перегрузка под UnityEvent<float> от UI Slider (значения слайдера — float).</summary>
    public void SetMusicVolume(float percent)
    {
        SetMusicVolume(Mathf.RoundToInt(percent));
    }
}
