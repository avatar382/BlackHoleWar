using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class blackHoleLogic : MonoBehaviour {

	Vector3 initialSize;
	bool blackHoleAppeared = false;
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
}
