using UnityEngine;
using System.Collections;

public class BarkManager : MonoBehaviour {

	[SerializeField]
	TextAsset
	tooCloseBarks,
	enterLOSBarks
	;
	public string[]
	m_tooCloseBarksArray,
	m_enterLOSBarksArray;
	public static BarkManager s_instance;
	
	void Awake() {
		if (s_instance == null) {
			s_instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		m_tooCloseBarksArray = tooCloseBarks.ToString ().Split (","[0]);
		m_enterLOSBarksArray = enterLOSBarks.ToString ().Split (","[0]);
	}
	public void Bark(string[] textOptions) {
		int rand = Random.Range (0, textOptions.Length-1);
		GUIManager.s_instance.SetSubtitleText ("\""+ textOptions [rand]+"\"");
	}
}
