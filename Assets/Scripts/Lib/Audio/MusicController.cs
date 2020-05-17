using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    public bool forceMute;

    protected List<MusicTrack> musicTracks;

    public float defaultVolume = 0.8f;
    public float fadeTime = 0.5f;

    protected MusicTrack currentTrack;
    protected MusicTrack previousTrack;
    protected bool isMusicMuted;
    protected float volume;

    // Use this for initialization
    void Start ()
    {
        musicTracks = new List<MusicTrack>(transform.GetComponentsInChildren<MusicTrack>());

        foreach(MusicTrack musicTrack in musicTracks)
        {
            musicTrack.AudioSource.volume = defaultVolume;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public MusicTrack GetTrackFromSlug(string trackSlug)
    {
        return musicTracks.Find(x => x.trackSlug.Equals(trackSlug));
    }

    public void SwitchToTrack(string trackSlug, bool fade = true)
    {
        MusicTrack incomingTrack = GetTrackFromSlug(trackSlug);

        if (currentTrack == null || currentTrack.GetInstanceID() != incomingTrack.GetInstanceID())
        {
            previousTrack = currentTrack;
            currentTrack = incomingTrack;

            if (previousTrack != null)
            {
                StopTrack(previousTrack, fade);
            }
            
            PlayTrack(currentTrack, fade);
        }
    }

    public void SwitchToPreviousTrack(bool fade = true)
    {
        SwitchToTrack(previousTrack.trackSlug, fade);
    }

    public void PlayTrack(MusicTrack musicTrack, bool fadeIn = true)
    {
        if (fadeIn)
        {
            StartCoroutine(FadeIn(this, musicTrack, fadeTime));
        }
        else
        {
            if (IsMuted())
            {
                SetVolume(musicTrack.AudioSource, 0);
            }

            musicTrack.AudioSource.Play();
        }
    }

    public void PlayTrack(string trackSlug, bool fadeIn = true)
    {
        MusicTrack musicTrack = GetTrackFromSlug(trackSlug);

        PlayTrack(musicTrack, fadeIn);
    }

    public void StopTrack(MusicTrack musicTrack, bool fadeOut = true)
    {
        if (fadeOut)
        {
            StartCoroutine(FadeOut(this, musicTrack, fadeTime));
        }
        else
        {
            musicTrack.AudioSource.Stop();
        }
    }

    public void StopTrack(string trackSlug, bool fadeOut = true)
    {
        MusicTrack musicTrack = GetTrackFromSlug(trackSlug);

        StopTrack(musicTrack, fadeOut);
    }

    public static IEnumerator FadeIn(MusicController musicController, MusicTrack musicTrack, float FadeTime)
    {
        AudioSource audioSource = musicTrack.AudioSource;
        float endVolume = musicController.IsMuted() ? 0 : musicController.defaultVolume;

        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < endVolume)
        {
            musicController.SetVolume(audioSource, audioSource.volume + (Time.deltaTime / FadeTime));

            yield return null;
        }

        musicController.SetVolume(audioSource, endVolume);
    }

    public static IEnumerator FadeOut(MusicController musicController, MusicTrack musicTrack, float FadeTime)
    {
        AudioSource audioSource = musicTrack.AudioSource;
        float startVolume = musicController.IsMuted() ? 0 : musicController.defaultVolume;

        while (audioSource.volume > 0 && musicController.currentTrack.GetInstanceID() != musicTrack.GetInstanceID())
        {
            musicController.SetVolume(audioSource, audioSource.volume - (startVolume * Time.deltaTime / FadeTime));

            yield return null;
        }

        if (musicController.GetInstanceID() != musicTrack.GetInstanceID())
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
