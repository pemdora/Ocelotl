using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBlock : MonoBehaviour
{
    public float x;
    public float z;

    // Use this for initialization
    void Start()
    {
        this.x = this.transform.position.x;
        this.z = this.transform.position.z;
    }

}
