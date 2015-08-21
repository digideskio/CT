using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]

public class PathNode : MonoBehaviour {
	
	public static bool pathValidated;
	public static bool distanceCalculated;
	
	static PathNode _first; // Bleh naming conventions
	
	public List<PathNode> nextPaths;
	public List<PathNode> prevPaths;
	public float distanceToEnd = 0;
	public bool isStart = false; //cars should start in front of the isStart PathNode in order for the placing to work properly
	//	public bool halfWayPoint = false;
	
	void Awake() {
		if (nextPaths == null)
			nextPaths = new List<PathNode>();
		if (prevPaths == null)
			prevPaths = new List<PathNode>();
		
		if (isStart) {
			if(first == null)
				first = this;
			else
				isStart = false;
		}
	}
	
	public static PathNode first {
		get {
			if(_first == null) {
				PathNode[] nodes = FindObjectsOfType<PathNode> ();
				foreach(PathNode node in nodes) {
					if(node.isStart)
						_first = node;
				}
			}
			if(_first == null) {
				Debug.LogWarning("Couldn't find first node");
			}
			return _first;
		}
		set {
			_first = value;
		}
	}
	
	public bool IsStart() {
		return (this == PathNode.first);
	}
	
	#if UNITY_EDITOR
	Color[] colors = {Color.cyan, Color.green, Color.red, Color.yellow, Color.blue};
	
	void OnDrawGizmosSelected() {
		PathNode[] nodes = FindObjectsOfType<PathNode> ();
		
		for(int i = 0; i < nodes.Length; i++) {
			Gizmos.DrawIcon(nodes[i].transform.position, "Waypoint.png", true);
			for(int j = 0; j < nodes[i].nextPaths.Count; j++) {
				if(nodes[i].nextPaths[j] != null) {
					Gizmos.color = colors[0];
					Gizmos.DrawLine (nodes[i].transform.position + nodes[i].transform.up * .3f, nodes[i].nextPaths[j].transform.position);
				} else {
					Debug.LogWarning("Removing null connections", nodes[i]);
					nodes[i].nextPaths.RemoveAt(j);
				}
			}
			for(int j = 0; j < nodes[i].prevPaths.Count; j++) {
				if(nodes[i].prevPaths[j] != null) {
					Gizmos.color = colors[1];
					Gizmos.DrawLine (nodes[i].transform.position + nodes[i].transform.up * .3f, nodes[i].prevPaths[j].transform.position);
				} else {
					Debug.LogWarning("Removing null connections", nodes[i]);
					nodes[i].prevPaths.RemoveAt(j);
				}
			}
			Gizmos.color = colors[2];
//			DrawSpawnPositions.DrawArrow(nodes[i].transform.position, nodes[i].transform.forward, nodes[i].transform.right, 18f);
		}
	}
	#endif	
}