using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fpsgravitywell_Pull : MonoBehaviour {

	Rigidbody rb;
	public Vector3 initialForce;
	float initialForceAmount;
	public fpsgravitywell_Pull objectToAttractRef;
	public bool noAttract = false;


	void Start(){
		rb = gameObject.GetComponent<Rigidbody> ();
		rb.AddForce (initialForce);

	}

	void FixedUpdate()
	{
		//attractor[] attractors = FindObjectsOfType<attractor> ();


		//foreach (attractor a in attractors) {
		//	if (a != this)

		if (!noAttract)
			Attract (objectToAttractRef);
		//}
	}


	void Attract (fpsgravitywell_Pull objectToAttract)
	{
		Rigidbody rbToAttract = objectToAttract.rb;

		Vector3 direction = rb.position - rbToAttract.position;

		float dist
		= direction.magnitude;

		float forceMag = (rb.mass * rbToAttract.mass) / Mathf.Pow (dist, 2);

		Vector3 force = direction.normalized * forceMag;

		rbToAttract.AddForce (force);


	}
}
