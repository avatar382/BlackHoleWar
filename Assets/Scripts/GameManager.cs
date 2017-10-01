using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour {

	public Camera winCamP1;
	public Camera winCamP2;

	public GameObject p1WinUI;
	public GameObject p2WinUI;

	public GameObject VictorySong;

	public GameObject[] disableonWin;

	public GameObject audioPlayer;
	public GameObject audioPlayerAnnouncer;

	public AudioMixer mixer;
	public AudioMixerSnapshot[] sn;

	public AudioClip[] announcerSounds;
	public AudioClip[] CroudChants;



	// Use this for initialization
	void Start () {
		playCrowd (1);
		playAnnouncer (6);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void playAnnouncer(int s)
	{
		playAudioAnnouncer (announcerSounds [s]);


	}

	public void playCrowd(int s)
	{
		playAudio (CroudChants [s]);


	}

	void playAudio(AudioClip clip)
	{

		GameObject g = Instantiate (audioPlayer);

		g.GetComponent<AudioSource> ().clip = clip;
		g.GetComponent<AudioSource> ().Play ();

	}
	void playAudioAnnouncer(AudioClip clip)
	{

		GameObject g = Instantiate (audioPlayerAnnouncer);

		g.GetComponent<AudioSource> ().clip = clip;
		g.GetComponent<AudioSource> ().Play ();

	}

	public void TriggerWin(int win)
	{
		playCrowd (1);

		//disable all game objects on win
		foreach (GameObject g in disableonWin)
			g.SetActive (false);

		VictorySong.SetActive (true);

		float[] w = new float[2];
		w [0] = 1;
		w [1] = 0;

		mixer.TransitionToSnapshots (sn, w, 0); 
		
		if (win == 1) {
			playAnnouncer (7);
			winCamP1.enabled = true;
			p1WinUI.SetActive (true);
			//p1WinUI.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);

			//Vector3 ns = new Vector3 (45, 45, 45);
			//p1WinUI.transform.DOScale (ns, 1.5f);
			//winCamP1.gameObject.transform.DOBlendableMoveBy (Vector3.forward * 400, 4);
		}
		if (win == 2) {
			playAnnouncer (8);
			winCamP2.enabled = true;
			p2WinUI.SetActive (true);
			//p2WinUI.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);

			//Vector3 ns = new Vector3 (45, 45, 45);
			//p1WinUI.transform.DOScale (ns, 1.5f);
			//winCamP2.gameObject.transform.DOBlendableMoveBy (Vector3.forward * 400, 4);

		}
		StartCoroutine (winTimeOut ());

	}

	IEnumerator winTimeOut()
	{

		yield return new WaitForSeconds(10);
		SceneManager.LoadScene (0);




	}


}
