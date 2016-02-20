using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	float bulletSpeed = 8f, bulletDamage = 1f, bulletMass = 10000f;
	[SerializeField] GameObject bulletExplosion;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (new Vector3(0,0,1) * bulletSpeed);
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit, 10f)) {
			if (hit.collider.gameObject.tag == "Player") {
				hit.collider.gameObject.GetComponent<PlayerCar> ().TakeDamage (bulletDamage);
				hit.collider.gameObject.GetComponent<Rigidbody> ().AddForce (transform.forward * bulletMass);

			}
			Instantiate (bulletExplosion, hit.point, Quaternion.identity);
			Destroy (gameObject);
		}

//		transform.LookAt(PlayerCar.s_instance.transform.position);
	}

}
