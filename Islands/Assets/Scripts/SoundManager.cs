using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundManager : MonoBehaviour
{
    public static SoundManager main;
    private void Awake()
    {
        main = this;
    }

    [SerializeField]
    private AudioSource music;

    [SerializeField]
    private bool muteMusic;
    public bool MusicMuted { get { return muteMusic; } }
    [SerializeField]
    private bool muteSfx;
    public bool SfxMuted { get { return muteSfx; } }
    [SerializeField]
    private LerpSoundPitch flyingSound;

    public void ToggleSfx()
    {
        muteSfx = !muteSfx;
    }

    public void ToggleMusic()
    {
        muteMusic = !muteMusic;
    }

    public void PlayFlyingSound(float flightSpeed)
    {
        if (muteSfx)
        {
            return;
        }
        flyingSound.StartPitching(flightSpeed);
    }

    public void StopPlayingFlyingSound()
    {
        flyingSound.StopPitching();
    }

    [SerializeField]
    private List<GameSound> gameSounds;

    public void PlaySound(GameSoundType soundType)
    {
        if (muteSfx)
        {
            return;
        }
        GameSound gameSound = gameSounds.Where(sound => sound.Type == soundType).FirstOrDefault();
        if (gameSound != null)
        {
            AudioSource audio = gameSound.Get();
            if (audio != null)
            {
                audio.Play();
            }
        }
    }


    public void PlaySoundLoop(GameSoundType soundType)
    {
        if (muteSfx)
        {
            return;
        }
        GameSound gameSound = gameSounds.Where(sound => sound.Type == soundType && sound.Loop).FirstOrDefault();
        if (gameSound != null)
        {
            AudioSource audio = gameSound.GetLoop();
            if (audio != null && !audio.isPlaying)
            {
                audio.Play();
            }
        }
    }

    public void PauseLoop(GameSoundType soundType)
    {
        GameSound gameSound = gameSounds.Where(sound => sound.Type == soundType && sound.Loop).FirstOrDefault();
        if (gameSound != null)
        {
            AudioSource audio = gameSound.GetLoop();
            if (audio != null && audio.isPlaying)
            {
                audio.Pause();
            }
        }
    }

    private void Update()
    {
        if (muteMusic && music.isPlaying)
        {
            music.Pause();
        }
        if (!muteMusic && !music.isPlaying)
        {
            music.UnPause();
        }
    }
}


public enum GameSoundType
{
    Molsk,
    Boom,
    HitBirds,
    Skip,
    Hit,
    TurnHorizontal,
    TurnVertical
}

[System.Serializable]
public class GameSound
{
    [field: SerializeField]
    public GameSoundType Type { get; private set; }

    [field: SerializeField]
    private List<AudioSource> sounds;

    private List<GameSoundPool> soundPools = new List<GameSoundPool>();
    private bool initialized = false;

    [field: SerializeField]
    public bool Loop { get; private set; } = false;

    public AudioSource Get()
    {
        if (!initialized)
        {
            initialize();
        }

        if (sounds == null || sounds.Count == 0)
        {
            return null;
        }
        return soundPools[Random.Range(0, soundPools.Count)].getAvailable();
    }

    public AudioSource GetLoop()
    {
        if (!initialized)
        {
            initialize();
        }

        if (sounds == null || sounds.Count == 0)
        {
            return null;
        }
        return soundPools[Random.Range(0, soundPools.Count)].getAvailableLoop();
    }

    private void initialize()
    {
        soundPools = sounds.Select(it => new GameSoundPool(it)).ToList();
        initialized = true;
    }


    private class GameSoundPool
    {
        private AudioSource originalAudioSource;
        private List<AudioSource> audioSources = new List<AudioSource>();

        public GameSoundPool(AudioSource audioSource)
        {
            originalAudioSource = audioSource;
            addNewToPool();
        }

        public AudioSource getAvailableLoop()
        {
            var src = audioSources.First();
            if (src == null)
            {
                src = addNewToPool();
            }
            return src;
        }

        public AudioSource getAvailable()
        {
            var src = audioSources.Where(it => it.isPlaying == false).FirstOrDefault();
            if (src == null)
            {
                src = addNewToPool();
            }
            return src;
        }

        private AudioSource addNewToPool()
        {
            AudioSource newSource = GameObject.Instantiate(originalAudioSource, originalAudioSource.transform.parent);
            audioSources.Add(newSource);
            return newSource;
        }
    }
}