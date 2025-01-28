using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SoundType
{
    aNOTES,
    bNOTES,
    cNOTES,
    dNOTES,
    eNOTES,
    fNOTES,
    gNOTES,
    SHARPS
    // MENU,
    // BACKGROUND,
    // SONGS,
    // MUSIC,
    // SCORESHEETS
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        Debug.Log("Playing sound: " + sound);
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        if (clips.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned for: " + sound);
            return;
        }
        AudioClip Clips = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(Clips, volume);
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
#endif
}
[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; set => sounds = value; }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}

