using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "MapDuoData", menuName = "ScriptableObject/MapDuoData", order = 1)]

public class MapDuoData : ScriptableObject
{
    public int length;
    public int level;
    public int submapLevel;

    // TODO store maps in txt files
    // Array map (lighter than storing with list of game objects)
    public int[,] map1;
    public int[,] map2;

    public void InitMap()
    {
        switch (level)
        {
            case 0:
                map1 = new int[8, 8] { // map of 8 line and 8 colums
                { 0, 0, 1, 0, 1, 0, 0, 0 },
                { 0, 0, 1, 0, 1, 0, 0, 0 },
                { 1, 1, 1, 0, 1, 0, 1, 0 },
                { 0, 0, 0, 0, 1, 0, 1, 1 },
                { 1, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 1, 1, 1, 1, 1 },
                { 0, 1, 1, 1, 0, 0, 0, 0 }
                };

                map2 = new int[8, 8] { // map of 8 line and 8 colums
                { 0, 0, 0, 0, 1, 1, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 1, 1 },
                { 1, 1, 1, 0, 0, 0, 0, 0 },
                { 1, 0, 1, 0, 1, 1, 1, 0 },
                { 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 1, 1, 1, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0 }
                };
                break;
            case 1:
                break;
        }
    }
}

