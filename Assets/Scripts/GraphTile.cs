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

    public GraphTile predecessor;

    public GraphTile(float x, float z)
    {
        this.neighbors = new List<GraphTile>(4); // 4 neighbors max
        this.x = x;
        this.z = z;
        this.tileGameObject = null;
        this.predecessor = null;
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
    public void ChangeColor()
    {
        if (this.tileGameObject != null)
        {
            Animator animator = this.tileGameObject.GetComponent<Animator>();
            if (animator.gameObject.activeSelf)
            {
                animator.SetTrigger("soluce");
            }
            
        }
    }

}