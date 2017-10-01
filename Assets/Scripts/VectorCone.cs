using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorCone : MonoBehaviour {

	public GameObject player;
	public ParticleSystem ps;
	public float maxAngle;
	public GameObject selected;
	public bool isSelected = false;

	// Use this for initialization
	void Start () {
	}

	void iterate(Vector3 p, Vector3 dir){
		float dirMag = Vector3.Magnitude (dir);
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Debris");
		GameObject minAGO;
		float minAngle = maxAngle;
		foreach (GameObject g in gos){
			Vector3 tp = g.transform.position;
			Vector3 toTarget = tp - p;
			float theta = 180 * Mathf.Acos( Vector3.Dot (dir, toTarget) / 
				(Vector3.Magnitude(toTarget) * dirMag ) ) / Mathf.PI;
			if (theta < minAngle) {
				minAngle = theta;
				selected = g;
				isSelected = true;
//				g.gameObject.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
//				g.transform.position += toTarget * 0.01f;
			}
		}
		if (isSelected) {
			selected.GetComponent<Debris>().isSelected = true;
			print("selected an object foO!OiUFASOIFOWAIU YBOIUDWA GOIWAD");
			isSelected = false;
		}
	}

	// Update is called once per frame
	void Update () {
		var s = ps.shape;
		s.angle = maxAngle;
		Vector3 p = player.transform.position;
		Vector3 dir = player.transform.TransformDirection(Vector3.forward);
//		Debug.DrawRay (p, dir*40);
		if (!isSelected) {
//			iterate (p, dir);
		}
		RaycastHit hit;
		if ( Physics.Raycast(p, dir, out hit, 10.0f) ){
			print (hit.transform.gameObject.tag);
			if (hit.transform.gameObject.tag == "Debris") {
				GameObject go = hit.transform.gameObject;
				go.GetComponent<Debris>().wasHit = true;
				Rigidbody rb = go.GetComponent<Rigidbody> ();
				rb.AddForce (dir * 1f, ForceMode.Impulse);
			}
		}
	}
}
