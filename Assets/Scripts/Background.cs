using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {
	
	public Vector3 initialLocalPosition;
	// Use this for initialization
	void Start () {
		GameEvent.GameStart += GameStart;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(-0.1f*Time.deltaTime,0f,0f);
	}
	
	private void GameStart(){
		transform.localPosition = initialLocalPosition;	
	}
}
