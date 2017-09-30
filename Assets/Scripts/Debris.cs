using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour {

  public GameObject debris;
  private float speed;
  private int life = 100;

	// Use this for initialization
	void Start () {
    speed = Random.Range(0.1f, 0.5f);

	}
	
	// Update is called once per frame
	void Update () {
    debris.transform.position += new Vector3(0f, 0f, speed);
    if(life <= 0 ) {
      Destroy(debris);
    }
    life--;

	}
}
