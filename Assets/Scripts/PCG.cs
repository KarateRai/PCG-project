using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class will generate the PCG map and take care of the overarching generation on content
/// </summary>
public class PCG : MonoBehaviour
{
    //Private
    private NodeGrid grid;
    private Vector3[,] dqHeightMap;
    private RoomConnecter connector;
    private Dictionary<Vector2, GameObject> createdObjects;
    private List<Node> checkedNodes;

    //Public
    public List<Room> roomsOnMap = new List<Room>();
    public GameObject NodePrefab;
    public NeighbourhoodType neighbourhoodType;

    [Header("Grid")]
    [SerializeField] int gridSize = 0;

    [Header("CA")]
    [SerializeField] int interations = 1;
    [SerializeField] int neighbourhood = 1;
    [SerializeField] int threshold = 4;

    [Header("Agent")]
    public GameObject agentPrefab;
    public AgentStopCondition stopCondition;
    public int itemsToPlace;
    public float timeToLive;

    //UI elements
    [Header("UI")]
    [SerializeField]
    Button cAButton, updateButton, connectorButton, agentButton;

    // Start is called before the first frame update
    void Start()
    {
        //createdObjects = new List<GameObject>();
        createdObjects = new Dictionary<Vector2, GameObject>();
        checkedNodes = new List<Node>();
        connector = new RoomConnecter();
        DisableButtons();
    }

    private void DisableButtons()
    {
        cAButton.enabled = false;
        updateButton.enabled = false;
        connectorButton.enabled = false;
        agentButton.enabled = false;
    }

    private void N_OnChangeNodeColor(int X, int  Y, Color obj)
    {
        createdObjects[new Vector2(X, Y)].GetComponent<Renderer>().material.color = obj;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TEST()
    {
        //TerrainData data = new TerrainData();
        //data.size = new Vector3(grid.nodeSize * grid.size, 0, grid.nodeSize * grid.size);
        
    }

    public void ConnectRooms()
    {
        if (roomsOnMap == null || grid == null)
        {
            Debug.Log("RoomsOnMap or grid was null");
        }
        else
            connector.InitializeConnector(grid, roomsOnMap);

        agentButton.enabled = true;
    }

    public void StartAgent()
    {
        //Add timer of objects or time
        //Change to middle node instead of bottom corner node
        agentPrefab = Instantiate(agentPrefab, createdObjects[FindStartingNode()].transform.position, Quaternion.identity);
        Agent agent = agentPrefab.GetComponent<Agent>();
        agent.grid = grid;
        agent.SetStopCondition(stopCondition, itemsToPlace, timeToLive);
    }
    private Vector2 FindStartingNode()
    {
        for (int i = 0; i < grid.size/2; i++)
        {
            for (int x = -i; x < i; x++)
            {
                for (int y = -i; y < i; y++)
                {
                    if (grid[grid.size / 2 + x, grid.size / 2 + y].Alive != true)
                        continue;
                    else
                    {
                        //Debug.Log()
                        return new Vector2(grid.size / 2 + x, grid.size / 2 + y);
                    }
                }
            }
        }
        //for (int x = 0; x < grid.size; x++)
        //{
        //    for (int y = 0; y < grid.size; y++)
        //    {
        //        if (grid[x,y].Alive != true)
        //        {
        //            continue;
        //        }
        //        else
        //        {
        //            return new Vector2(x, y);
        //        }
        //    }
        //}
        Debug.LogError("Every node is dead");
        return Vector2.zero;
    }

    //Methods for doing PGC work
    public void DoCA()
    {
        NodeCA.neighbourhoodType = this.neighbourhoodType;
        NodeCA.StartCA(grid, interations, neighbourhood, threshold);
        MapRooms();

        updateButton.enabled = true;
    }
    public void DoDiamondSquare()
    {
        dqHeightMap = DiamondSquare.DoDiamondSquare(gridSize);
    }

    //Quality check methods

    //Here we compare different grids so that we know they are not the same or to similar
    public void CompareGrids()
    {

    }

    //This method floods all usable areas
    public void MapRooms()
    {
        checkedNodes.Clear();
        int roomIndex = 1;
        for (int x = 0; x < grid.size; x++)
        {
            for (int y = 0; y < grid.size; y++)
            {
                if (checkedNodes.Contains(grid[x,y]))
                {
                    continue;
                }
                else
                {
                    if (grid[x,y].Alive)
                    {
                        Flood(grid[x, y], new Room(roomIndex));
                        roomsOnMap.Add(grid[x, y].room);
                        roomIndex++;
                    }
                    else
                    {
                        checkedNodes.Add(grid[x, y]);
                        grid[x, y].isChecked = true;
                    }
                }
            }
        }
    }
    private void Flood(Node node, Room room)
    {
        if (checkedNodes.Contains(node))
        {
            return;
        }
        if (room == null)
        {
            Debug.Log("ROOM IS NULL");
        }
        node.isChecked = true;
        //Add node to room and then add reference in node
        room.AddNode(node);
        node.room = room;
        
        checkedNodes.Add(node);

        //Check left
        if (grid[node.X - 1, node.Y] != null && grid[node.X - 1, node.Y].Alive == true)
        {
            Flood(grid[node.X - 1, node.Y], room);
        }
        //Check right
        if (grid[node.X + 1, node.Y] != null && grid[node.X + 1, node.Y].Alive == true)
        {
            Flood(grid[node.X + 1, node.Y], room);
        }
        //Check up
        if (grid[node.X, node.Y + 1] != null && grid[node.X, node.Y + 1].Alive == true)
        {
            Flood(grid[node.X, node.Y + 1], room);
        }
        //Check down
        if (grid[node.X, node.Y - 1] != null && grid[node.X, node.Y - 1].Alive == true)
        {
            Flood(grid[node.X, node.Y - 1], room);
        }
        return;
    }
   
    //Create grid presentation
    public void CreateGrid()
    {
        DisableButtons();

        grid = new NodeGrid(gridSize);
        DoDiamondSquare();

        if (createdObjects.Count != 0)
            ClearObjectList();

        GameObject go;
        Material m;
        //Temp version, final version should make grid with some kind of meshrenderer for better performance

        for (int x = 0; x < grid.size; x++)
        {
            for (int y = 0; y < grid.size; y++)
            {
                if (grid[x,y].Alive)
                {
                    go = Instantiate(NodePrefab, /*v[x,y]*/new Vector3(grid[x, y].X, 0, grid[x, y].Y), NodePrefab.transform.rotation);
                    m = go.GetComponent<Renderer>().material;
                    m.color = Color.green;
                    createdObjects.Add(new Vector2(x, y), go);
                }
                else
                {
                    go = Instantiate(NodePrefab, /*v[x, y]*/ new Vector3(grid[x, y].X, 0, grid[x, y].Y), NodePrefab.transform.rotation);
                    m = go.GetComponent<Renderer>().material;
                    m.color = Color.red;
                    createdObjects.Add(new Vector2(x,y), go);
                }
                
            }
        }

        foreach (Node n in grid.nodegrid)
        {
            n.OnChangeNodeColor += N_OnChangeNodeColor;
        }
        cAButton.enabled = true;
    }
    public void UpdateGrid()
    {
        Material m;

        for (int x = 0; x < grid.size; x++)
        {
            for (int y = 0; y < grid.size; y++)
            {
                m = createdObjects[new Vector2(x,y)].GetComponent<Renderer>().material;
                m.color = grid[x, y].Alive == true ? Color.green : Color.red;
            }
        }
        connectorButton.enabled = true;
    }
    public void ClearObjectList()
    {
        foreach (KeyValuePair<Vector2, GameObject> pair in createdObjects)
        {
            Destroy(pair.Value);
        }
        createdObjects.Clear();
    }
}
