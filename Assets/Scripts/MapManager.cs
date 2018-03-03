using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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
    /*
    public int[,] map5Array = new int[8, 8] { // map of 8 line and 8 colums
        { 0, 1, 0, 1, 0, 0, 1, 0 },
        { 0, 1, 0, 0, 1, 0, 1, 1 },
        { 1, 0, 0, 1, 0, 1, 0, 0 },
        { 0, 1, 0, 0, 1, 0, 1, 0 },
        { 0, 1, 0, 1, 0, 1, 1, 0 },
        { 0, 0, 1, 0, 0, 0, 1, 0 },
        { 0, 0, 1, 0, 1, 0, 1, 0 },
        { 0, 0, 1, 0, 1, 0, 1, 0 }
    };
    public int[,] map6Array = new int[8, 8] { // map of 8 line and 8 colums
        { 1, 1, 0, 0, 0, 1, 1, 0 },
        { 0, 0, 1, 0, 1, 0, 1, 0 },
        { 0, 1, 0, 1, 0, 0, 1, 0 },
        { 0, 0, 0, 1, 0, 0, 0, 1 },
        { 1, 1, 0, 1, 0, 1, 1, 0 },
        { 0, 0, 1, 0, 1, 0, 1, 0 },
        { 1, 1, 0, 0, 0, 0, 1, 1 },
        { 0, 0, 1, 0, 0, 0, 0, 0 }
    };*/
    #endregion
    public bool mapSwap;
    private List<int[,]> mapList;
    public static int sublvl = 0;
    public static int arraylength = 8; // Arrays are symetrics

    [Header("Goal")]
    public Transform goal;
    private List<Vector3> goalList;
    private List<Vector3> startList;

    [Header("Wall Prefab")]
    public GameObject wall;

    [Header("Walkable Graph")]
    public Transform floor;
    private List<Transform> floormap;
    private List<GraphTile> walkableGraph; // the walkable area that the player can walk through by switching maps
    private List<GraphTile> obstacleGraph1; // the graph of walls
    private List<GraphTile> obstacleGraph2; // the graph of walls
    private GraphTile startTile;
    private GraphTile endTile;


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
        #region Initializing objects
        this.mapList = new List<int[,]>();
        mapList.Add(map1Array);
        mapList.Add(map2Array);
        mapList.Add(map3Array);
        mapList.Add(map4Array);
        tilesMap1 = new List<Transform>();
        tilesMap2 = new List<Transform>();
        walkableGraph = new List<GraphTile>();
        obstacleGraph1 = new List<GraphTile>();
        obstacleGraph2 = new List<GraphTile>();

        // Get child elements from map object
        floormap = new List<Transform>();
        for (int i = 0; i < floor.childCount; i++)
        {
            floormap.Add(floor.GetChild(i));
        }
        #endregion


        // Building walls and graphs
        for (int i = 0; i < arraylength; i++) // x values
        {
            for (int j = 0; j < arraylength; j++) // z values
            {
                // 2 maps/sublvl
                if (mapList[2*sublvl][i, j] == 1) // If we got a 1 => wall position in the map array
                {
                    // Instanciate wall objects for 1st map
                    InstanciateWall(i, j, tilesMap1, map1);
                    GraphTile tile = new GraphTile(i, j);
                    obstacleGraph1.Add(tile);
                }
                else // its a walkable tile
                {
                    CheckAndAddGraphTile(i, j);
                }

                // Map 2
                if (mapList[2*sublvl+1][i, j] == 1)
                {
                    // Instanciate wall objects for 2d map
                    InstanciateWall(i, j, tilesMap2, map2);
                    GraphTile tile = new GraphTile(i, j);
                    obstacleGraph2.Add(tile);
                }
                else
                {
                    CheckAndAddGraphTile(i, j);
                }
            }
        }


        #region Manualy defining start and goal points
        //Start Point
        this.startList = new List<Vector3>();
        goalList.Add(new Vector3(0, 0, 0));
        goalList.Add(new Vector3(0, 0, 0));
        if (walkableGraph.Exists(item => (item.x == startList[sublvl].x) && (item.z == startList[sublvl].z))) // if startTile already exist in walkableGraph
        {
            startTile = walkableGraph.Find(item => (item.x == startList[sublvl].x) && (item.z == startList[sublvl].z));
        }
        else
        {
            Debug.Log("Start tile wasn't in graph");
            GraphTile tile = new GraphTile(0, 0)
            {
                TileGameObject = floormap.Find(obj => obj.position.x == 0 && obj.position.z == 0)
            };
            startTile = tile;
            walkableGraph.Add(tile);
        }

        //GoalPoints
        this.goalList = new List<Vector3>();
        goalList.Add(new Vector3(7, 0, 7));
        goalList.Add(new Vector3(0, 0, 7));
        this.goal.transform.position = goalList[sublvl];

        endTile = walkableGraph.Find(item => (item.x == goalList[sublvl].x) && (item.z == goalList[sublvl].z));
        #endregion

        // Get Neighbors for walkable tiles
        foreach (GraphTile tempTile in walkableGraph)
        {
            GetNeighbors(walkableGraph, tempTile);
        }
        #endregion

        // Remove unwalkable neighbors tile in walkable graph
        foreach (GraphTile tempTile in obstacleGraph1)
        {
            RemoveUnwalkableTile(obstacleGraph2, tempTile);
        }
        foreach (GraphTile tempTile in obstacleGraph2)
        {
            RemoveUnwalkableTile(obstacleGraph1, tempTile);
        }

        map1active = true;
        map2active = false;
        map1.gameObject.SetActive(map1active);
        map2.gameObject.SetActive(map2active);
        tilesMap = tilesMap1;
        mapSwap = true;
        Debug.Log(ShortestPath());
    }

    /// <summary>
    /// Function used to create a wall object in the world
    /// and attach this wall to a map
    /// </summary>
    /// <param name = x > float position in X axis.</param>
    /// <param name = z > float position in Z axis.</param>
    /// <param name = map > Map parent transform </param>
    /// <param name = mapList > List of transforms for a map </param>
    public void InstanciateWall(int x, int z, List<Transform> mapList, Transform map)
    {
        Vector3 position = new Vector3(x, 0, z);
        // Instantiate the wall, set its position
        GameObject wallObj = (GameObject)Instantiate(this.wall);
        wallObj.transform.position = position;
        wallObj.transform.parent = map.transform;
        mapList.Add(wallObj.transform);
    }

    /// <summary>
    /// Function used to check if tile already exist in list
    /// if not, add the tle to walkableGraph
    /// </summary>
    /// <param name = x > float position in X axis.</param>
    /// <param name = z > float position in Z axis.</param>
    public void CheckAndAddGraphTile(int x, int z)
    {
        GraphTile tile = new GraphTile(x, z);
        bool containsTile = walkableGraph.Any(item => item.x == tile.x && item.z == tile.z);
        if (!containsTile)
        {
            tile.TileGameObject = floormap.Find(obj => obj.position.x == x && obj.position.z == z);
            walkableGraph.Add(tile);
        }
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
            tile.position += new Vector3(0, 2, 0);
        }
        yield return new WaitForSeconds(2f);
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
        Transform tile = tilesMap.Find(tileFinding => (tileFinding.position.x == x && tileFinding.position.z == z));
        if (tile != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Chack if a shortest path exists in the graph of walkable tile
    /// Applying A star without time calculation (breadth First search)
    /// </summary>
    /// <returns>Return true if a path exists or false if not</returns>
    public bool ShortestPath()
    {
        // Since GraphTile is a structure it can't be directly null, so we transformed it to GraphTile? or Nullable<GraphTile>
        List<GraphTile> openList = new List<GraphTile>(); // set of nodes to be evaluated
        List<GraphTile> closedList = new List<GraphTile>(); // set of nodes already evaluated

        openList.Add((GraphTile)this.startTile);
        while (openList.Count != 0) // while the list is not empty
        {
            GraphTile current = null;

            current = openList[0];
            if (current == null)
            {
                Debug.Log("Error current GraphTile is null");
                return false;
            }

            openList.Remove(current);

            if (current == endTile) // Final path found
            {
                Debug.Log("Path Found !!");
                Reconstruct_path(startTile, current);
                return true;
            }

            List<GraphTile> neighbors = current.neighbors;
            for (int i = 0; i < neighbors.Count; i++)
            {
                GraphTile neighbor = neighbors[i];
                if (closedList.Contains(neighbor)) // if the node has already been 
                {
                    continue;
                }

                // float costToNeighbor = current.G + GetDistance(current, neighbor);
                if (!openList.Contains(neighbor)) //costToNeighbor < neighbor.G || 
                {
                    // neighbor.G = costToNeighbor;
                    // neighbor.H = GetDistance(neighbor, end);
                    // neighbor.F = neighbor.G + neighbor.H;

                    neighbor.predecessor = current;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }

            }

            if (!closedList.Contains(current))
                closedList.Add(current);

        }
        return false;
    }

    /// <summary>
    /// Retrieve a list of node from start to end node to get the solution
    /// Also showing the answer in green
    /// </summary>
    /// <returns>Return true if a path exists or false if not</returns>
    public void Reconstruct_path(GraphTile startNode, GraphTile endNode)
    {
        List<GraphTile> solution = new List<GraphTile>();
        GraphTile node = endNode;

        // We will reconstruct the path from the end
        while (node != startNode && node.predecessor != null) //node.Predecessor != null
        {
            solution.Add(node);
            node = node.predecessor;
            node.ChangeColor();
        }
        solution.Add(startNode);
        solution.Reverse();
    }

    /// <summary>
    /// Get Neighbors for a node in a list of GraphTile
    /// </summary>
    /// <param name = graph > List of nodes (GraphTile).</param>
    /// <param name = node > Node given to search neighbors .</param>
    public void GetNeighbors(List<GraphTile> graph, GraphTile node)
    {
        if (graph != null)
        {
            List<GraphTile> neighbors = new List<GraphTile>();
            if (!node.Equals(null))
            {
                // We have 4 neighbors maximum : x-1,z | x+1,z | x,z-1 | x,z+1 
                for (int i = -1; i < 2; i += 2)
                {
                    bool tileExist = graph.Exists(item => (item.x == (node.x + i)) && (item.z == node.z));
                    if (tileExist)
                    {
                        GraphTile tile = graph.Find(item => (item.x == (node.x + i)) && (item.z == node.z));
                        neighbors.Add(tile);
                    }
                    tileExist = graph.Exists(item => (item.x == (node.x)) && (item.z == node.z + i));
                    if (tileExist)
                    {
                        GraphTile tile = graph.Find(item => (item.x == node.x) && (item.z == node.z + i));
                        neighbors.Add(tile);
                    }
                }
            }
            node.neighbors = neighbors;
        }
        else
            Debug.Log("Walkable Graph is null");
    }

    /// <summary>
    /// Get Neighbors for a (wall) node in a list of GraphTile
    /// Then remove existing neihbors in walkable graph.
    /// This function is used to remove unconsistant unwalkable tile in the walkable graph.
    /// </summary>
    /// <param name = graph > List of nodes (GraphTile).</param>
    /// <param name = node > Node given to search neighbors .</param>
    public void RemoveUnwalkableTile(List<GraphTile> graph, GraphTile wallNode)
    {
        if (graph != null)
        {
            if (!wallNode.Equals(null))
            {

                if (graph.Exists(item => (item.x == wallNode.x) && (item.z == wallNode.z))) // if the wall exists in the other map, we don't need to remove something
                {
                    return;
                }
                // get walkable node 
                GraphTile walkableNode = walkableGraph.Find(item => (item.x == wallNode.x) && (item.z == wallNode.z));
                // We have 4 neighbors maximum : x-1,z | x+1,z | x,z-1 | x,z+1 
                for (int i = -1; i < 2; i += 2)
                {
                    if (graph.Exists(item => (item.x == (wallNode.x + i)) && (item.z == wallNode.z))) // if an adjacent neighbor exists in the given graph
                    {
                        // we remove the unwalkable neighbors tile
                        GraphTile unwalkableNeighbor = walkableGraph.Find(item => (item.x == (wallNode.x + i)) && (item.z == wallNode.z));
                        walkableNode.neighbors.Remove(unwalkableNeighbor);
                    }
                    if (graph.Exists(item => (item.x == (wallNode.x)) && (item.z == wallNode.z + i))) // if an adjacent neighbor exists in the given graph
                    {
                        GraphTile unwalkableNeighbor = walkableGraph.Find(item => (item.x == wallNode.x) && (item.z == wallNode.z + i));
                        walkableNode.neighbors.Remove(unwalkableNeighbor);
                    }

                }
            }
        }
        else
            Debug.Log("Walkable Graph is null");
    }
}