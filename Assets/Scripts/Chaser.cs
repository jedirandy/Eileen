using UnityEngine;
using System.Collections;

public class Chaser : MonoBehaviour {
	
	public static float positionX;
	public static float speed;
	
	public Vector3 initialPosition;
	public float initialSpeed; 
	public float maxSpeed; // Max speed
	public float xFactor; // The X factor, which determine the speed's increment rate, be carefull!
	// Use this for initialization
	void Start () {
		GameEvent.GameStart += GameStart;
		GameEvent.GameOver += GameOver;
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.right*Time.deltaTime*speed);
		positionX = transform.localPosition.x;
	}
	
	void FixedUpdate(){
		IncreaseSpeed();
	}
		
	private void GameStart(){
		enabled = true;
		speed = initialSpeed;
		transform.position = initialPosition;
		positionX = transform.position.x;
	}
	
	private void GameOver(){
		enabled = false;
	}
	
	// Increase the speed during the game
	private void IncreaseSpeed(){
		if(speed>=maxSpeed){
			speed = maxSpeed;	
		}else{
			speed += GameEvent.score/(Mathf.Pow(10,xFactor));	
		}
	}	
}
