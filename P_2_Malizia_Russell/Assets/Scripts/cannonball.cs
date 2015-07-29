using UnityEngine;
using System.Collections;

public class cannonball : MonoBehaviour {

	public GameObject explosionPrefab;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col)
	{
		Instantiate (explosionPrefab, transform.position, transform.rotation);
		Destroy (gameObject);
	}
}
