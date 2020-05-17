using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrack : MonoBehaviour
{
    public string trackSlug;

    public AudioSource AudioSource { get; set; }

    // Use this for initialization
    void Start ()
    {
        AudioSource = GetComponent<AudioSource>();
    }
}
