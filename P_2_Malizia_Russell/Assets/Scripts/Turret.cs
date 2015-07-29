using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {


	public Transform turretTransform;
	

	void Start () {
		
		turretTransform = transform;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey(KeyCode.A))
		{
			turretTransform.Rotate(new Vector3(0, -1, 0));
		}
		if (Input.GetKey(KeyCode.D))
		{
			turretTransform.Rotate(new Vector3(0, 1, 0));
		}
		
	}
}
