using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class responsible for main character controlls :
/// Character is able to Move with WASD keys
/// Show And Hide cursor with Right Click
/// </summary>

[RequireComponent(typeof(NavMeshAgent))]
public class MainCharacterController : MonoBehaviour {

    public float walkSpeed = 10f;
    private bool mouseLock;
    private NavMeshAgent agent;

    // Use this for initialization
    void Start()
    {
        // Hide the cursor to begin with
        Cursor.lockState = CursorLockMode.Locked;
        mouseLock = true;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    // Get input and translate character 
    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)|| Input.GetKeyDown(KeyCode.S))
        {
            Vector3 targetPosition = this.transform.position + Vector3.back;
            LookAt(targetPosition);
            // Moving
            agent.SetDestination(targetPosition);
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z))  // else if because we cannot move in both Vertical et Horizontal axis 
        {
            Vector3 targetPosition = this.transform.position + Vector3.forward;
            LookAt(targetPosition);
            // Moving
            agent.SetDestination(targetPosition);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 targetPosition = this.transform.position + Vector3.left;
            LookAt(targetPosition);
            // Moving
            agent.SetDestination(targetPosition);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            Vector3 targetPosition = this.transform.position + Vector3.right;
            LookAt(targetPosition);
            // Moving
            agent.SetDestination(targetPosition);
        }
    }

    public void LookAt(Vector3 positionToLookAt)
    {
        Vector3 direction = positionToLookAt - this.transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction); // mathematical way to deal with rotation
            Vector3 rotation = Quaternion.Lerp(this.transform.rotation, lookRotation,10f).eulerAngles;
            this.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f); // will just aim in the (x,z) plan
        }
    }

    /*
    public void CalculatePath(Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(target, path);
        if (path.status == NavMeshPathStatus.PathInvalid)
        {
            Debug.Log("no");
        }
    }*/

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
