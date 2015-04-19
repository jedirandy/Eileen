using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public static int HP;
	
	public int playerInitialHP; // Initial health point
	public int doubleJumpPenalty; // Penalty when performs a double jump
	public Vector3 initialPosition;
	public Vector3 jumpForce; // Force of jumping
	public Vector3 doubleJumpForce; // Force of double jump
	
	private bool onPlatform;
	private bool onDoubleJump; 
	private bool zooming; // is Zooming
	private float zoomSpeed = 0.1f; // how fast do we zoom in/out
    private float zoomMin = -0.3f; // Zoomed out value
    private float zoomMax = -0f; // Zoomed in value
	private float zoomX = 20f;	// X Gap between the chaser and the player
	private float zoomY = 8f; // Position Y to trigger the zooming 
	
	private OTSprite player;
	// Use this for initialization
	void Start () {
		GameEvent.GameStart += GameStart;
		GameEvent.GameOver += GameOver;
		// Stuck the object
		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;
		player = gameObject.GetComponent("OTSprite") as OTSprite;
	}
	
	// Update is called once per frame
	void Update () {
		if(GameEvent.gameStarted){
			// Game over events
			// Fall
			if(transform.position.y<-12f){
				GameEvent.OnGameOver();
			}
			// Caught up by the chaser
			if(transform.position.x<Chaser.positionX){
				GameEvent.OnGameOver();	
			}
			// HP exhaustied
			if(HP<=0){
				GameEvent.OnGameOver();	
			}
			
			// Constrain the rotation
			transform.rotation = Quaternion.identity; 
			
			// Zoom the view according to the player's position
			if (transform.position.y > zoomY || transform.position.x-Chaser.positionX>zoomX ){
				zooming = true;
			}else{
				zooming = false;
			}
			
            // we are zooming in or out
            if (zooming)
            {
                // zooming out
                OT.view.zoom -= zoomSpeed * Time.deltaTime;
                // cap zooming at min
                if (OT.view.zoom < zoomMin)
                {
                    OT.view.zoom = zoomMin;
                }
            }else{
				// zooming in
				OT.view.zoom += zoomSpeed * Time.deltaTime;
                if (OT.view.zoom > zoomMax)
                {
                    OT.view.zoom = zoomMax;
                }
			}
        
			// Jump
			if(Input.GetButtonDown("Jump")&&onPlatform){
				onDoubleJump = false;
				rigidbody.AddForce(jumpForce);
			}
			// Double Jump
			if(Input.GetButtonDown("Jump")&&!onPlatform){
				// I'm sorry, but you can't commit suicide :p
				if (HP-doubleJumpPenalty>0&&!onDoubleJump){
					onDoubleJump = true;
					rigidbody.AddForce(doubleJumpForce);
					UpdateHP(-1*doubleJumpPenalty);
				}
			}
			if(onPlatform){
				// Walk on the platform
				rigidbody.AddForce(Input.GetAxis("Horizontal")*20f, 0f, 0f, ForceMode.Acceleration);
				ChangeFrame(player,0);
			}else{
				// Horizontal adjustment when flying
				transform.Translate(Input.GetAxis("Horizontal")*0.075f,0f, 0f );
				ChangeFrame(player,1);	
			}
		}
	}
	
	void OnCollisionEnter(){
		onPlatform = true;	
	}
	
	void OnCollisionExit(){
		onPlatform = false;
	}
	
	private void GameStart(){
		// Reset the player
		OT.view.zoom = 0f;
		transform.position = initialPosition;
		rigidbody.useGravity = true;
		rigidbody.isKinematic = false;
		HP = playerInitialHP;
	}
	
	private void GameOver(){
		// Freeze the player
		rigidbody.useGravity = false;
		rigidbody.isKinematic = true;
	}
	
	// Change the sprite frame
	private void ChangeFrame(OTSprite sprite,int index){
		Vector2 position = sprite.position;
		sprite.frameIndex = index;
		sprite.position = position; // Restore the position
	}
	
	// Update the player's HP
	public static void UpdateHP(int deltaHP){		
		Player.HP += deltaHP;
	}
}
