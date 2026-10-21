using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Один на всю игру. Хранит громкость музыки (PlayerPrefs) и меняет трек при смене сцены:
/// в меню играет menuMusic, во всех остальных сценах — backgroundMusic.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Музыка")]
    public AudioSource musicSource;
    public AudioClip menuMusic;        // hz_kuda (музыка меню)
    public AudioClip backgroundMusic;  // фоновая (музыка игры)
    public AudioClip creditsMusic;     // музыка титров (если пусто — играет backgroundMusic)

    [Header("Названия сцен")]
    public string menuSceneName = "MainMenu";
    public string creditsSceneName = "Credits";

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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        PlayForScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayForScene(scene.name);
    }

    private void PlayForScene(string sceneName)
    {
        if (musicSource == null) return;

        AudioClip clip;
        if (sceneName == menuSceneName)
            clip = menuMusic;
        else if (sceneName == creditsSceneName && creditsMusic != null)
            clip = creditsMusic;
        else
            clip = backgroundMusic;
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    /// <summary>0..100 — основной метод, используй его из кода.</summary>
    public void SetMusicVolume(int percent)
    {
        CurrentVolumePercent = Mathf.Clamp(percent, 0, 100);
        if (musicSource != null)
            musicSource.volume = CurrentVolumePercent / 100f;

        PlayerPrefs.SetInt(VolumePrefKey, CurrentVolumePercent);
    }

    /// <summary>Перегрузка под UnityEvent<float> от UI Slider.</summary>
    public void SetMusicVolume(float percent)
    {
        SetMusicVolume(Mathf.RoundToInt(percent));
    }
}