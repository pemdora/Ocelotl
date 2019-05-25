using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class responsible for main character controlls :
/// Character is able to Move with WASD keys
/// Show And Hide cursor with Right Click
/// Check if it has a collision with a Wall
/// Check if the tile with a given position is walkable or not
/// </summary>

[RequireComponent(typeof(NavMeshAgent))]
public class MainCharacterController : MonoBehaviour
{

    private static bool mouseLock = true;
    private Vector3 front, back, left, right;
    private NavMeshAgent agent;
    public bool lockControls;

    #region PositionVariables
    private Vector3 characterPos;
    private Vector3 targetPosition;
    #endregion

    #region AnimationVariables
    private float speed;
    public Animator animator;
    #endregion

    #region Events
    public delegate void WallCollision(); // delegate type is similar to a method signature,  similar to function pointers in C++
    public static event WallCollision OnWallCollisionEvent; // event variable attached to delegate function, static so that it can be called outside of our class
    public delegate void ReachedGoal();
    public static event ReachedGoal ReachedGoalEvent;
    public delegate void PressingEnter();
    public static event PressingEnter PressingEnterEvent;
    private bool hasfinished;
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

        characterPos = this.transform.position; // since player position is not 100% accurate, we generate an accurate variable position
        targetPosition = this.transform.position;

        speed = 2f;
        animator = GetComponent<Animator>();
        hasfinished = false;
        lockControls = false; // player can move at the begining at the level

        front = Vector3.forward;
        back = Vector3.back;
        left = Vector3.left;
        right = Vector3.right;

    }

    /// <summary>
    /// Get input and translate character 
    /// </summary>
    private void GetMovementInput()
    {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                targetPosition = characterPos + back;
                LookAt(targetPosition);
                CanMoveFoward(targetPosition);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z))  // else if because we cannot move in both Vertical et Horizontal axis 
            {
                targetPosition = characterPos + front;
                LookAt(targetPosition);
                CanMoveFoward(targetPosition);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Q))
            {
                targetPosition = characterPos + left;
                LookAt(targetPosition);
                CanMoveFoward(targetPosition);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                targetPosition = characterPos + right;
                LookAt(targetPosition);
                CanMoveFoward(targetPosition);
            }
    }

    public void ChangeAxis(char upkeyAxis)
    {
        switch (upkeyAxis)
        {
            case 'f': // front : default camera axis
                front = Vector3.forward;
                back = Vector3.back;
                left = Vector3.left;
                right = Vector3.right;
                break;
            case 'b': // back : camera turned 2 times
                // if the players enter up arrow it will move along -z axis
                front = Vector3.back;
                back = Vector3.forward;
                left = Vector3.right;
                right = Vector3.left;
                break;
            case 'r': // right
                // if the players enter up arrow it will move along +X axis
                front = Vector3.right;
                back = Vector3.left;
                left = Vector3.forward;
                right = Vector3.back;
                break;
            case 'l': // left 
                // if the players enter up arrow it will move along -X axis
                front = Vector3.left;
                back = Vector3.right;
                left = Vector3.back;
                right = Vector3.forward;
                break;
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

    #region Events trigger

    /// <summary>
    /// Check player position, inputs and move player or display mouse
    /// </summary>
    private void Update()
    {
        // if we are not locking player controls, detect events from player
        if (!lockControls)
        {
            if (animator.GetBool("isWalking")) // if the player is moving
            {
                // if the player has reached the tiled if wanted to move
                if (Vector3.Distance(targetPosition, transform.position) < 0.34f) // check character's position with mathematical aproximation
                {
                    animator.SetBool("isWalking", false); // set walking animation
                    characterPos = targetPosition; // update characterPos
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime); // translate
                }
            }
            else
            {
                // if the player press "Space" and is not moving 
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (PressingEnterEvent != null) // if we have at least one subscriber
                        PressingEnterEvent();
                    else
                        Debug.LogError("No subscriber on PressingEnterEvent");
                }
                else
                {
                    GetMovementInput();
                }
            }

            if (!hasfinished && Vector3.Distance(this.transform.position, MapManager.mapInstance.goal.position) < 0.34f)
            {
                animator.SetBool("isWalking", false); // stop moving
                if (ReachedGoalEvent != null)
                    ReachedGoalEvent();
                else
                    Debug.LogError("No subscriber on ReachedGoalEvent");
                lockControls = true;
                hasfinished = true;
            }
        }
        ShowMouse(); // still showing/hide mouse cursor at anytime
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
            if (OnWallCollisionEvent != null)
                OnWallCollisionEvent();
            else
                Debug.LogError("No subscriber on OnWallCollisionEvent");
        }
    }
    #endregion

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
                Cursor.visible = true;
            }
            else
            {
                Cursor.visible = false;
            }
        }
    }

}
