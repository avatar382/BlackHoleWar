using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class blackHoleLogic : MonoBehaviour {

	Vector3 initialSize;
	bool blackHoleAppeared = false;

	public int blackHoleLevel;
	public int blackHoleStepUp;

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
		yield return new WaitForSeconds (1.5f);
		transform.DOScale (initialSize, 1);
		yield return new WaitForSeconds (1);
		blackHoleAppeared = true;
	}



	void OnTriggerEnter(Collider Col)
	{
		if (Col.tag == "Debris") {

			Vector3 newScale = new Vector3 (transform.localScale.x * blackHoleScaleFactor, transform.localScale.y * blackHoleScaleFactor, transform.localScale.z * blackHoleScaleFactor);
			transform.DOScale (newScale, 1); 
			Destroy (Col.gameObject);
			blackHoleLevel++;
		}


	}

}
