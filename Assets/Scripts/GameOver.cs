using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for GameOver
/// </summary>
public class GameOver : MonoBehaviour {
    public GameObject retryUI;

    public void OnAwake()
    {
        retryUI.SetActive(false);
    }

    /// <summary>
    /// Function called when the object becomes enabled and active
    /// </summary>
    public void OnEnable()
    {
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
        Debug.Log("Try again");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
