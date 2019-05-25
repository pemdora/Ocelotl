using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public Camera worldCamera;

    [Header("World Camera variables")]  // Camera rotation for candidate
    public Transform orbit;
    public float fDistance = 50;
    public float fSpeed = 5f;
    public int way = 1;
    public bool turningcamera = false;
    public Transform targetPosition;
    private int cameraPos = 0; // we have 4 postion
    /*
     * Camera positions : 
     
          _2_
        1|___|3
           0  
           
     */

    //SINGLETON
    public static CameraManager cameraManagerinstance;
    void Awake()
    {
        if (cameraManagerinstance != null)
        {
            Debug.LogError("More than one GameManager in scene");
            return;
        }
        else
        {
            cameraManagerinstance = this;
        }
    }
   
	// Update is called once per frame
	void Update ()
    {
        if (turningcamera) // if the player is turning camera
        {
            if (Vector3.Distance(this.targetPosition.position, worldCamera.transform.position) < 0.34f) // turn 90degrees
            {
                turningcamera = false;
            }
            else
            {
                float orbitCircumference = 2F * fDistance * Mathf.PI;
                float distanceDegrees = (fSpeed / orbitCircumference) * 2 * Mathf.PI;
                worldCamera.transform.RotateAround(orbit.transform.position, Vector3.up, distanceDegrees*way);
            }
        }
        else // we can get input
        { 
            if (Input.GetKeyDown(KeyCode.E)) // right
            {
                way = -1;
                targetPosition.transform.RotateAround(orbit.transform.position, Vector3.up, 90 * way);
                cameraPos = (cameraPos - 1) % 4;
                ChangeControls();
                turningcamera = true;
            }
            else if (Input.GetKeyDown(KeyCode.A)) // left 
            {
                way = 1;
                targetPosition.transform.RotateAround(orbit.transform.position, Vector3.up, 90 * way);
                cameraPos = (cameraPos + 1) % 4;
                ChangeControls();
                turningcamera = true;
            }
        }
    }

    void ChangeControls()
    {
        if (cameraPos < 0)
            cameraPos = 3;
        switch (cameraPos)
        {
            case 0:
                MainCharacterController.characterController.ChangeAxis('f');
                break;
            case 1:
                MainCharacterController.characterController.ChangeAxis('r');
                break;
            case 2:
                MainCharacterController.characterController.ChangeAxis('b');
                break;
            case 3:
                MainCharacterController.characterController.ChangeAxis('l');
                break;
        }
    }
}
