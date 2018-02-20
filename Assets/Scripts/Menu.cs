using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

/// <summary>
/// Class responsible for Menu interactions :
/// </summary>
public class Menu : MonoBehaviour {

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        AudioManager.audioManagerInstance.SelectAudio("TEMP Daniel_Birch_-_02_-_Deep_In_Peace");
        AudioManager.audioManagerInstance.PlaySelectedAudio(true); // looping = true
        GameMaster.elapsedTime = Time.time;
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void MainMenu()
    {
        AudioManager.audioManagerInstance.SelectAudio("01 MainMenu - Nevada_City");
        AudioManager.audioManagerInstance.PlaySelectedAudio(true); // looping = true
        SceneManager.LoadScene(0);
    }

    public void SetVolume(float volume)
    {
        AudioManager.audioManagerInstance.ChangeVolume(volume);
    }
}
