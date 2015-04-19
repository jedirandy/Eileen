using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformGenerator : MonoBehaviour {
	
	public int numberPlatforms; // The number of platforms
	public Vector2 minGap,maxGap; // The gap of two platforms
	public int minY,maxY; // Y range
	public int oddsOfItem; // Odds of spwaning an item 
	public OTSprite[] platforms; // Platform sprites
	public OTSprite[] items; // Item sprites
	public Vector3 initialPosition;
	
	private Queue<OTSprite> platformQueue; // The queue to store the platforms
	private Queue<OTSprite> itemQueue; // The queue to store the items
	private Vector3 newPosition; // The position of the newest platform
	// Use this for initialization
	void Start () {		
		GameEvent.GameStart += GameStart;
		GameEvent.GameOver += GameOver;
		// Instantiate n platforms
		platformQueue = new Queue<OTSprite>(numberPlatforms);
		for(int i=0;i<numberPlatforms;i++){
			platformQueue.Enqueue(GetRandomSprite(platforms));
		}
		itemQueue = new Queue<OTSprite>();
	}
	
	// Update is called once per frame
	void Update () {
		// Update the position if the chaser surpassed the oldest platform
		if(GameEvent.gameStarted){
			if(platformQueue.Peek().transform.position.x + platformQueue.Peek().transform.localScale.x/2 < Chaser.positionX){
				UpdatePlatform(ref newPosition,true);
			}
			// Recycle items
			if (itemQueue.Count != 0){
				if (itemQueue.Peek().transform.position.x < Chaser.positionX){
					OT.DestroyObject(itemQueue.Dequeue());	
				}
			}
		}
	}
	
	private void GameStart(){		
		newPosition = initialPosition;
		InitilizePlatform();
	}
	
	private void GameOver(){
		ClearItems();
	}
	
	// Initilize the platforms
	private void InitilizePlatform(){
		for(int i=0;i<numberPlatforms;i++){
			UpdatePlatform(ref newPosition,false);
		}
	}
		
	// Update the platform queue
	private void UpdatePlatform(ref Vector3 spawnPosition,bool generateItems){
		OTSprite newSprite;
		newSprite = GetRandomSprite(platforms);
		newSprite.position = spawnPosition;
		
		if(generateItems){
			// Roll the dice and see what you've got
			if (jackpot(oddsOfItem)){
				OTSprite item = GetRandomSprite(items);
				item.position = new Vector2(spawnPosition.x+Random.Range(-4f,4f),spawnPosition.y+Random.Range(2f,4f));
				itemQueue.Enqueue(item);
			}
		}
		
		// Destroy the oldest object
		OT.DestroyObject(platformQueue.Dequeue());
		platformQueue.Enqueue(newSprite);
		
		spawnPosition = GetNewPosition(spawnPosition);
	}
	
	// Get a new random position based on the current position
	private Vector3 GetNewPosition(Vector3 currentPosition){
		Vector3 newPosition;
		newPosition = currentPosition + new Vector3(Random.Range(minGap.x,maxGap.x),Random.Range(minGap.y,maxGap.y),0f);	
		if(newPosition.y < minY){
			newPosition.y += Random.Range(1f,maxGap.y); 
		}
		else if(newPosition.y > maxY){
			newPosition.y -= Random.Range(1f,maxGap.y);	
		}
		return newPosition;
	}
	
	// Clear items from the itemQueue
	private void ClearItems(){
		int numberOfItems = itemQueue.Count;
		for(int i=0;i<numberOfItems;i++){
			OT.DestroyObject(itemQueue.Dequeue());
		}
	}
	
	// Get a random sprite from the array
	private OTSprite GetRandomSprite(OTSprite[] sprites){
		int index = Random.Range(0,sprites.Length);
		return Instantiate(sprites[index]) as OTSprite;
	}
	
	// Try your luck!
	private bool jackpot(int odds){
		return Random.Range(1,100)<=odds? true:false;
	}
}
