using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackPuzzle : MonoBehaviour {

	[SerializeField]
	private GameObject firewallNode;
	[SerializeField]
	private GameObject controlledNode;
	[SerializeField]
	private GameObject uncontrolledNode;
	[SerializeField]
	private GameObject accessPointNode;
	[SerializeField]
	private Transform nodeParent;


	enum NodeTypes { Blank, Firewall, Controlled, Uncontrolled, AccessPoint };

	private NodeTypes[,] grid = new NodeTypes[5,5];

	// Use this for initialization
	void Start () {
		grid[0, 2] = NodeTypes.Firewall;
		grid[1, 2] = NodeTypes.Uncontrolled;
		grid[1, 1] = NodeTypes.Uncontrolled;
		grid[1, 0] = NodeTypes.Controlled;
		grid[2, 0] = NodeTypes.Uncontrolled;
		grid[2, 1] = NodeTypes.Uncontrolled;
		grid[3, 0] = NodeTypes.AccessPoint;

		for (int row = 0; row < 5; row++) {
			for (int column = 0; column < 5; column++) {
				Vector3 _posToSpawn = new Vector3(0, column - 2, row - 2) * 2;
				Vector3 _offset = this.transform.position;
				Vector3 _offsetPosToSpawn = _offset + _posToSpawn;
				switch(grid[row,column]) {
					case NodeTypes.Firewall:
						CreateNode(firewallNode, _offsetPosToSpawn);
						break;
					case NodeTypes.Uncontrolled:
						CreateNode(uncontrolledNode, _offsetPosToSpawn);
						break;
					case NodeTypes.Controlled:
						CreateNode(controlledNode, _offsetPosToSpawn);
						break;
					case NodeTypes.AccessPoint:
						CreateNode(accessPointNode, _offsetPosToSpawn);
						break;
				}
			}
		}


	}

	void CreateNode (GameObject _node, Vector3 _pos) {
		Instantiate(_node, _pos, Quaternion.identity, nodeParent);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
