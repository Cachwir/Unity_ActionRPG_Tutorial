using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour {

    [System.Serializable]
    public class SoundEffect
    {
        public string sfxSlug;
        public AudioSource audioSource;
    }

    public List<SoundEffect> soundEffects;

    public float defaultVolume = 0.4f;

    // Use this for initialization
    void Start ()
    {
        foreach (SoundEffect soundEffect in soundEffects)
        {
            soundEffect.audioSource.volume = defaultVolume;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public SoundEffect GetSoundEffectFromSlug(string sfxSlug)
    {
        return soundEffects.Find(x => x.sfxSlug.Equals(sfxSlug));
    }

    public void PlaySoundEffect(string sfxSlug)
    {
        GetSoundEffectFromSlug(sfxSlug).audioSource.Play();
    }
}
