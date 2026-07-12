using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header ("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip bgmMusic;
    public AudioClip dashSFX;
    public AudioClip jumpSFX;
    public AudioClip landSFX;
    public AudioClip collectSerumSFX;
    public AudioClip memoryTriggeredSFX;
    public AudioClip centinelShotsSFX;
    public AudioClip securityAlarmSFX;
    public AudioClip gettingHurtSFX;
    public AudioClip dyingSFX;
    public AudioClip dyingFromInsanitySFX;
    public AudioClip acidSettingSFX;
    public AudioClip flyingDroneSFX;
    public AudioClip metalMillSFX;

    [Header("Nuevos Sonidos de Feedback")]
    public AudioClip footstepSFX;
    public AudioClip acidDeathSFX;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (musicSource != null && bgmMusic != null)
        {

            //bgmMusic = Resources.Load<AudioClip>("Audio/BGM/BackgroundMusic");
            musicSource.clip = bgmMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }


    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        sfxSource.PlayOneShot(clip, volume);
    }
}
