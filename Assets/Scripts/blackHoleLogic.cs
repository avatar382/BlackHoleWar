using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio;

public class blackHoleLogic : MonoBehaviour {

	public GameManager gm;

	Vector3 initialSize;
	bool blackHoleAppeared = false;
	public float[] weights;

	public int blackHoleLevel = 0;
	public int blackHoleStepUp;

	public AudioMixer mixer;
	public AudioMixerSnapshot[] sn;
	public bool win = false;
	public AudioSource blackHoleMusic;

	public float blackHoleScaleFactor;
	// Use this for initialization
	void Start () {
		initialSize = transform.localScale;
		transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
		StartCoroutine (blackHoleAppears ());
	}

	void awake()
	{
		initialSize = transform.localScale;
		transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
		StartCoroutine (blackHoleAppears ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator blackHoleAppears()
	{
		blackHoleMusic.Play ();
		weights [0] = 0.0f;
		weights [1] = 1.0f;
		mixer.TransitionToSnapshots (sn, weights, 3);
		yield return new WaitForSeconds (1.5f);
		transform.DOScale (initialSize, 1);
		yield return new WaitForSeconds (1);
		blackHoleAppeared = true;
	}



	void OnTriggerEnter(Collider Col)
	{
		
		if (!win) {
			if (Col.tag == "Debris") {

				blackHoleLevel++;

				Vector3 newScale = new Vector3 (transform.localScale.x * blackHoleScaleFactor, transform.localScale.y * blackHoleScaleFactor, transform.localScale.z * blackHoleScaleFactor);
				transform.DOScale (newScale, 1); 

				if (newScale.x > 752) newScale = new Vector3 (752, 752, 752);
				
				Destroy (Col.gameObject);

				blackHoleLevel++;
				if (blackHoleLevel == 1) {
					gm.playCrowd (2);
					gm.playAnnouncer (9);
				}
				if (blackHoleLevel == 2) {
					gm.playCrowd (0);
					gm.playAnnouncer (1);
				}
				if (blackHoleLevel == 10) {
					gm.playCrowd (3);
					gm.playAnnouncer (1);
				}


			}
			if (Col.tag == "pl1") {

				gm.TriggerWin (2);
				win = true;

			}

			if (Col.tag == "pl2") {

				gm.TriggerWin (1);
				win = true;

			}
		}


	}

}
