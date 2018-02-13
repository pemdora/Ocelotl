using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBlock : MonoBehaviour
{
    [Header("Tile variables")]
    public float x;
    public float z;
    
    /// <summary>
    /// Initialize variables
    /// </summary>
    void Start()
    {
        this.x = this.transform.position.x;
        this.z = this.transform.position.z;
    }

}
