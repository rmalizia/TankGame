using UnityEngine;
using System.Collections;

public class Detect_enemyhit : MonoBehaviour {

	public GameObject cannonShell;
	
	// Use this for initialization
	void Start()
	{
		
		cannonShell = gameObject;
	}
	
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject == cannonShell)
		{
			Destroy(this);
		}
		
	}
}
