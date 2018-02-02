using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackPuzzle : MonoBehaviour
{

    [SerializeField]
    private GameObject firewallNode;
    [SerializeField]
    private GameObject controlledNode;
    [SerializeField]
    private GameObject uncontrolledNode;
    [SerializeField]
    private GameObject accessPointNode;
    [SerializeField]
    private GameObject cursorPrefab;
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

    enum NodeTypes { Null, Firewall, Controlled, Uncontrolled, AccessPoint };

    private Node[,] nodeOnGrid = new Node[5, 5];

    // Use this for initialization
    void Start()
    {
        nodeOnGrid[0, 0].type = NodeTypes.AccessPoint;
        nodeOnGrid[0, 4].type = NodeTypes.Uncontrolled;
        nodeOnGrid[4, 0].type = NodeTypes.Controlled;
        nodeOnGrid[4, 4].type = NodeTypes.Firewall;

        nodeOnGrid[3, 0].type = NodeTypes.Uncontrolled;
        nodeOnGrid[2, 0].type = NodeTypes.Uncontrolled;
        nodeOnGrid[1, 0].type = NodeTypes.Uncontrolled;

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


    }

    void CreateNode(GameObject _node, Vector3 _pos, int x, int y)
    {
        nodeOnGrid[x, y].gameObj = Instantiate(_node, _pos, Quaternion.LookRotation(-transform.right, transform.up), transform);
        nodeOnGrid[x, y].location = _pos;
    }

    // Update is called once per frame
    void Update()
    {
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
                if (nodeOnGrid[selectedX + 1, selectedY].type != NodeTypes.Null)
                {
                    selectedX++;
                }
            }
            else if (Input.GetKeyDown(KeyCode.A) && selectedX - 1 != -1)
            {
                if (nodeOnGrid[selectedX - 1, selectedY].type != NodeTypes.Null)
                {
                    selectedX--;
                }
            }
            else if (Input.GetKeyDown(KeyCode.W) && selectedY + 1 != 5)
            {
                if (nodeOnGrid[selectedX, selectedY + 1].type != NodeTypes.Null)
                {
                    selectedY++;
                }
            }
            else if (Input.GetKeyDown(KeyCode.S) && selectedY - 1 != -1)
            {
                if (nodeOnGrid[selectedX, selectedY - 1].type != NodeTypes.Null)
                {
                    selectedY--;
                }
            }

            cursorInstance.transform.position = nodeOnGrid[selectedX, selectedY].location;
        }
    }
}
