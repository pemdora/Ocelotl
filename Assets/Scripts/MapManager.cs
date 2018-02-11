using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    [Header("Maps variables")]
    public Transform map1; // TileList
    private Transform[] tilesMap1;
    private bool map1active;

    public Transform map2; // TileList
    private Transform[] tilesMap2;
    private bool map2active;
    
    // Use this for initialization
    void Start () {

        // Get child elements from map object
        tilesMap1 = new Transform[map1.childCount];
        for (int i = 0; i < tilesMap1.Length; i++)
        {
            tilesMap1[i] = map1.GetChild(i);
        }

        map1active = true;
        map2active = false;
        map1.gameObject.SetActive(map1active);
        map2.gameObject.SetActive(map2active);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            map1active = !map1active;
            map2active = !map2active;
            map1.gameObject.SetActive(map1active);
            map2.gameObject.SetActive(map2active);
        }
	}
}
