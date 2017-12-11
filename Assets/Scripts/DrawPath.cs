using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPath : MonoBehaviour {

	Transform[] waypointChildren;

	private void OnDrawGizmos() {
		waypointChildren = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			waypointChildren[i] = transform.GetChild(i);
		}

		for (int i = 0; i < transform.childCount; i++) {
			Gizmos.DrawSphere(waypointChildren[i].position, 0.1f);
			Gizmos.DrawLine(waypointChildren[i].position, waypointChildren[(i + 1) % transform.childCount].position);
		}

	}

}
