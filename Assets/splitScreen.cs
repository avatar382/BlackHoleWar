using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splitScreen : MonoBehaviour {

	public Camera p1;
	public Camera p2;

	public bool Horizontal = false;

	// Use this for initialization
	/*void Start () {
		
	}*/
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space))
			ChangeSplitScreen ();
	}

	public void ChangeSplitScreen(){
		Horizontal = !Horizontal;

		if (Horizontal) {
			p1.rect = new Rect (0, 0, 1, 0.5f);
			p2.rect = new Rect (0, 0.5f, 1, 0.5f);
		} else {
			p1.rect = new Rect (0, 0, 0.5f, 1);
			p2.rect = new Rect (0.5f, 0, 0.5f, 1);
		}
	}

}
