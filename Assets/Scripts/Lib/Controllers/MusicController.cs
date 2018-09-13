using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    [System.Serializable]
    public class MusicTrack
    {
        public string trackSlug;
        public AudioSource audioSource;
    }

    public bool forceMute;

    public List<MusicTrack> musicTracks;

    public float defaultVolume = 0.8f;
    public float fadeTime = 0.5f;

    protected string currentTrackSlug;
    protected bool isMusicMuted;
    protected float volume;

    // Use this for initialization
    void Start ()
    {
        foreach(MusicTrack musicTrack in musicTracks)
        {
            musicTrack.audioSource.volume = defaultVolume;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public MusicTrack GetTrackFromSlug(string trackSlug)
    {
        return musicTracks.Find(x => x.trackSlug.Equals(trackSlug));
    }

    public void SwitchToTrack(string trackSlug, bool FadeIn = true)
    {
        if (currentTrackSlug != trackSlug)
        {
            string previousTrackSlug = currentTrackSlug;
            currentTrackSlug = trackSlug;

            if (previousTrackSlug != null)
            {
                StopTrack(previousTrackSlug, FadeIn);
            }
            
            PlayTrack(currentTrackSlug, FadeIn);
        }
    }

    public void PlayTrack(string trackSlug, bool fadeIn = true)
    {
        MusicTrack musicTrack = GetTrackFromSlug(trackSlug);

        if (fadeIn)
        {
            StartCoroutine(FadeIn(this, musicTrack, fadeTime));
        }
        else
        {
            musicTrack.audioSource.Play();
        }
    }

    public void StopTrack(string trackSlug, bool fadeOut = true)
    {
        MusicTrack musicTrack = GetTrackFromSlug(trackSlug);

        if (fadeOut)
        {
            StartCoroutine(FadeOut(this, musicTrack, fadeTime));
        }
        else
        {
            musicTrack.audioSource.Stop();
        }
    }

    public static IEnumerator FadeIn(MusicController musicController, MusicTrack musicTrack, float FadeTime)
    {
        AudioSource audioSource = musicTrack.audioSource;
        float endVolume = musicController.IsMuted() ? 0 : musicController.defaultVolume;

        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < endVolume)
        {
            audioSource.volume += Time.deltaTime / FadeTime;

            yield return null;
        }

        musicController.SetVolume(audioSource, endVolume);
    }

    public static IEnumerator FadeOut(MusicController musicController, MusicTrack musicTrack, float FadeTime)
    {
        AudioSource audioSource = musicTrack.audioSource;
        float startVolume = musicController.IsMuted() ? 0 : musicController.defaultVolume;

        while (audioSource.volume > 0 && musicController.currentTrackSlug != musicTrack.trackSlug)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        if (musicController.currentTrackSlug != musicTrack.trackSlug)
        {
            audioSource.Stop();
            musicController.SetVolume(audioSource, startVolume);
        }
    }

    public void SetVolume(AudioSource track, float volume)
    {
        track.volume = IsMuted() ? 0 : volume;
    }

    public bool IsMuted()
    {
        return forceMute || isMusicMuted;
    }

    public void MuteMusic()
    {
        isMusicMuted = true;
    }

    public void UnmuteMusic()
    {
        isMusicMuted = false;
    }
}
