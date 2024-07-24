using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioClip[] _tracks;
    [SerializeField] private AudioSource _music, _soundEffects;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    private void Start()
    {
        PlayMusic(0);
    }

    public void PlayMusic(int index)
    {
        AudioClip newClip = _tracks[index];
        if (_music.clip != newClip)
        {
            _music.Stop();
            _music.clip = newClip;
            _music.Play();
        }
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        _soundEffects.Stop();
        _soundEffects.PlayOneShot(clip, volume);
    }

}
