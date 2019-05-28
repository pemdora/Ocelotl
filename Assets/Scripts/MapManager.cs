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
    public Transform map1Transform; // TileList
    private List<Transform> tilesMap1;
    private bool map1active;
    public int[,] map1;
    #endregion

    #region map2
    public Transform map2Transform; // TileList
    private List<Transform> tilesMap2;
    private bool map2active;
    public int[,] map2;
    #endregion

    [SerializeField]
    private LevelData leveldata;

    #region Maps in Array
    #endregion

    private bool mapSwap;
    public static int sublvl = 0;

    [Header("Goal")]
    public Transform goal; // Goal gameObject with particul system

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
    private List<GraphTile> weightedGraph; // Graph that contains map 1 and map 2 datas
    public enum GroundColor { Green, StayGreen, Red, Yellow}

    //SINGLETON
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

    private void Start()
    {
        #region Instanciate maps
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

        if (GameMaster.lvl == 99) // Random generation
        {
            Debug.Log(CreateRandomGraph());
        }
        else
        {
            // Init map
            foreach (MapDuoData maps in leveldata.subLevelList)
            {
                maps.InitMap();
            }
            map1 = leveldata.subLevelList[sublvl].map1;
            map2 = leveldata.subLevelList[sublvl].map2;
        }


        // Building walls and graphs // Need to separate graphs construction and wall construction
        for (int i = 0; i < leveldata.subLevelList[sublvl].arraylength; i++) // x values
        {
            for (int j = 0; j < leveldata.subLevelList[sublvl].arraylength; j++) // z values
            {
                bool tiledAdded = false;
                // Map 1
                if (map1[i, j] == 1) // If we got a 1 => wall position in the map array
                {
                    // Instanciate wall objects for 1st map
                    InstanciateWall(i, 0, j, tilesMap1, map1Transform);
                    GraphTile tile = new GraphTile(i, j);
                    obstacleGraph1.Add(tile);
                }
                else // its a walkable tile
                {
                    tiledAdded = true;
                    CheckAndAddGraphTile(i, j);
                }

                // Map 2
                if (map2[i, j] == 1)
                {
                    // Instanciate wall objects for 2d map
                    InstanciateWall(i, -5.5f, j, tilesMap2, map2Transform);
                    GraphTile tile = new GraphTile(i, j);
                    obstacleGraph2.Add(tile);
                }
                else
                {
                    if(!tiledAdded)
                        CheckAndAddGraphTile(i, j);
                }
            }
        }
        
        if (GameMaster.lvl != 99)
        {
            #region Fetching start and goal points and send it to graph
            Vector3 start = leveldata.subLevelList[sublvl].startPoint;

            if (walkableGraph.Exists(item => (item.x == start.x) && (item.z == start.z))) // if startTile already exist in walkableGraph
            {
                startTile = walkableGraph.Find(item => (item.x == start.x) && (item.z == start.z));
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
            Vector3 end = leveldata.subLevelList[sublvl].goalPoint;
            goal.position = end;

            endTile = walkableGraph.Find(item => (item.x == end.x) && (item.z == end.z));
            #endregion

            // Get Neighbors for walkable tiles
            foreach (GraphTile tempTile in walkableGraph)
            {
                GetNeighbors(walkableGraph, tempTile);
            }

            // Remove unwalkable neighbors tile in walkable graph
            foreach (GraphTile tempTile in obstacleGraph1)
            {
                RemoveUnwalkableTile(obstacleGraph2, tempTile);
            }
            foreach (GraphTile tempTile in obstacleGraph2)
            {
                RemoveUnwalkableTile(obstacleGraph1, tempTile);
            }
        }

        map1active = true;
        map2active = false;
        map1Transform.gameObject.SetActive(map1active);
        map2Transform.gameObject.SetActive(map2active);
        tilesMap = tilesMap1;
        mapSwap = true;
    }

    // USED FOR TESTS
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ShortestPath();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ShowGraphTiles(obstacleGraph1, GroundColor.Red);
        }

        //ShowGraphTiles(obstacleGraph1, GroundColor.Red);
        //ShowGraphTiles(obstacleGraph2, GroundColor.Yellow);

    }

    /// <summary>
    /// Function used to create a wall object in the world
    /// and attach this wall to a map
    /// </summary>
    /// <param name = x > float position in X axis.</param>
    /// <param name = y > float position in Y axis.</param>
    /// <param name = z > float position in Z axis.</param>
    /// <param name = map > Map parent transform </param>
    /// <param name = mapList > List of transforms for a map </param>
    public void InstanciateWall(float x, float y, float z, List<Transform> mapList, Transform map)
    {
        Vector3 position = new Vector3(x, y, z);
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
            MainCharacterController.characterController.lockControls = true;
            IEnumerator coroutine1 = WallsMapFall();
            StartCoroutine(coroutine1);
        }
    }

    /// <summary>
    /// Function that make walls' map falls
    /// </summary>
    private IEnumerator WallsMapFall()
    {
        mapSwap = false;
        map1active = !map1active;
        map2active = !map2active;
        GameMaster.gameMasterinstance.ChangeMapIcon();
        List<Transform> floorMapToActivate = new List<Transform>();
        foreach (Transform wallTile in tilesMap)
        {
            Animator animator = wallTile.GetComponent<Animator>();
            animator.SetTrigger("FadeOut");
            Transform floorTile = floormap.Find(tileFinding => (tileFinding.position.x == wallTile.position.x && tileFinding.position.z == wallTile.position.z));
            floorTile.gameObject.SetActive(false);
            floorMapToActivate.Add(floorTile);
        }
        yield return new WaitForSeconds(0.8f);
        #region WaitAndDo // this will be executed only when the coroutine have finished
        IEnumerator coroutine2 = ReactiveFloorMap(floorMapToActivate);
        StartCoroutine(coroutine2);
        #endregion
    }


    /// <summary>
    /// Function that make walls' map falls
    /// </summary>
    private IEnumerator ReactiveFloorMap(List<Transform> floorMapToActivate)
    {
        foreach (Transform tile in floorMapToActivate)
        {
            tile.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(0.01f);
        #region WaitAndDo // this will be executed only when the coroutine have finished
        IEnumerator coroutine3 = SwapMaps();
        StartCoroutine(coroutine3);
        #endregion
    }

    /// <summary>
    /// Get Active Map number
    /// </summary>
    /// <returns>Return number of active map </returns>
    public int GetActiveMap()
    {
        if (map1active)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    /// <summary>
    /// Function that swaps maps and play wall animation
    /// </summary>
    private IEnumerator SwapMaps()
    {
        if (map1active)
        {
            tilesMap = tilesMap1;
        }
        else
        {
            tilesMap = tilesMap2;
        }
        foreach (Transform tile in tilesMap)
        {
            tile.position += new Vector3(0, 9, 0);
        }
        map1Transform.gameObject.SetActive(map1active);
        map2Transform.gameObject.SetActive(map2active);
        yield return new WaitForSeconds(1f);
        #region WaitAndDo // this will be executed only when the coroutine have finished
        mapSwap = true;
        MainCharacterController.characterController.lockControls = false;
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

        openList.Add((GraphTile)startTile);
        while (openList.Count != 0) // while the list is not empty
        {
            GraphTile current = null;

            current = openList[0];
            if (current == null)
            {
                Debug.LogError("Error current GraphTile is null");
                return false;
            }

            openList.Remove(current);

            if (current == endTile) // Final path found
            {
                Reconstruct_path(startTile, current, GroundColor.Green);
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
    /// Chack if a shortest path exists in the graph of walkable tile
    /// Applying A star without time calculation (breadth First search)
    /// </summary>
    /// <returns>Return true if a path exists or false if not</returns>
    public bool ShortestPathOnWeightedGraph()
    {
        // Since GraphTile is a structure it can't be directly null, so we transformed it to GraphTile? or Nullable<GraphTile>
        List<GraphTile> openList = new List<GraphTile>(); // set of nodes to be evaluated
        List<GraphTile> closedList = new List<GraphTile>(); // set of nodes already evaluated

        openList.Add((GraphTile)startTile);
        while (openList.Count != 0) // while the list is not empty
        {
            GraphTile current = null;

            current = openList[0];
            if (current == null)
            {
                Debug.LogError("Error current GraphTile is null");
                return false;
            }

            openList.Remove(current);

            if (current == endTile) // Final path found
            {
                Debug.Log(" END " + endTile.x + " " + endTile.z);
                Reconstruct_path(startTile, current,GroundColor.Green);
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

                    if(current.value==1&& neighbor.value==2 || current.value == 2 && neighbor.value == 1)
                    {
                        break;
                    }
                    else
                    {
                        neighbor.predecessor = current;

                        if (!openList.Contains(neighbor))
                        {
                            openList.Add(neighbor);
                        }
                    }
                }

            }

            if (!closedList.Contains(current))
                closedList.Add(current);

        }
        return false;
    }

    public void ShowGraphTiles(List<GraphTile> graph, GroundColor color )
    {
        foreach(GraphTile tile in graph)
        {
            tile.ChangeColor(floormap, color);
        }
    }


    /// <summary>
    /// Retrieve a list of node from start to end node to get the solution
    /// Also showing the answer in green
    /// </summary>
    /// <returns>Return true if a path exists or false if not</returns>
    public void Reconstruct_path(GraphTile startNode, GraphTile endNode, GroundColor color)
    {
        List<GraphTile> solution = new List<GraphTile>();
        GraphTile node = endNode;

        // We will reconstruct the path from the end
        while (node != startNode && node.predecessor != null) //node.Predecessor != null
        {
            solution.Add(node);
            node = node.predecessor;
            node.ChangeColor(floormap,color);
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

    /// <summary>
    /// Get Neighbors for a node in a list of GraphTile
    /// </summary>
    /// <param name = graph > List of nodes (GraphTile).</param>
    /// <param name = node > Node given to search neighbors .</param>
    private bool CreateRandomGraph()
    {
        map1 = new int[8, 8];
        map2 = new int[8, 8];

        System.Random r = new System.Random(DateTime.Now.Millisecond);
        System.Random r2 = new System.Random(DateTime.Now.Millisecond + 42);

        int ite = 0;
        int lenght = 5;// map1.GetLength(0);
        while (ite < 200)
        {
            weightedGraph = new List<GraphTile>();

            for (int i = 0; i < lenght; i++)
            {
                for (int j = 0; j < lenght; j++)
                {
                    int rInt = r.Next(0, 2);  //Return a random float number between min [inclusive] and max [exclusive]
                                              //int value = (new UnityEngine.Random.Range(0, 1) > 0.5f) ? 0 : 1;
                    map1[i, j] = rInt;

                    rInt = r2.Next(0, 2);  //Return a random float number between min [inclusive] and max [exclusive]
                    map2[i, j] = rInt;
                }
            }

            map1[0, 0] = 0;
            map2[0, 0] = 0;
            map2[lenght, lenght] = 0;

            // Building Graph
            for (int i = 0; i < lenght; i++)
            {
                for (int j = 0; j < lenght; j++)
                {
                    if (map1[i, j] == 1 && map2[i, j] == 1) // cannot go through tile at any case
                    {
                        break;
                        //GraphTile tile = new GraphTile(i, j, 1);
                        //weightedGraph.Add(tile);
                    }
                    else if (map1[i, j] == 0 && map2[i, j] == 0) // always walkable without switching map
                    {
                        GraphTile tile = new GraphTile(i, j, 0);
                        weightedGraph.Add(tile);
                    }
                    else if (map1[i, j] == 0 && map2[i, j] == 1)
                    {
                        GraphTile tile = new GraphTile(i, j, 1);
                        weightedGraph.Add(tile);
                    }
                    else if (map1[i, j] == 1 && map2[i, j] == 0)
                    {
                        GraphTile tile = new GraphTile(i, j, 2);
                        weightedGraph.Add(tile);
                    }
                    else
                        Debug.LogWarning("Error case not understood");
                }
            }

            #region Setting start and goal points and send it to graph
            //Start Point
            Vector3 start = new Vector3(0, 0, 0);

            if (weightedGraph.Exists(item => (item.x == start.x) && (item.z == start.z))) // if startTile already exist in weightedGraph
            {
                startTile = weightedGraph.Find(item => (item.x == start.x) && (item.z == start.z));
            }
            else
            {
                Debug.Log("Start tile wasn't in graph");
                GraphTile tile = new GraphTile(0, 0)
                {
                    TileGameObject = floormap.Find(obj => obj.position.x == 0 && obj.position.z == 0)
                };
                startTile = tile;
                weightedGraph.Add(tile);
            }

            //GoalPoints
            Vector3 end = new Vector3(lenght-1, 0, lenght-1);
            endTile = weightedGraph.Find(item => (item.x == end.x) && (item.z == end.z));
            #endregion

            // Get Neighbors for walkable tiles
            foreach (GraphTile tempTile in weightedGraph)
            {
                GetNeighbors(weightedGraph, tempTile);
            }

            if (ShortestPathOnWeightedGraph() == true)
            {
                foreach (GraphTile tempTile in weightedGraph)
                {
                    Debug.Log(tempTile.value);
                }

                return true;
            }
            else
            {
                Debug.Log("+");
                ite++;
            }
        }
        return false;
    }
}