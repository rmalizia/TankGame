using UnityEngine;
using System.Collections;

public class Camera_Switch : MonoBehaviour {
	
	public Camera Main_Camera;
	public Camera Tank_Camera;
	private GUI screen;
	private GUIContent screen_cont;
	
	// Use this for initialization
	void Start () {
		Main_Camera = camera;
		Tank_Camera = camera;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey(KeyCode.F))
		{
			if(Main_Camera.enabled)
			{
				Main_Camera.camera.enabled = false;
				Tank_Camera.camera.enabled = true;
				//screen_cont.text = "Tank Camera";
				print("Tank Camera");
				
			}
			else
			{
				Main_Camera.camera.enabled = true;
				Tank_Camera.camera.enabled = false;
				//screen_cont.text = "Main Camera";
			}
		}
		
	}
}

