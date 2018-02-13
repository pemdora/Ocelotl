using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for Map controlls :
/// </summary>
public class MapManager : MonoBehaviour {

    [Header("Maps variables")]
    public Transform map1; // TileList
    private Transform[] tilesMap1;
    private bool map1active;

    public Transform map2; // TileList
    private Transform[] tilesMap2;
    private bool map2active;

    public Transform[] tilesMap;

    public static MapManager mapInstance;
    //SINGLETON
    void Awake()
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

    // Use this for initialization
    void Start () {

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
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            map1active = !map1active;
            map2active = !map2active;
            map1.gameObject.SetActive(map1active);
            map2.gameObject.SetActive(map2active);
            if (map1.gameObject.activeInHierarchy)
                tilesMap = tilesMap1;
            else
                tilesMap = tilesMap2;

        }
	}

    public bool GetWall(float x, float z)
    {
        foreach (Transform go in tilesMap)
        {
            TileBlock tile = go.GetComponent<TileBlock>();
            if (tile!= null)
            {
                if(tile.x == x && tile.z == z)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
