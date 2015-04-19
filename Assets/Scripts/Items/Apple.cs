using UnityEngine;
using System.Collections;

public class Apple : MonoBehaviour {
	
	public int bonus;
	
	private OTSprite sprite;
	// Use this for initialization
	void Start () {
		sprite = gameObject.GetComponent("OTSprite") as OTSprite;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if (other.name.Equals("Player")){
			// Heal when the player touches it
			Player.UpdateHP(bonus);
			sprite.visible = false;
		}	
	}
}
