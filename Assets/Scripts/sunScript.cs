using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class sunScript : MonoBehaviour {

	public int sunPhase = 0;
	public int phaseIncrement;
	public int blackHoleLevel;
	public Light lightRef;
	int phase = 0;

	public Material sunMaterialRef;

	Renderer rendererRef;
	public Color[] colorPhases;
	public float[] sizePhases;
	public float[] lightBrightnessPhases;

	public bool debugMode = false;
	public GameObject blackHoleRef;
	//public GameObject blackHoleLight;



	// Use this for initialization
	void Start () {



		rendererRef = gameObject.GetComponent<Renderer> ();
		rendererRef.material.color = colorPhases [0];




	}
	
	// Update is called once per frame
	void Update () {

		rendererRef.material.SetColor ("_EmissionColor", rendererRef.material.color);
		lightRef.color = rendererRef.material.color;
		if (Input.GetKeyUp (KeyCode.Space)) {
			if (debugMode) {
				nextPhase ();
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
		if (phase <= blackHoleLevel) {
			Debug.Log ("material should change");
			//gameObject.GetComponent<Renderer> ().material.color = Color.blue;
			rendererRef.material.DOColor(colorPhases[phase], 2);
			float nextScale = sizePhases [phase];
			transform.DOScale (new Vector3 (nextScale, nextScale, nextScale), 2);

		}
		else
			if (blackHoleRef)
			blackHoleStart ();
		

	}

	void blackHoleStart()
	{
		blackHoleRef.SetActive (true);
		transform.DOScale (new Vector3 (0, 0, 0), 1.5f);
		rendererRef.material.DOColor (Color.black, 1.5f);
	}

	void OnTriggerEnger(Collider col)
	{
		if (col.tag == "debris") {

			levelUp ();
			Destroy (col.gameObject);

		}
	}

}
