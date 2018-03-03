using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

/// <summary>
/// Class responsible for GameOver and Success
/// Manage Time
/// </summary>
public class GameMaster : MonoBehaviour {

    public static int MAXLVL = 2;

    [Header("UI Elements variables")]
    public GameObject retryUI;
    public static bool retry = false; // for 1st loading, we don't want to display "retry" txt
    public GameObject finishUI;


    public static float elapsedTime = 0; // we wan't to get the time spend on menu
    public TextMeshProUGUI timeFinish;

    public static GameMaster gameMasterinstance;
    //SINGLETON
    /// <summary>
    /// Initialize singleton instance and variables
    /// </summary> 
    private void Awake()
    {
        Debug.Log("Time" + (Time.time));
        Debug.Log("Start Time"+(Time.time - elapsedTime));
        if (gameMasterinstance != null)
        {
            Debug.LogError("More than one gameMasterinstance in scene");
            return;
        }
        else
        {
            gameMasterinstance = this;
        }
    }

    /// <summary>
    /// Function called when the object becomes enabled and active
    /// </summary>
    public void OnEnable()
    {
        retryUI.SetActive(GameMaster.retry); // Activate Retry Ui, Animation will play on Entry
        MainCharacterController.OnWallCollisionEvent += Retry; // Subscribing to OnCollision event
        MainCharacterController.ReachedGoalEvent += LevelFinished;
    }

    /// <summary>
    /// Function called when the object becomes disabled and inactive
    /// </summary>
    public void OnDisable()
    {
        MainCharacterController.OnWallCollisionEvent -= Retry;
        MainCharacterController.ReachedGoalEvent -= LevelFinished;
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
    /// Finishing level if the player has reached goal
    /// </summary>
    public void LevelFinished()
    {
        Debug.Log("Finish" + (Time.time - elapsedTime));

        finishUI.SetActive(true);
        float time = Mathf.Floor(Time.time - elapsedTime);
        timeFinish.text = time.ToString();
        MapManager.sublvl++;
        if (MapManager.sublvl != MAXLVL)
        {
            IEnumerator coroutine = WaitAndLoadScene();
            StartCoroutine(coroutine);
        }
    }

    /// <summary>
    /// Function that waits seconds and LoadNextScene
    /// </summary>
    /// <returns>Return true if a wall with the given X and Z postion or false if not</returns>
    private IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(2f);
        #region WaitAndDo // this will be executed only when the coroutine have finished
        elapsedTime = Time.time - elapsedTime; // reset timer
        SceneManager.LoadScene(1); // Reload 1st lvl
        GameMaster.retry = false;
        #endregion
    }

    /// <summary>
    /// Function that triggers when option button is cliked on game
    /// </summary>
    public void PreventControlsInOptions()
    {
        MainCharacterController.characterController.lockControls = true;
    }

    /// <summary>
    /// Function that triggers when back button is cliked on option menu in game
    /// </summary>
    public void RestoreControlsFromOptions()
    {
        MainCharacterController.characterController.lockControls = false;
    }
}
