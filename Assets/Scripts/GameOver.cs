using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Class responsible for GameOver
/// </summary>
public class GameOver : MonoBehaviour {

    public GameObject retryUI;
    public static bool retry = false; // for 1st loading, we don't want to display "retry" txt

    public static GameOver gameOverinstance;
    //SINGLETON
    /// <summary>
    /// Initialize singleton instance and variables
    /// </summary> 
    private void Awake()
    {
        if (gameOverinstance != null)
        {
            Debug.LogError("More than one Player in scene");
            return;
        }
        else
        {
            gameOverinstance = this;
        }
    }

    /// <summary>
    /// Function called when the object becomes enabled and active
    /// </summary>
    public void OnEnable()
    {
        retryUI.SetActive(GameOver.retry); // Activate Retry Ui, Animation will play on Entry
        MainCharacterController.OnWallCollision += Retry; // Subscribing to OnCollision event
    }

    /// <summary>
    /// Function called when the object becomes disabled and inactive
    /// </summary>
    public void OnDisable()
    {
        MainCharacterController.OnWallCollision -= Retry;
    }

    /// <summary>
    /// Function called when OnCollision broadcast a signal
    /// </summary>
    public void Retry()
    {
        SceneManager.LoadScene(1); // Reload 1st lvl
        GameOver.retry = true; // Tell the class that we have retried (so the next scene can display retry UI)
    }

}
