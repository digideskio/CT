using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticPool {
	
	static StaticPool s_instance;
	
	public Dictionary<GameObject, List<GameObject>> objLists;
	
	const int DEFAULT_SIZE = 10;
	const int SIZE_INCREMENT = 5;
	
	[System.NonSerialized]
	public GameObject parent;
	
	public StaticPool () {
		s_instance = this;
		objLists = new Dictionary<GameObject, List<GameObject>>();
		parent = new GameObject("StaticPool");
		GameObject.DontDestroyOnLoad(parent);
	}
	
	/// <summary>
	/// Not neccesary. Preload prefab for the pool.
	/// </summary>
	/// <param name="prefab">Prefab.</param>
	public static void InitObj(GameObject prefab) {
		if(s_instance.objLists.ContainsKey(prefab) == false) {
			GameObject holder = new GameObject(prefab.name);
			holder.transform.parent = s_instance.parent.transform;
			
			s_instance.objLists.Add (prefab, new List<GameObject>());
			AddToList(prefab, DEFAULT_SIZE, holder.transform);
		}
	}
	
	/// <summary>
	/// Gets the object of type prefab.
	/// </summary>
	/// <returns>The object.</returns>
	/// <param name="prefab">Prefab.</param>
	public static GameObject GetObj(GameObject prefab) {
		InitObj(prefab);
		
		// Find inactive
		for(int i = 0, n = s_instance.objLists[prefab].Count; i < n; i++) {
			if(s_instance.objLists[prefab][i].activeSelf == false) {
				s_instance.objLists[prefab][i].SetActive(true);
				return s_instance.objLists[prefab][i];
			}
		}
		
		// None found
		AddToList(prefab, SIZE_INCREMENT, s_instance.parent.transform.FindChild(prefab.name));
		
		//		Debug.Log(s_instance.objLists[prefab].Count - SIZE_INCREMENT);
		
		// Zero indexed and minus what we just added
		s_instance.objLists[prefab][s_instance.objLists[prefab].Count - SIZE_INCREMENT].SetActive(true);
		return s_instance.objLists[prefab][s_instance.objLists[prefab].Count - SIZE_INCREMENT];
		
	}
	
	static void AddToList(GameObject prefab, int count, Transform holder) {
		for(int i = 0; i < count; i++) {
			GameObject obj = (GameObject)GameObject.Instantiate(prefab, Vector3.one * 1000f, Quaternion.identity);
			s_instance.objLists[prefab].Add(obj);
			obj.transform.parent = holder;
			obj.SetActive(false);
		}
	}
}