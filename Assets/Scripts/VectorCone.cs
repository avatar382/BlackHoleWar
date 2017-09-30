using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorCone : MonoBehaviour {

	public GameObject player;
	public ParticleSystem ps;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 p = player.transform.position;
		Vector3 dir = player.transform.TransformDirection(Vector3.forward);
		Debug.DrawRay (p, dir*4);

	}
}
