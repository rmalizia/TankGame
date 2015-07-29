using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	// Use this for initialization

		public GameObject[] gun; //gun attack player
		public GameObject prefabShot; //shot
		public float enemyShotSpeed;
		private float delayAttack; 
		public float timeAttack; 
		
		//player
		private Transform player;
		
		// Use this for initialization
		void Start () {
			player = GameObject.FindGameObjectWithTag("Player").transform;
		}
		
		// Update is called once per frame
		void Update () {
			delayAttack += Time.deltaTime;
			if (delayAttack >= timeAttack){            
				for(int x = 0; x < gun.Length; x++){
					GameObject shot = Instantiate(prefabShot, gun[x].transform.position, Quaternion.identity) as GameObject;
					shot.rigidbody.AddForce(((Vector2)(player.position - shot.transform.position)).normalized * 500); 

				}                        
				delayAttack = 2;
			}
			
		}
	}


