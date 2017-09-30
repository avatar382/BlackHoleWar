using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class sunScript : MonoBehaviour {

	public int sunPhase = 0;
	public int phaseIncrement;
	public int blackHoleLevel;
	int phase = 0;

	public Material sunMaterialRef;

	Material materialRef;
	public Color[] colorPhases;
	public float[] sizePhases;
	public float[] lightBrightnessPhases;

	public bool debugMode = false;



	// Use this for initialization
	void Start () {



		materialRef = gameObject.GetComponent<Renderer> ().material; 
		materialRef.color = colorPhases [0];




	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.Space)) {
			if (debugMode) {
				levelUp ();
			}
		}
	}

	public void levelUp()
	{
		sunPhase++;
		if (sunPhase % phaseIncrement == 0) {
			Debug.Log ("Next phase");
			nextPhase ();
		}
	}

	public void nextPhase()
	{
		phase++;
		if (phase < blackHoleLevel) {
			Debug.Log ("material should change");
			materialRef.DOColor (colorPhases [phase], 2);
		}
		else
			blackHoleStart ();


	}

	void blackHoleStart()
	{
	}

	void OnTriggerEnger(Collider col)
	{
		if (col.tag == "debris") {

			levelUp ();
			Destroy (col.gameObject);

		}
	}

}
