using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

/// <summary>
/// Class responsible for Menu interactions :
/// </summary>
public class Menu : MonoBehaviour {

    public static string playerName = "Anonymous";
    public TMP_InputField userNameInput;

    public void Start()
    {
        if (userNameInput != null&&playerName != "Anonymous") 
        {
            userNameInput.text = playerName;
        }

    }

    public void ChangeUserName()
    {
        if(userNameInput != null)
        {
            playerName = userNameInput.text;
            Debug.Log(userNameInput.text);
        }
    }

    public void PlayLevel(int index) // index of level, start at 1
    {
        GameMaster.lvl = index - 1; // 1st level is tutorial we don't want to count it as a level
        MapManager.sublvl = 0;
        SceneManager.LoadScene(index);
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
        GameMaster.retry = false;
        SceneManager.LoadScene(0);
    }

    public void SetVolume(float volume)
    {
        AudioManager.audioManagerInstance.ChangeVolume(volume);
    }
}
