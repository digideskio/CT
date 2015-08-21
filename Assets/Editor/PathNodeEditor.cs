using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(PathNode))]
public class PathNodeEditor : Editor {
	
	public override void OnInspectorGUI() {
		GUI.changed = false;
		DrawDefaultInspector ();
		
		EditorGUILayout.Space();
		
		serializedObject.Update();
		
		if (GUI.changed) { //FIXME Doesn't really work
			PathNode.pathValidated = false;
			PathNode.distanceCalculated = false;
		}
		if(!PathNode.pathValidated)
			GUILayout.Label("Not Validated");
		else
			GUILayout.Label("Validated!");
		if (GUILayout.Button("Validate")) {
			PathNode.pathValidated = ValidatePath();
		}
		
		if(!PathNode.distanceCalculated)
			GUILayout.Label("Dist not Calculated");
		else
			GUILayout.Label("Dist calculated!");
		if (GUILayout.Button("Calulate Distance")) {
			PathNode.distanceCalculated = CalculateDistanceToEnd();
		}
		
		if (GUILayout.Button("Fix rotations")) {
			FixRotations();
		}
		
		EditorGUILayout.Space();
		
		if (GUILayout.Button("New node")) {
			CreateNewNode();
		}
		
		serializedObject.ApplyModifiedProperties();
	}
	
	bool ValidatePath() { //TODO count how many next and prevs and make sure they match up. Make sure first is working as intented
		PathNode cur = PathNode.first;
		if (cur != null) {
			do {
				if (cur.nextPaths.Count > 0) {
					foreach (PathNode next in cur.nextPaths) {
						if (!next.prevPaths.Contains(cur)) {
							Debug.Log("Path not linked properly", cur);
							return false;
						}
					}
				} else {
					Debug.LogWarning("Path not finished", cur);
					return false;
				}
				
				if (cur.prevPaths.Count > 0) {
					foreach (PathNode prev in cur.prevPaths) {
						if (!prev.nextPaths.Contains(cur)) {
							Debug.Log("Path not linked properly", cur);
							return false;
						}
					}
				} else {
					Debug.LogWarning("Path not finished", cur);
					return false;
				}
			} while(cur != PathNode.first);
			Debug.Log("Path validated!");
			return true;
		}
		return false;
	}
	
	bool CalculateDistanceToEnd() { //TODO everything. Calculate back to front and store along the way instead of this n^2 BS.
		if (PathNode.pathValidated) {
			PathNode[] pathNodes = FindObjectsOfType<PathNode>();
			foreach (PathNode pathNode in pathNodes) {
				PathNode iter = pathNode;
				float distance = 0f;
				do {
					distance += Vector3.Distance(iter.transform.position, iter.nextPaths [0].transform.position);
					iter = iter.nextPaths [0];
				} while (iter != PathNode.first);
				pathNode.distanceToEnd = distance;
			}
			return true;
		} else {
			Debug.LogWarning("Path not validated!");
			return false;
		}
	}
	
	void FixRotations() {
		if (PathNode.pathValidated) {
			PathNode[] pathNodes = FindObjectsOfType<PathNode>();
			foreach (PathNode pathNode in pathNodes) {
				pathNode.transform.LookAt(pathNode.nextPaths[0].transform.position);
			}
		} else {
			Debug.LogWarning("Path not validated!");
			return;
		}
	}
	
	GameObject CreateNewNode() {
		GameObject newNodeObj = (GameObject)Instantiate(Selection.activeGameObject);// new GameObject ("Node");
		//		newNode.AddComponent<PathNode> ();
		newNodeObj.transform.parent = PathNode.first.transform.parent;
		newNodeObj.transform.position = Selection.activeGameObject.transform.position + Selection.activeGameObject.transform.forward;
		newNodeObj.name = "Node" + FindObjectsOfType<PathNode>().Length;
		
		PathNode oldNode = Selection.activeGameObject.GetComponent<PathNode>();
		PathNode newNode = newNodeObj.GetComponent<PathNode>();
		
		newNode.nextPaths.Clear();
		newNode.prevPaths.Clear();
		
		newNode.prevPaths.Add(oldNode);
		oldNode.nextPaths.Add(newNode);
		
		//		new SerializedObject(oldNode).ApplyModifiedProperties();
		//		new SerializedObject(newNode).ApplyModifiedProperties();
		
		Selection.activeObject = newNodeObj;
		return newNodeObj;
	}
}