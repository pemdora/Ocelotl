using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for Map controlls :
/// </summary>
public class MapManager : MonoBehaviour
{

    [Header("Maps variables")]
    // Active map
    public Transform[] tilesMap;
    #region map1
    public Transform map1; // TileList
    private Transform[] tilesMap1;
    private bool map1active;
    #endregion
    #region map2
    public Transform map2; // TileList
    private Transform[] tilesMap2;
    private bool map2active;
    #endregion
    private bool mapSwap;


    public static MapManager mapInstance;
    //SINGLETON
    /// <summary>
    /// Initialize singleton instance
    /// </summary>
    private void Awake()
    {
        if (mapInstance != null)
        {
            Debug.LogError("More than one Map Manager in scene");
            return;
        }
        else
        {
            mapInstance = this;
        }
    }

    /// <summary>
    /// Initialize variables
    /// </summary>
    private void Start()
    {

        // Get child elements from map object
        tilesMap1 = new Transform[map1.childCount];
        for (int i = 0; i < tilesMap1.Length; i++)
        {
            tilesMap1[i] = map1.GetChild(i);
        }

        // Get child elements from map object
        tilesMap2 = new Transform[map2.childCount];
        for (int i = 0; i < tilesMap2.Length; i++)
        {
            tilesMap2[i] = map2.GetChild(i);
        }

        map1active = true;
        map2active = false;
        map1.gameObject.SetActive(map1active);
        map2.gameObject.SetActive(map2active);
        tilesMap = tilesMap1;
        mapSwap = true;
    }

    /// <summary>
    /// Get User input for switching Map
    /// </summary>
    private void Update()
    {
        // if the player press "Space" and is not moving and not swaping maps
        if (Input.GetKeyDown(KeyCode.Space) && !MainCharacterController.characterController.animator.GetBool("isWalking") && mapSwap) 
        {
            IEnumerator coroutine = SwapMaps();
            StartCoroutine(coroutine);
        }
    }

    /// <summary>
    /// Function that swaps maps and play wall animation
    /// </summary>
    /// <returns>Return true if a wall with the given X and Z postion or false if not</returns>
    private IEnumerator SwapMaps()
    {
        map1active = !map1active;
        map2active = !map2active;
        mapSwap = false;
        map1.gameObject.SetActive(map1active);
        map2.gameObject.SetActive(map2active);
        if (map1.gameObject.activeInHierarchy)
            tilesMap = tilesMap1;
        else
            tilesMap = tilesMap2;
        foreach (Transform tile in tilesMap)
        {
            tile.position += new Vector3(0, 5, 0);
        }
        yield return new WaitForSeconds(1f);
        #region WaitAndDo // this will be executed only when the coroutine have finished
        mapSwap = true;
        #endregion
    }

    /// <summary>
    /// Chack if a wall exists in the active map with a given "x" and "y" position
    /// </summary>
    /// <param name = x > float position in X axis.</param>
    /// <param name = y > float position in Z axis.</param>
    /// <returns>Return true if a wall with the given X and Z postion or false if not</returns>
    public bool GetWall(float x, float z)
    {
        foreach (Transform tile in tilesMap)
        {
            TileBlock tileBlock = tile.GetComponent<TileBlock>();
            if (tileBlock != null)
            {
                if (tileBlock.x == x && tileBlock.z == z)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
