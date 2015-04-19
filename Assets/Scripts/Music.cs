using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {
	
	public AudioClip intro;
	public AudioClip[] clips;
	
	private AudioSource audioSource;
	// Use this for initialization
	void Start () {
		GameEvent.GameStart += GameStart;
		GameEvent.GameOver += GameOver;
		audioSource = gameObject.GetComponent("AudioSource") as AudioSource;
		audioSource.clip = intro;
		audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void GameStart(){
		audioSource.clip = GetRandomClip();
		audioSource.Play();	
	}
	
	void GameOver(){
		audioSource.clip = intro;
		audioSource.Play();	
	}	
	
	private AudioClip GetRandomClip(){
		return clips[Random.Range(0,clips.Length)];
	}
}
