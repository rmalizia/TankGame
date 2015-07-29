using UnityEngine;
using System.Collections;

public class Detect_tankhit : MonoBehaviour {


	public GameObject enemyBullet;
	public int lifeCount = 10;
	
	// Use this for initialization
	void Start () {
		
		enemyBullet = gameObject;
	}
	
	void OnCollisionEnter(Collision col){

		BroadcastMessage("HP", lifeCount);
		if (col.gameObject == enemyBullet) {
				
			--lifeCount;
						
			if (lifeCount <= 0) {
								Destroy (this);
						}
						if (Input.GetKey (KeyCode.F)) {
								
						}
				}
		}
		
		
	}
		
		
		

