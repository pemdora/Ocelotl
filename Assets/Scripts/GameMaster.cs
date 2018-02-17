using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class responsible for GameOver and Success
/// </summary>
public class GameMaster : MonoBehaviour {

    public GameObject retryUI;
    public static bool retry = false; // for 1st loading, we don't want to display "retry" txt


    // public GameObject finishUI;
    public static float elapsedTime = 0; // we wan't to get the time spend on menu
    public Transform player;
    public Transform goal;
    public TextMeshProUGUI timeFinish;
    public GameObject finishUI;
    private bool finish;

    public static GameMaster gameOverinstance;
    //SINGLETON
    /// <summary>
    /// Initialize singleton instance and variables
    /// </summary> 
    private void Awake()
    {
        Debug.Log("Time"+(Time.time - elapsedTime));
        if (gameOverinstance != null)
        {
            Debug.LogError("More than one Player in scene");
            return;
        }
        else
        {
            gameOverinstance = this;
        }
        finish = false;
    }

    /// <summary>
    /// Function called when the object becomes enabled and active
    /// </summary>
    public void OnEnable()
    {
        retryUI.SetActive(GameMaster.retry); // Activate Retry Ui, Animation will play on Entry
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
        GameMaster.retry = true; // Tell the class that we have retried (so the next scene can display retry UI)
    }

    /// <summary>
    /// Check if the player has reached goal
    /// </summary>
    private void Update()
    {
        // float time = Mathf.Floor(Time.time - elapsedTime);
        // timeFinish.text = time.ToString();
        // if the player press "Space" and is not moving and not swaping maps
        if (Vector3.Distance(player.position,goal.position) <=0.5f&&!finish)
        {
            Debug.Log("Finish");
            Debug.Log("Time" + (Time.time - elapsedTime));
            finish = true;
            finishUI.SetActive(true);
            float time = Mathf.Floor(Time.time - elapsedTime);
            timeFinish.text = time.ToString();
        };
    }
}
