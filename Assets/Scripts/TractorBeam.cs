using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityStandardAssets.CrossPlatformInput;

public class TractorBeam : MonoBehaviour {

	public int PlayerNumber;
	public GameObject player;
	public Camera cameraRef;
	public ParticleSystem ps;
	public float maxAngle;
	public GameObject selected;
	public bool isSelected = false;
	public bool tractorMode = false;

	public Transform tractorPoint;


	GameObject selectedObject;

	private string input_R1;
	private string input_L1;


	// Use this for initialization
	void Start () {

		if (PlayerNumber == 1) {
			input_R1 = "P1-R1";
			input_L1 = "P1-L1";
		} else if (PlayerNumber == 2) {
			input_R1 = "P2-R1";
			input_L1 = "P2-L1";
		}

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

		if (tractorMode)
			TractorMode ();
		else
			targetMode ();



	}

	void targetMode()
	{

		var s = ps.shape;
		s.angle = maxAngle;
		Vector3 p = player.transform.position;
		Vector3 dir = player.transform.forward;
		//		Debug.DrawRay (p, dir*40);
		if (!isSelected) {
			//			iterate (p, dir);
		}

		RaycastHit hit;
		Ray r = cameraRef.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0));
		selectedObject = null;



		//if ( Physics.Raycast(p, dir, out hit, 10000.0f) ){
		//if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out hit, Mathf.Infinity))
		//{
		if (Physics.Raycast(r, out hit))
		{
			Debug.Log ("Something was hit! ");
			print (hit.transform.gameObject.tag);
			if (hit.transform.gameObject.tag == "Debris") {

				selectedObject = hit.transform.gameObject;
				//GameObject go = hit.transform.gameObject;
				//go.GetComponent<Debris>().wasHit = true;
				//Rigidbody rb = go.GetComponent<Rigidbody> ();
				//rb.AddForce (dir * 1f, ForceMode.Impulse);
			}
		}

		//if (Input.GetMouseButtonDown (0)) {
		if (CrossPlatformInputManager.GetButtonDown(input_R1)) {
			if (selectedObject != null) {
				selectedObject.transform.parent = player.transform;
				selectedObject.GetComponent<Debris> ().wasHit = true;
				selectedObject.GetComponent<Rigidbody> ().isKinematic = true;
				tractorMode = true;
				selectedObject.transform.DOMove (tractorPoint.position, 0.2f);
			}
		}

	}

	void TractorMode()
	{

		if (CrossPlatformInputManager.GetButtonDown(input_L1)) {//FIRE OBJECT
			selectedObject.GetComponent<Rigidbody> ().isKinematic = false;
			selectedObject.transform.parent = null;
			selectedObject.GetComponent<Rigidbody> ().AddForce (player.transform.forward * 1500000);
			selectedObject = null;
			tractorMode = false;


		}
		if (CrossPlatformInputManager.GetButtonDown(input_R1)) {//RELEASE OBJECT
			selectedObject.GetComponent<Rigidbody> ().isKinematic = false;
			selectedObject.transform.parent = null;
			selectedObject = null;
			tractorMode = false;


		}

	}

}
