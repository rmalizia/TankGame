using UnityEngine;
using System.Collections;

public class Maingun : MonoBehaviour {

	public GameObject shotPrefab;
	public Transform Spawnpoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey(KeyCode.Space)) 
		{
			FireCannon();
		}
	}

	void FireCannon()
	{
		GameObject shot = (GameObject)Instantiate (shotPrefab, Spawnpoint.position, Spawnpoint.rotation);
		shot.rigidbody.AddForce (Spawnpoint.forward * 80, ForceMode.Impulse);
	}

}

