using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for Map controlls :
/// </summary>
public class MapManager : MonoBehaviour
{

    [Header("Maps variables")]
    private List<Transform> tilesMap; // Active map
    #region map1
    public Transform map1; // TileList
    private List<Transform> tilesMap1;
    private bool map1active;
    #endregion
    #region map2
    public Transform map2; // TileList
    private List<Transform> tilesMap2;
    private bool map2active;
    #endregion
    #region Maps in Array
    // Array map (lighter than storing with list of game objects)
    public int[,] map1Array = new int[8, 8] { // map of 8 line and 8 colums
        { 0, 0, 1, 1, 0, 0, 1, 0 },
        { 0, 1, 0, 0, 1, 1, 0, 0 },
        { 1, 1, 1, 0, 0, 1, 0, 1 },
        { 0, 1, 0, 1, 1, 1, 0, 0 },
        { 0, 1, 0, 0, 0, 0, 0, 1 },
        { 1, 1, 1, 0, 1, 1, 1, 1 },
        { 0, 0, 1, 0, 0, 0, 0, 1 },
        { 1, 0, 1, 0, 0, 1, 1, 0 }
    };
    public int[,] map2Array = new int[8, 8] { // map of 8 line and 8 colums
        { 0, 0, 0, 1, 1, 0, 1, 0 },
        { 0, 1, 0, 1, 0, 1, 1, 1 },
        { 1, 0, 1, 0, 1, 0, 1, 0 },
        { 0, 1, 1, 0, 0, 0, 1, 0 },
        { 0, 0, 1, 0, 1, 1, 1, 1 },
        { 0, 0, 0, 1, 0, 0, 1, 0 },
        { 0, 1, 1, 1, 1, 1, 0, 1 },
        { 0, 1, 0, 0, 0, 1, 0, 0 }
    };
    public int[,] map3Array = new int[8, 8] { // map of 8 line and 8 colums
        { 0, 1, 0, 1, 0, 0, 1, 0 },
        { 0, 1, 0, 0, 1, 0, 1, 1 },
        { 1, 0, 0, 1, 0, 1, 0, 0 },
        { 0, 1, 0, 0, 1, 0, 1, 0 },
        { 0, 1, 0, 1, 0, 1, 1, 0 },
        { 0, 0, 1, 0, 0, 0, 1, 0 },
        { 0, 0, 1, 0, 1, 0, 1, 0 },
        { 0, 0, 1, 0, 1, 0, 1, 0 }
    };
    public int[,] map4Array = new int[8, 8] { // map of 8 line and 8 colums
        { 1, 1, 0, 0, 0, 1, 0, 0 },
        { 0, 0, 1, 0, 1, 0, 1, 0 },
        { 0, 1, 0, 1, 0, 0, 1, 0 },
        { 0, 0, 0, 1, 0, 0, 0, 1 },
        { 1, 1, 0, 1, 0, 1, 1, 0 },
        { 0, 0, 1, 0, 1, 0, 1, 0 },
        { 1, 1, 0, 0, 0, 0, 1, 1 },
        { 0, 0, 1, 0, 0, 0, 0, 0 }
    };
    #endregion
    public bool mapSwap;
    private List<int[,]> mapList;
    public static int sublvl = 0;
    public static int arraylength = 8; // Arrays are symetrics

    [Header("Goal")]
    public Transform goal;
    private List<Vector3> goalList;
    
    [Header("Wall Prefab")]
    public GameObject wall;
    
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
        #region Instanciate maps
        this.mapList = new List<int[,]>();
        mapList.Add(map1Array);
        mapList.Add(map2Array);
        mapList.Add(map3Array);
        mapList.Add(map4Array);
        tilesMap1 = new List<Transform>();
        tilesMap2 = new List<Transform>();
        for (int i = 0; i < arraylength; i++)
        {
            for (int j = 0; j < arraylength; j++)
            {
                if (mapList[sublvl][i, j] == 1)
                {
                    Vector3 position = new Vector3(i, 0, j);
                    // Instantiate the wall, set its position
                    GameObject wallObj = (GameObject)Instantiate(this.wall);
                    wallObj.transform.position = position;
                    wallObj.transform.parent = map1.transform;
                    tilesMap1.Add(wallObj.transform);
                }
                if (mapList[sublvl + 1][i, j] == 1)
                {
                    Vector3 position = new Vector3(i, 0, j);
                    // Instantiate the wall, set its position
                    GameObject wallObj = (GameObject)Instantiate(this.wall);
                    wallObj.transform.position = position;
                    wallObj.transform.parent = map2.transform;
                    tilesMap2.Add(wallObj.transform);
                }
            }
        }
        #endregion

        this.goalList = new List<Vector3>();
        goalList.Add(new Vector3(7, 0, 7));
        goalList.Add(new Vector3(0, 0, 7));
        this.goal.transform.position = goalList[sublvl / 2];

        map1active = true;
        map2active = false;
        map1.gameObject.SetActive(map1active);
        map2.gameObject.SetActive(map2active);
        tilesMap = tilesMap1;
        mapSwap = true;
    }

    /// <summary>
    /// Function called when the object becomes enabled and active
    /// </summary>
    public void OnEnable()
    {
        MainCharacterController.PressingEnterEvent += SwitchMap; // Subscribing to event
    }

    /// <summary>
    /// Function called when the object becomes disabled and inactive
    /// </summary>
    public void OnDisable()
    {
        MainCharacterController.PressingEnterEvent -= SwitchMap;
    }
    
    /// <summary>
    /// Switch Map, subscriber from MainCharacterController event on Space
    /// </summary>
    private void SwitchMap()
    {
        if (mapSwap) // if we are already not swaping maps
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
        Transform tile = tilesMap.Find(tileFinding => (tileFinding.GetComponent<TileBlock>().x == x && tileFinding.GetComponent<TileBlock>().z == z));
        if (tile != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
