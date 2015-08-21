using UnityEngine;
using System.Collections;

public class CountDownGUI : MonoBehaviour {

	public Animator three,two,one,go;
	public AudioSource low, high;
	Animation introCameraMove;
	float animationTime;
	float timeBetweenNumbers = 1.3f;
	// Use this for initialization
	void Start () {
		introCameraMove = GameObject.FindGameObjectWithTag ("CarCamera").GetComponent<Animation>();
		animationTime = introCameraMove.GetComponent<Animation>().clip.length;
		StartCoroutine ("CountDown");

	}
	
	IEnumerator CountDown() {
		yield return new WaitForSeconds (animationTime - (3 * timeBetweenNumbers));
		three.SetTrigger ("go");
		if (low != null)
			low.Play ();
		yield return new WaitForSeconds (timeBetweenNumbers);
		two.SetTrigger ("go");
		if (low != null)
			low.Play ();
		yield return new WaitForSeconds (timeBetweenNumbers);
		one.SetTrigger ("go");
		if (low != null)
			low.Play ();
		yield return new WaitForSeconds (timeBetweenNumbers);
		if (high != null)
			high.Play ();
		go.SetTrigger ("go");
	}
}
