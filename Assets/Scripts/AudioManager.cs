using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Class responsible for Audio management:
/// </summary>
public class AudioManager : MonoBehaviour
{

    public AudioMixer audioMixer;
    private List<AudioSource> audioMusics;
    private AudioSource playingAudio;

    public static AudioManager audioManagerInstance;
    //SINGLETON
    /// <summary>
    /// Initialize singleton instance
    /// </summary>
    private void Awake()
    {
        if (audioManagerInstance == null)
        {
            // Get child elements from sounds attached to game object
            audioMusics = new List<AudioSource>();
            for (int i = 0; i < this.gameObject.transform.childCount; i++)
            {
                AudioSource audio = this.gameObject.transform.GetChild(i).GetComponent<AudioSource>(); // getting AudioSource component from child transform
                audioMusics.Add(audio);
            }
            audioManagerInstance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(audioManagerInstance); // We keep one instance for music that should never be destroyed
    }

    /// <summary>
    /// Play a the 1st audio
    /// </summary>
    private void Start()
    {
        SelectAudio("01 MainMenu - Nevada_City");
        PlaySelectedAudio(true); // looping = true
    }

    /// <summary>
    /// Change volume from master audio Mixer
    /// </summary>
    /// <param name = volume> float volume </param>
    public void ChangeVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    /// <summary>
    /// Play a selected audio
    /// </summary>
    public void PlaySelectedAudio(bool loop)
    {
        if (playingAudio == null)
        {
            Debug.Log("Error, no selected sound");
            return;
        }
        playingAudio.Play();
        playingAudio.loop = loop;
    }

    /// <summary>
    /// Stop current Audio selected and Select an audio file from name
    /// </summary>
    public void SelectAudio(string name)
    {
        if (playingAudio != null)
        {
            playingAudio.Stop();
        }
        AudioSource audio = audioMusics.Find(audioFile => audioFile.name == name);
        if (audio == null)
        {
            Debug.Log("Error Sound :" + name + "not found");
            return;
        }
        this.playingAudio = audio;
    }
}
