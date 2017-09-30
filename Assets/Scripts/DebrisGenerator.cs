using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisGenerator : MonoBehaviour {
  public GameObject debris;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	 if(Random.value > 0.9) {
     GameObject newDebris = Instantiate(debris, transform.position, transform.rotation);

     Vector3 posXYZ;
     posXYZ = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, 0); 
     posXYZ.Normalize();
     newDebris.transform.position = transform.position + posXYZ * 2 * Random.value; // TODO: scale to size of disk
   }	
	}
}
