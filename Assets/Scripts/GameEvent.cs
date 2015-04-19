using UnityEngine;
using System.Collections;

public class GameEvent : MonoBehaviour {
	
	public delegate void EventHandler();
	public static event EventHandler GameStart,GameOver;
	
	public static bool gameStarted;
	public static int score = 0;
	
	// Use this for initialization
	void Start () {
		gameStarted = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(gameStarted){
			UpdateScore();
		}
	}
	
	public static void OnGameStart(){
		if(GameStart !=null){
			gameStarted = true;
			GameStart();
		}
	}
	
	public static void OnGameOver(){
		if(GameOver != null){
			gameStarted = false;
			CheckHighScore();
			GameOver();	
		}
	}
	
	private static void UpdateScore(){
		score = (int)Chaser.positionX + 20;
	}
	
	// Check if this is the highest score, then update the record.
	private static void CheckHighScore(){
		if (score > PlayerPrefs.GetInt("highscore",0)){
			PlayerPrefs.SetInt("highscore",score); 
		}
	}
	
}
