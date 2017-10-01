using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour {

	public GameObject debris;
	public bool isSelected = false;
	public bool wasSelected = false;
	public bool wasHit = false;
	private float speed;
	private int life = 500;

	// Use this for initialization
	void Start () {
	    speed = Random.Range(0.1f, 0.5f);
		float r = Random.Range(1,5);
		debris.GetComponent<Rigidbody> ().mass *= r * r * r;
		debris.transform.localScale *= r;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isSelected  && !wasHit) {
			wasSelected = true;
			debris.transform.position += new Vector3 (0f, 0f, speed*0.2f);
			if (life <= 0) {
				Destroy (debris);
			}
			life--;
		}
	}
}
