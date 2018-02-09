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


    //Data structure to contain information for drawing lines between nodes.
    class Pair
    {
        public Node a;
        public Node b;
        public LineRenderer line;
    }

    //For keeping track of what nodes are connected to what.
    private List<Pair> NodePairs = new List<Pair>();

    //The types of node that can exist on the grid, except Null, its just empty.
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
        if (gridActive) //Don't handle input if it ain't.
        {
            //If the user presses space and is on a node, capture it.
            if (Input.GetKeyDown(KeyCode.Space) && nodeOnGrid[selectedX, selectedY].type == NodeTypes.Uncontrolled)
            {
                List<Node> _nodesConnected = GetConnectedNodes(nodeOnGrid[selectedX, selectedY], NodeTypes.Controlled);
                Debug.Log(_nodesConnected.Count);
                if (_nodesConnected.Count > 0) //I.e. if a controlled node is connected to the selected uncontrolled node.
                {
                    Debug.Log("Capturing!");
                    ChangeNodeType(ref nodeOnGrid[selectedX, selectedY], NodeTypes.Controlled);
                }
            }
            //First condition is which key got pressed, second is whether its inside the array.
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

            //Update the cursor position.
            cursorInstance.transform.position = nodeOnGrid[selectedX, selectedY].location;
        }

        #endregion
    }


    #region NodeFunctions
    //This function connects node a and node b together.
    private void ConnectNodes(Node a, Node b)
    {
        //Keep track of connections.
        Pair _newPair = new Pair();

        _newPair.a = a;
        _newPair.b = b;

        //Create an instance of a line.
        Vector3 _lineSpawnPos = new Vector3(0f, 0.05f, 0f);
        _newPair.line = Instantiate(linePrefab, _lineSpawnPos, Quaternion.identity, transform).GetComponent<LineRenderer>();

        //Match the beginning and ends of the line to the position of the two nodes.
        Vector3[] _pos = new Vector3[2];
        _pos[0] = a.location; //Line beginning.
        _pos[1] = b.location; //Line end.

        _newPair.line.SetPositions(_pos);

        //And the new connection to a list, so they can be tracked (and removed if nessesary).
        NodePairs.Add(_newPair);
    }

    //Function determines whether node a is connected to node b.
    private bool IsConnected(Node a, Node b)
    {
        for (int i = 0; i < NodePairs.Count; i++)
        {
            if ((NodePairs[i].a.Equals(a) && NodePairs[i].b.Equals(b)) || (NodePairs[i].a.Equals(b) && NodePairs[i].b.Equals(a)))
            {
                return true;
            }
        }
        return false;
    }

    //Function returns a list of nodes connected to the node passed in.
    //It will only consider nodes of the type passed in, unless thats null, then all are considered.
    private List<Node> GetConnectedNodes(Node node, NodeTypes type)
    {

        List<Node> _connected = new List<Node>();

        for (int Y = 0; Y < 5; Y++)
        {
            for (int X = 0; X < 5; X++)
            {
                if (IsConnected(node, nodeOnGrid[X, Y]) && type == NodeTypes.Null)
                {
                    _connected.Add(nodeOnGrid[X, Y]);
                    Debug.Log("Connected Node (any)");
                }
                else if (IsConnected(node, nodeOnGrid[X, Y]) && nodeOnGrid[X, Y].type == type)
                {
                    _connected.Add(nodeOnGrid[X, Y]);
                    Debug.Log("Connected Node (" + type + ")");
                }
            }
        }

        return _connected;
    }


    //Create the gameObject for the node.
    private GameObject CreateNodeGameObj(NodeTypes _type, Vector3 _pos)
    {

        GameObject _newNode = null;

        //Match prefab to the type passed in.
        switch (_type)
        {
            case NodeTypes.Firewall:
                _newNode = Instantiate(firewallNode, _pos, Quaternion.identity, transform);
                break;
            case NodeTypes.Uncontrolled:
                _newNode = Instantiate(uncontrolledNode, _pos, Quaternion.identity, transform);
                break;
            case NodeTypes.Controlled:
                _newNode = Instantiate(controlledNode, _pos, Quaternion.identity, transform);
                break;
            case NodeTypes.AccessPoint:
                _newNode = Instantiate(accessPointNode, _pos, Quaternion.identity, transform);
                break;
            case NodeTypes.Null:
                _newNode = null;
                break;
        }

        return _newNode;
    }


    //Quick little function for neatly changing the type of a node, and updating its gameObj as well.
    private void ChangeNodeType(ref Node _a, NodeTypes _type)
    {
        GameObject _newNodeObj = CreateNodeGameObj(_type, _a.location);

        //Look through all pairs and update them as nessesary.
        for (int i = 0; i < NodePairs.Count; i++)
        {
            if (NodePairs[i].a.Equals(_a))
            {
                NodePairs[i].a.type = _type;
                NodePairs[i].a.gameObj = _newNodeObj;
            } 
            else if (NodePairs[i].b.Equals(_a)) 
            {
                NodePairs[i].b.type = _type;
                NodePairs[i].b.gameObj = _newNodeObj;
            }
        }

        _a.type = _type;
        Destroy(_a.gameObj);
        _a.gameObj = _newNodeObj;
    }
    #endregion


    //Gets called by whatever wants to start the hacking game. 
    public void CreateGrid()
    {
        gridActive = true;
        for (int Y = 0; Y < 5; Y++)
        {
            for (int X = 0; X < 5; X++)
            {
                //Create offsets so that the nodes are spawned relative to the script object.
                Vector3 _posToSpawn = new Vector3(-Y + 2, 0f, X - 2) * 0.15f; //the scalar is just making the offset smaller.
                Vector3 _offset = transform.position;
                Vector3 _offsetPosToSpawn = _offset + _posToSpawn;

                //Create the node based on what the array says.
                nodeOnGrid[X, Y].gameObj = CreateNodeGameObj(nodeOnGrid[X, Y].type, _offsetPosToSpawn);
                nodeOnGrid[X, Y].location = _offsetPosToSpawn;
            }
        }

        //create and instance of the cursor.
        cursorInstance = Instantiate(cursorPrefab, nodeOnGrid[selectedX, selectedY].location, Quaternion.identity) as GameObject;


        //Here is where nodes are connected together.
        ConnectNodes(nodeOnGrid[0, 4], nodeOnGrid[4, 4]);
        ConnectNodes(nodeOnGrid[0, 4], nodeOnGrid[2, 2]);
        ConnectNodes(nodeOnGrid[4, 0], nodeOnGrid[2, 2]);

    }



}
