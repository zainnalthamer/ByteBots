using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;


public class SoundController : MonoBehaviour
{
    [System.Serializable]
    public class MusicSettings
    {
        public string sceneName;
        public bool switchMusic, switchEnvironment, isLooping;
        public int musicIndex;
        public int environmentIndex;

    }


    [SerializeField]
    List<AudioClip> musicClips = new List<AudioClip>();
    [SerializeField]
    List<AudioClip> environmentClips = new List<AudioClip>();
    [SerializeField]
    List<AudioClip> soundClips = new List<AudioClip>();
    [SerializeField]
    AudioSource musicAudioSource, environmentAudioSource, soundAudioSource;

    public List<MusicSettings> startMusicOnSceneLoad = new List<MusicSettings>();
    public AudioMixer gameMixer;

    public static SoundController Instance { get; private set; }

    #region Singelton

    private void Awake()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "GameScene")
        {
            // If we are in the GameScene, destroy any existing instance from previous scenes
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }
            // Set this as the new instance
            Instance = this;
        }
        else
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        SwapCheck(currentSceneName);
    }
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SwapCheck(scene.name);

        // Destroy the existing instance if entering the GameScene
        if (scene.name == "GameScene" && Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
    }


    private void Start()
    {
        if (SaveLoadManager.Instance.localData.muteSound)
        {
            MuteSound(true);
            musicAudioSource.Stop();
        }
    }

    public void SwapCheck(string scene)
    {
        foreach (var item in startMusicOnSceneLoad)
        {
            if (scene == item.sceneName)
            {
                if (item.switchMusic)
                    PlayOrSwitchMusic(item.musicIndex);

                if (item.switchEnvironment)
                    PlayOrSwitchEnvironment(item.environmentIndex);
            }
        }
    }

    public void SwapCheck(int sceneIndex)
    {
        string scene = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
        scene = System.IO.Path.GetFileNameWithoutExtension(scene);

        foreach (var item in startMusicOnSceneLoad)
        {
            if (scene == item.sceneName)
            {
                if (item.switchMusic)
                    PlayOrSwitchMusic(item.musicIndex);

                if (item.switchEnvironment)
                    PlayOrSwitchEnvironment(item.environmentIndex);
            }
        }
    }

    public void MuteSound(bool value)
    {
        if (value)
        {
            gameMixer.SetFloat("MasterVolume", -80);
            SaveLoadManager.Instance.localData.muteSound = value;
        }
        else
        {
            gameMixer.SetFloat("MasterVolume", 0);
            SaveLoadManager.Instance.localData.muteSound = value;

        }
    }

    public void PauseMusic()
    {
        musicAudioSource.Pause();
    }

    public void ResumeMusic()
    {
        musicAudioSource.Play();
    }

    public void PauseSFX()
    {
        environmentAudioSource.Pause();
        soundAudioSource.Pause();
    }

    public void ResumeSFX()
    {
        environmentAudioSource.Play();
        soundAudioSource.Play();
    }

    public void PlayOrSwitchMusic(int musicIndex)
    {
        StartCoroutine(SwapMusic(musicIndex));
    }

    public void PlayOrSwitchEnvironment(int environmentIndex)
    {
        StartCoroutine(SwapEnvironment(environmentIndex));
    }

    public void PlaySFX(int sfxIndex)
    {
        if (!SaveLoadManager.Instance.localData.muteSound)
            soundAudioSource.PlayOneShot(soundClips[sfxIndex]);
    }

    IEnumerator SwapMusic(int index)
    {

        if (musicAudioSource.clip == musicClips[index])
        {
            yield break;
        }
        else
        {
            if (musicAudioSource.isPlaying)
                StartCoroutine(FadeAudioSource.StartFade(musicAudioSource, 2.5f, 0f));

            yield return new WaitUntil(() => FadeAudioSource.currentVolume == 0);
            musicAudioSource.clip = musicClips[index];

            if (!musicAudioSource.isPlaying)
                if (!SaveLoadManager.Instance.localData.muteSound)
                    musicAudioSource.Play();

            StartCoroutine(FadeAudioSource.StartFade(musicAudioSource, 2.5f, 1f));
        }
    }

    IEnumerator SwapEnvironment(int index)
    {

        if (environmentAudioSource.clip == musicClips[index])
        {
            yield break;
        }
        else
        {
            if (environmentAudioSource.isPlaying)
                StartCoroutine(FadeAudioSource.StartFade(environmentAudioSource, 2.5f, 0f));

            yield return new WaitUntil(() => FadeAudioSource.currentVolume == 0);
            environmentAudioSource.clip = environmentClips[index];

            if (!environmentAudioSource.isPlaying)
                if (!SaveLoadManager.Instance.localData.muteSound)
                    environmentAudioSource.Play();

            StartCoroutine(FadeAudioSource.StartFade(environmentAudioSource, 2.5f, .55f));
        }
    }

    public void FadeMusic(float duration, float targetVolume)
    {
        StartCoroutine(FadeAudioSource.StartFade(musicAudioSource, duration, targetVolume));
    }

    public void FadeEnvironment(float duration, float targetVolume)
    {
        StartCoroutine(FadeAudioSource.StartFade(environmentAudioSource, duration, targetVolume));
    }

    public void StopEnvironment()
    {
        StartCoroutine(FadeAudioSource.StartFade(environmentAudioSource, 2.5f, 0));
        StartCoroutine(StopEnvironmentCoroutine());
    }

    IEnumerator StopEnvironmentCoroutine()
    {
        yield return new WaitUntil(() => environmentAudioSource.volume == 0);
        environmentAudioSource.Stop();
    }

    public void ChangeMusicValue(float value)
    {
        musicAudioSource.volume = value;

    }


}