using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	
	public int damage;
	public OTAnimatingSprite animExplosion;

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
			// Explode when the player touches it
			OTAnimatingSprite explode = Instantiate(animExplosion) as OTAnimatingSprite;
			explode.transform.position = transform.position + Vector3.up*1f;
			explode.PlayOnce("explode");
			// Cast a damage to the player
			Player.UpdateHP(damage*-1);
			// Hide the sprite
			sprite.visible = false;
		}	
	}
}
