using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for main character controlls :
/// Character is able to Move with WASD keys
/// Show And Hide cursor with Right Click
/// </summary>

[RequireComponent(typeof(NavMeshAgent))]
public class MainCharacterController : MonoBehaviour
{

    private bool mouseLock;
    private NavMeshAgent agent;

    #region PositionVariables
    private Vector3 characterPos;
    private Vector3 targetPosition;
    #endregion

    #region AnimationVariables
    private float speed;
    public Animator animator;
    #endregion

    public static MainCharacterController characterController;
    //SINGLETON
    /// <summary>
    /// Initialize singleton instance
    /// </summary>
    private void Awake()
    {
        if (characterController != null)
        {
            Debug.LogError("More than one Player in scene");
            return;
        }
        else
        {
            characterController = this;
        }
    }

    /// <summary>
    /// Initialize variables
    /// </summary>
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        Cursor.lockState = CursorLockMode.Locked; // Hide the cursor to begin with
        mouseLock = true;
        
        characterPos = this.transform.position; // since player position is not 100% accurate, we generate an accurate variable position
        targetPosition = this.transform.position;

        speed = 1f;
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Check player inputs and move player or display mouse
    /// </summary>
    private void Update()
    {
        if (animator.GetBool("isWalking")) // if the player is moving
        {
            if (Vector3.Distance(targetPosition,transform.position)<0.34f) // check character's position with mathematical aproximation
            {
                animator.SetBool("isWalking", false); // set walking animation
                characterPos = targetPosition; // update characterPos
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position,targetPosition,speed*Time.deltaTime); // translate
            }
        }
        else
        {
            GetMovementInput();
        }
        ShowMouse();
    }
    
    /// <summary>
    /// Get input and translate character 
    /// </summary>
    private void GetMovementInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            targetPosition = characterPos + Vector3.back;
            LookAt(targetPosition);
            CanMoveFoward(targetPosition);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z))  // else if because we cannot move in both Vertical et Horizontal axis 
        {
            targetPosition = characterPos + Vector3.forward;
            LookAt(targetPosition);
            CanMoveFoward(targetPosition);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Q))
        {
            targetPosition = characterPos + Vector3.left;
            LookAt(targetPosition);
            CanMoveFoward(targetPosition);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            targetPosition = characterPos + Vector3.right;
            LookAt(targetPosition);
            CanMoveFoward(targetPosition);
        }
    }

    /// <summary>
    /// Rotate the character towards the direction to look at
    /// </summary>
    private void LookAt(Vector3 positionToLookAt)
    {
        Vector3 direction = positionToLookAt - characterPos;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction); // mathematical way to deal with rotation
            Vector3 rotation = Quaternion.Lerp(this.transform.rotation, lookRotation, 10f).eulerAngles;
            this.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f); // will just aim in the (x,z) plan
        }
    }

    /// <summary>
    /// Chack if the character can move to a given direction 
    /// </summary>
    private void CanMoveFoward(Vector3 targetPosition)
    {
        if (CalculatePath(targetPosition) && NoCollision(targetPosition))
        {
            // Moving
            animator.SetBool("isWalking", true);
        }
    }
    
    /// <summary>
    /// Chack if the targeted position is walkable (a tile exist)
    /// </summary>
    /// <param name = target > Vector3 targeted tile position.</param>
    /// <returns>Return either true if a walkable tile exist with the given postion or false if not</returns>
    private bool CalculatePath(Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(target, path);
        if (path.status == NavMeshPathStatus.PathInvalid)
        {
            Debug.Log("no");
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Chack if the targeted position is walkable (a wall doesn't exist)
    /// </summary>
    /// <param name = direction > Vector3 targeted wall position.</param>
    /// <returns>Return either false if a wall tile exist with the given postion or true if not</returns>
    private bool NoCollision(Vector3 direction)
    {
        if (MapManager.mapInstance.GetWall(direction.x, direction.z))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Chack if a colision with a Wall occured with character
    /// </summary>
    /// <param name = direction > Vector3 targeted wall position.</param>
    /// <returns>Return either false if a wall tile exist with the given postion or true if not</returns>
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Wall")
        {
            Debug.Log("Collision detected");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            // Destroy(col.gameObject);
        }
    }

    /// <summary>
    /// Show/hide mouse cursor
    /// </summary>
    private void ShowMouse()
    {
        if (Input.GetMouseButtonDown(1)) // Right click down
        {
            mouseLock = !mouseLock;
            if (mouseLock)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

}
