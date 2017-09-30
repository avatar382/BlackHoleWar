using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorCone : MonoBehaviour {

	public GameObject player;
	public ParticleSystem ps;
	public GameObject targets;
	public float maxAngle;

	// Use this for initialization
	void Start () {
	}

	void iterate(Vector3 p, Vector3 dir){
		float dirMag = Vector3.Magnitude (dir);
		foreach (Transform t in targets.transform){
			Vector3 tp = t.position;
			Vector3 toTarget = tp - p;
			float theta = 180 * Mathf.Acos( Vector3.Dot (dir, toTarget) / 
				(Vector3.Magnitude(toTarget) * dirMag ) ) / Mathf.PI;
			if (theta < maxAngle) {
				t.gameObject.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		var s = ps.shape;
		s.angle = maxAngle;
		Vector3 p = player.transform.position;
		Vector3 dir = player.transform.TransformDirection(Vector3.forward);
		Debug.DrawRay (p, dir*4);
		iterate (p, dir);
	}
}
