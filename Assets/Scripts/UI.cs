using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {
	
	public GUIText gameStatus;
	public GUIText gameScore;
	public GUIText gameInstruction;
	public GUIText gameQuit;
	
	public OTSprite[] digits;
	
	// Seems like this font is not so friendly with French characters :(
	private const string TEXT_GAMESTART = "Eileen";
	private const string TEXT_GAMEOVER = "Game Over";	
	private const string TEXT_INSTRUCTION = "Appuyez sur la barre d'espace a commencer\n" +
											"Boutons: A - Reculer, D - Avancer, Barre d'espace - Sauter\n" +
											"Barre d'espace en air - Doubler Sauter (penalite HP)";
	// Use this for initialization
	void Start () {
		GameEvent.GameStart += GameStart;
		GameEvent.GameOver += GameOver;
		gameStatus.text = TEXT_GAMESTART;
		gameInstruction.text = TEXT_INSTRUCTION;
		gameQuit.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		// When the game is not started, press the "Spacebar" to start.
		if(Input.GetKeyDown(KeyCode.Space)&&!GameEvent.gameStarted){
			GameEvent.OnGameStart();
		}
		if(GameEvent.gameStarted){
			UpdateHP();
			UpdateScore();
		}
		// Press "Escape" to quit the game
		if(Input.GetKeyDown(KeyCode.Escape)){
			gameQuit.enabled = true;
			// Pause the game
			Time.timeScale = 0;	
		}
		if(gameQuit.enabled){
			// Y/N Confirmation
			if(Input.GetKeyDown(KeyCode.Y)){
				Application.Quit();
			}else if(Input.GetKeyDown(KeyCode.N)){
				gameQuit.enabled = false;
				// Resume the game
				Time.timeScale = 1; 
			}	
		}
	}
	
	private void GameStart(){
		gameStatus.enabled = false;
		gameInstruction.enabled = false;
	}
	
	private void GameOver(){
		// Game over text
		// High score and hints
		gameStatus.text = TEXT_GAMEOVER + "\nLe meilleur score: " + PlayerPrefs.GetInt("highscore",0);
		gameStatus.enabled = true;
		gameInstruction.enabled = true;
	}
	
	private void UpdateScore(){
		// Game score text
		gameScore.text = "Score: " + GameEvent.score.ToString();
	}
	
	// Update the player's HP texts
	private void UpdateHP(){
		int hp = Player.HP;
		int [] hpIndex = new int[3];
		hpIndex[0] = hp%10; // units
		hpIndex[1] = (hp/10)%10; // tens
		hpIndex[2] = hp/100; // hundreds
		for(int i = 0;i<3;i++){
			digits[i].frameIndex = hpIndex[i] + 1;
		}
		// Hide the hundres digital if it is zero
		digits[2].visible = digits[2].frameIndex  == 1 ? false:true;	
	}
}
