using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackPuzzle : MonoBehaviour
{

    [SerializeField] private GameObject firewallNode;
    [SerializeField] private GameObject controlledNode;
    [SerializeField] private GameObject uncontrolledNode;
    [SerializeField] private GameObject accessPointNode;
    [SerializeField] private GameObject cursorPrefab;
    [SerializeField] private GameObject linePrefab;

    private GameObject cursorInstance;

    private bool gridActive = false;

    private int selectedX = 4;
    private int selectedY = 0;



    //C# doesn't support records yet, so a struct can provide the same functionality.
    struct Node
    {
        public NodeTypes type;
        public Vector3 location;
        public GameObject gameObj;
    }

    struct Pair
    {
        public Node a;
        public Node b;
        public LineRenderer line;
    }

    //For keeping track of what nodes are connected to what.
    private List<Pair> NodePairs = new List<Pair>();



    enum NodeTypes { Null, Firewall, Controlled, Uncontrolled, AccessPoint };

    private Node[,] nodeOnGrid = new Node[5, 5];

    // Use this for initialization
    void Start()
    {
        //Layout grid.
        nodeOnGrid[0, 0].type = NodeTypes.Uncontrolled;
        nodeOnGrid[0, 4].type = NodeTypes.AccessPoint;
        nodeOnGrid[4, 0].type = NodeTypes.Controlled;
        nodeOnGrid[4, 4].type = NodeTypes.Firewall;

        nodeOnGrid[2, 2].type = NodeTypes.Uncontrolled;
    }

    void Update()
    {
        #region input
        //This is one ugly controller.
        //TODO find a better way to write this for christ sake.
        if (gridActive)
        {

            if (Input.GetKeyDown(KeyCode.Space) && nodeOnGrid[selectedX, selectedY].type == NodeTypes.Uncontrolled)
            {
                //Capture node
            }

            if (Input.GetKeyDown(KeyCode.D) && selectedX + 1 != 5)
            {
                selectedX++;
            }
            else if (Input.GetKeyDown(KeyCode.A) && selectedX - 1 != -1)
            {
                selectedX--;
            }
            else if (Input.GetKeyDown(KeyCode.W) && selectedY + 1 != 5)
            {
                selectedY++;

            }
            else if (Input.GetKeyDown(KeyCode.S) && selectedY - 1 != -1)
            {
                selectedY--;

            }

            cursorInstance.transform.position = nodeOnGrid[selectedX, selectedY].location;
        }

        #endregion
    }

    private void ConnectNodes(Node a, Node b)
    {
        Pair _newPair = new Pair();
        _newPair.a = a;
        _newPair.b = b;

        Vector3 _lineSpawnPos = new Vector3(0f, 0.05f, 0f);
        _newPair.line = Instantiate(linePrefab, _lineSpawnPos, Quaternion.identity, transform).GetComponent<LineRenderer>();

        Vector3[] _pos = new Vector3[2];
        _pos[0] = a.location;
        _pos[1] = b.location;


        _newPair.line.SetPositions(_pos);

        NodePairs.Add(_newPair);
    }

    public void CreateGrid()
    {
        gridActive = true;
        for (int Y = 0; Y < 5; Y++)
        {
            for (int X = 0; X < 5; X++)
            {
                Vector3 _posToSpawn = new Vector3(-Y + 2, 0f, X - 2) * 0.15f;
                Vector3 _offset = transform.position;
                Vector3 _offsetPosToSpawn = _offset + _posToSpawn;
                switch (nodeOnGrid[X, Y].type)
                {
                    case NodeTypes.Firewall:
                        CreateNode(firewallNode, _offsetPosToSpawn, X, Y);
                        break;
                    case NodeTypes.Uncontrolled:
                        CreateNode(uncontrolledNode, _offsetPosToSpawn, X, Y);
                        break;
                    case NodeTypes.Controlled:
                        CreateNode(controlledNode, _offsetPosToSpawn, X, Y);
                        break;
                    case NodeTypes.AccessPoint:
                        CreateNode(accessPointNode, _offsetPosToSpawn, X, Y);
                        break;
                    case NodeTypes.Null:
                        nodeOnGrid[X, Y].location = _offsetPosToSpawn;
                        break;
                }
            }
        }
        cursorInstance = Instantiate(cursorPrefab, nodeOnGrid[selectedX, selectedY].location, Quaternion.identity) as GameObject;

        ConnectNodes(nodeOnGrid[0, 4], nodeOnGrid[4, 4]);
        ConnectNodes(nodeOnGrid[0, 4], nodeOnGrid[2, 2]);
        ConnectNodes(nodeOnGrid[4, 0], nodeOnGrid[2, 2]);


    }

    private void CreateNode(GameObject _node, Vector3 _pos, int x, int y)
    {
        nodeOnGrid[x, y].gameObj = Instantiate(_node, _pos, Quaternion.identity, transform);
        nodeOnGrid[x, y].location = _pos;
    }

    // Update is called once per frame

}
