using UnityEngine;
using System.Collections;

public class BillBoard : MonoBehaviour {
	void OnEnable() {
		CameraPreRender.onPreCull += MyPreCull;
	}

	void OnDisable() {
		CameraPreRender.onPreCull -= MyPreCull;
	}

	void MyPreCull() {
		//we want to look back
		Vector3 difference = Camera.current.transform.position - transform.position;
		transform.LookAt(transform.position - difference, Camera.current.transform.up);
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x+90, transform.eulerAngles.y, transform.eulerAngles.z+180);
	}
}