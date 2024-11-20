using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    F2,	     // 41
	Fsharp2, // 42
	G2,     // 43
    Gsharp2, // 44
	A2,     // 45
    Asharp2, // 46
	B2,	    // 47
	C3,     // 48
    Csharp3, // 49
	D3,     // 50
    Dsharp3, // 51
	E3,	    // 52
	F3,     // 53
    Fsharp3, // 54
	G3,     // 55
    Gsharp3, // 56
	A3,     // 57
    Asharp3, // 58
	B3,     // 59
	C4,     // (Middle C) 60
    Csharp4, // 61
	D4,     // 62
    Dsharp4, // 63
	E4      // 64
}
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    public static SoundManager instance;
    public AudioSource audioSource;
    
    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume =1)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }
}
