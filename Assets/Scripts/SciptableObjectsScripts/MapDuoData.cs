using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "MapDuoData", menuName = "ScriptableObject/MapDuoData", order = 1)]

public class MapDuoData : ScriptableObject
{
    // For now, Maps are all squares
    public int arraylength;
    public int level;
    public int submapLevel;

    // TODO store maps in txt files
    // Array map (lighter than storing with list of game objects)
    public int[,] map1;
    public int[,] map2;

    public Vector3 startPoint;
    public Vector3 goalPoint;

    public void InitMap()
    {
        switch (level)
        {
            case 0:
                startPoint = new Vector3(0, 0, 0);
                goalPoint = new Vector3(7, 0, 7);
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
                switch (submapLevel)
                {
                    case 0:
                        startPoint = new Vector3(0, 0, 0);
                        goalPoint = new Vector3(7, 0, 7);
                        map1 = new int[8, 8] { // map of 8 line and 8 colums
                        //Start at v point
                            //v
                            { 0, 0, 1, 1, 0, 0, 1, 0 },
                            { 0, 1, 0, 0, 1, 1, 0, 0 },
                            { 1, 1, 1, 0, 0, 1, 0, 1 },
                            { 0, 1, 0, 1, 1, 1, 0, 0 },
                            { 0, 1, 0, 0, 0, 0, 0, 1 },
                            { 1, 1, 1, 0, 1, 1, 1, 1 },
                            { 0, 0, 1, 0, 0, 0, 0, 1 },
                            { 1, 0, 1, 0, 0, 1, 1, 0 } // end
                         };
                        map2 = new int[8, 8] { // map of 8 line and 8 colums
                            { 0, 0, 0, 1, 1, 0, 1, 0 },
                            { 0, 1, 0, 1, 0, 1, 1, 1 },
                            { 1, 0, 1, 0, 1, 0, 1, 0 },
                            { 0, 1, 1, 0, 0, 0, 1, 0 },
                            { 0, 0, 1, 0, 1, 1, 1, 1 },
                            { 0, 0, 0, 1, 0, 0, 1, 0 },
                            { 0, 1, 1, 1, 1, 1, 0, 1 },
                            { 0, 1, 0, 0, 0, 1, 0, 0 }
                        };
                        break;
                    case 1:
                        startPoint = new Vector3(0, 0, 0);
                        goalPoint = new Vector3(0, 0, 7);
                        map1 = new int[8, 8] { // map of 8 line and 8 colums
                            { 0, 1, 0, 1, 0, 0, 1, 0 },
                            { 0, 1, 0, 0, 1, 0, 1, 1 },
                            { 1, 0, 0, 1, 0, 1, 0, 0 },
                            { 0, 1, 0, 0, 1, 0, 1, 0 },
                            { 0, 1, 0, 1, 0, 1, 1, 0 },
                            { 0, 0, 1, 0, 0, 0, 1, 0 },
                            { 0, 0, 1, 0, 1, 0, 1, 0 },
                            { 0, 0, 1, 0, 1, 0, 1, 0 }
                        };
                        map2 = new int[8, 8] { // map of 8 line and 8 colums
                            { 1, 1, 0, 0, 0, 1, 0, 0 },
                            { 0, 0, 1, 0, 1, 0, 1, 0 },
                            { 0, 1, 0, 1, 0, 0, 1, 0 },
                            { 0, 0, 0, 1, 0, 0, 0, 1 },
                            { 1, 1, 0, 1, 0, 1, 1, 0 },
                            { 0, 0, 1, 0, 1, 0, 1, 0 },
                            { 1, 1, 0, 0, 0, 0, 1, 1 },
                            { 0, 0, 1, 0, 0, 0, 0, 0 }
                        };
                        break;
                }
                break;
            case 666:
                map1 = new int[3, 3] { // map of 8 line and 8 colums
                    { 0, 0, 0 },
                    { 1, 1, 1 },
                    { 0, 0, 0 }
                };
                map2 = new int[3, 3] { // map of 8 line and 8 colums
                    { 0, 0, 0 },
                    { 0, 1, 1 },
                    { 0, 1, 0 }
                };
                break;
        }
    }
}

