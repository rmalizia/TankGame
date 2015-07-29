using UnityEngine;
using System.Collections;

public class ElevateCannon : MonoBehaviour {

	public Transform maingunTransform;
	private Vector3 up = new Vector3(-1,0,0);
	private Vector3 down = new Vector3(1, 0, 0);
	
	// Use this for initialization
	void Start () {
		
		maingunTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey(KeyCode.W))
		{
			if (maingunTransform.eulerAngles.x <= 0)
			{
				maingunTransform.Rotate(up);
			}
			else if (maingunTransform.eulerAngles.x > 270)
			{
				maingunTransform.Rotate(up);
			}
		}
		
		if (Input.GetKey(KeyCode.S))
		{
			if (maingunTransform.eulerAngles.x >= 270 && maingunTransform.eulerAngles.x <= 359)
			{
				maingunTransform.Rotate(down);
			}
		}
	}
}
