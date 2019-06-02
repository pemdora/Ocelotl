using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representation of a single node of a graph called GraphTile
/// Each node has 4 neighbors maximum
/// </summary>
public class GraphTile
{
    public List<GraphTile> neighbors;
    public float x;
    public float z;
    public Transform tileGameObject;
    public int value; // value of cell, 0 always walkable, 1 walkable from map 1 to map 2, 2 walkable from map 2 to map 1 

    public GraphTile predecessor;

    public GraphTile(float x, float z)
    {
        this.neighbors = new List<GraphTile>(4); // 4 neighbors max
        this.x = x;
        this.z = z;
        this.tileGameObject = null;
        this.predecessor = null;
    }

    public GraphTile(float x, float z, int value)
    {
        this.neighbors = new List<GraphTile>(4); // 4 neighbors max
        this.x = x;
        this.z = z;
        this.tileGameObject = null;
        this.predecessor = null;
        this.value = value;
    }

    public Transform TileGameObject
    {
        set
        {
            this.tileGameObject = value;
        }
    }


    /// <summary>
    /// Change color of the current tile
    /// </summary>
    public void ChangeColor(List<Transform> floormap, MapManager.GroundColor color)
    {
        if (this.tileGameObject != null)
        {
            Animator animator = this.tileGameObject.GetComponent<Animator>();
            if (animator.gameObject.activeSelf)
            {
                animator.SetTrigger(color.ToString());
            }

        }
        else
        {
            Transform tile = floormap.Find(obj => obj.position.x == x && obj.position.z == z );
            if (tile != null)
            {
                tileGameObject = tile;
                Animator animator = this.tileGameObject.GetComponent<Animator>();
                animator.SetTrigger(color.ToString());
            }
            else
                Debug.Log("no tile");
        }
    }

}