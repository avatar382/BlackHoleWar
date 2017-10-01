using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour {

	//This is so that mss
	//	public GameObject G;
	private float mass;
	// Use this for initialization
	void Start () {
		mass = transform.gameObject.GetComponent<Rigidbody>().mass;
	}

	void ApplyGravity(GameObject g){
		Rigidbody rb = g.GetComponent<Rigidbody> ();
		Vector3 dir = rb.position - transform.position;
		float dist = dir.magnitude;
		float F_g = rb.mass * mass / Mathf.Pow (dist, 2f);
		rb.AddForce (-dir * F_g, ForceMode.Impulse);
	}

	// Update is called once per frame
	void Update () {
		//TODO: ADD tag Attractable to all objects effected by Attractors
		//Note, Attractable objects should add themselves to some static array upon creation
		//  This requires removal upn deletion
		//  This is because storing all of the game objects as an array is inefficient
		//  This is a (fixable) hack...
		//  Debris already exists, how about player...?
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Attractable");
		foreach (GameObject g in gos) {
			if (g != this.gameObject)
				ApplyGravity (g);
		}
	}
}
