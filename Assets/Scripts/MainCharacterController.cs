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
    public Vector3 start;
    public Vector3 characterPos;
    public Vector3 targetPosition;
    private float speed;
    private bool move;
    

    // Use this for initialization
    void Start()
    {
        // Hide the cursor to begin with
        Cursor.lockState = CursorLockMode.Locked;
        mouseLock = true;
        agent = GetComponent<NavMeshAgent>();
        start = this.transform.position;
        characterPos = this.transform.position;
        targetPosition = this.transform.position;
        move = false;
        speed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            if (Vector3.Distance(targetPosition,transform.position)<0.34f)
            {
                move = false;
                characterPos = targetPosition;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position,targetPosition,speed*Time.deltaTime);
            }
        }
        else
        {
            GetMovementInput();
        }
        ShowMouse();
    }

    // Get input and translate character 
    private void GetMovementInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            targetPosition = characterPos + Vector3.back;
            LookAt(targetPosition);
            MoveFoward(targetPosition);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z))  // else if because we cannot move in both Vertical et Horizontal axis 
        {
            targetPosition = characterPos + Vector3.forward;
            LookAt(targetPosition);
            MoveFoward(targetPosition);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Q))
        {
            targetPosition = characterPos + Vector3.left;
            LookAt(targetPosition);
            MoveFoward(targetPosition);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            targetPosition = characterPos + Vector3.right;
            LookAt(targetPosition);
            MoveFoward(targetPosition);
        }
    }

    public void LookAt(Vector3 positionToLookAt)
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

    public void MoveFoward(Vector3 targetPosition)
    {
        if (CalculatePath(targetPosition) && NoCollision(targetPosition))
        {
            Debug.Log("move");
            // Moving
            move = true;
        }
    }

    public bool CalculatePath(Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(target, path);
        if (path.status == NavMeshPathStatus.PathInvalid)
        {
            Debug.Log("no");
            move = false;
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool NoCollision(Vector3 direction)
    {
        if (MapManager.mapInstance.GetWall(direction.x, direction.z))
        {
            move = false;
            return false;
        }
        else
        {
            return true;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Wall")
        {
            Debug.Log("Collision detected");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            // Destroy(col.gameObject);
        }
    }

    // Show the cursor
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
