using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour {

	public GameObject debris;
	public bool isSelected = false;
	public bool wasSelected = false;
	public bool wasHit = false;
	private float speed;
	public int life = 500;
	public TractorBeam holder;
	public bool beingHeld;


	// Use this for initialization
	void Start () {
	    speed = Random.Range(0.1f, 0.5f);
		float r = Random.Range(1,5);
		float minimumColliderRadius = 0.03f;
		float scaledColliderRadius = debris.GetComponent<SphereCollider> ().radius * r * 0.2f;


		debris.GetComponent<Rigidbody> ().mass *= r * r * r;
		debris.transform.localScale *= r;
		debris.GetComponent<SphereCollider> ().isTrigger = false;

		// this collider is a trigger, for black hole embiggening 
		debris.AddComponent<BoxCollider>();
		debris.GetComponent<BoxCollider>().isTrigger = true;

		if (scaledColliderRadius > minimumColliderRadius) {
				debris.GetComponent<SphereCollider> ().radius = scaledColliderRadius;
		} 
		else {
  			debris.GetComponent<SphereCollider> ().radius = minimumColliderRadius;
		}
	}

	public void putInWell()
	{
		beingHeld = true;

	}
	
	// Update is called once per frame
	void Update () {
		if (!isSelected  && !wasHit) {
			wasSelected = true;
			debris.transform.position += new Vector3 (0f, 0f, speed*0.2f);
			if (life <= 0) {
				Destroy (this.gameObject);
			}
			life--;
		}
	}
}
