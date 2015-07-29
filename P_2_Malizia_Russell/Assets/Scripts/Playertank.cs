using UnityEngine;
using System.Collections;

public class Playertank : MonoBehaviour {

	// tank physics
	private Vector3 accel;
	public float throttle;
	private float deadZone = .001f;
	private Vector3 myRight;
	private Vector3 velo;
	private Vector3 flatVelo;
	private Vector3 relativeVelocity;
	private Vector3 dir;
	private Vector3 flatDir;
	private Vector3 tankUp;
	private Transform tankTransform;
	private Rigidbody tankRigidbody;
	private Vector3 engineForce;
	
	private Vector3 turnVec;
	private Vector3 imp;
	private float rev;
	private float actualTurn;
	private float tankMass;
	private Transform[] wheelTransform = new
		Transform[4]; //these are the transforms for the 4 wheels
	public float actualGrip;
	public float horizontal; //horizontal input control
	private float maxSpeedToTurn = .2f;
	
	//the physical transforms for the tank's wheels
	public Transform frontLeftWheel;
	public Transform frontRightWheel;
	public Transform rearLeftWheel;
	public Transform rearRightWheel;
	
	//these transform parents allow the wheels to turn
	public Transform LFWheelTransform;
	public Transform RFWheelTransform;
	
	//tank physics
	public float power = 300;
	public float maxSpeed = 20;
	public float tankGrip = 150;
	public float turnSpeed = 1.0f;
	
	private float slideSpeed = 5.0f;
	public float mySpeed;
	
	private Vector3 tankRight;
	private Vector3 tankFwd;
	private Vector3 tempVEC;
	
	
	void  Start (){
		Initialize();
	}
	
	void  Initialize (){
		//cache a reference to our tank's transform
		tankTransform = transform;
		//cache the rigidbody for the tank
		tankRigidbody = rigidbody;
		//cache our vector up direction
		tankUp = tankTransform.up;
		//cache the mass of our tank
		tankMass = rigidbody.mass;
		//cache the forward world vector for the tank
		tankFwd = Vector3.forward;
		//cache the world right vector for the tank
		tankRight = Vector3.right;
		// call to set up wheels;
		setUpWheels();
		//set a COG her and lower the center of mass to
		// a negative value in Y axis to prevent tank from
		//flipping over
		tankRigidbody.centerOfMass = ( new Vector3(0f,-0.7f,.35f));
		
	}
	
	void  Update (){
		//car the function to start processing all vehicle physics
		tankPhysicsUpdate();
		
		//call the function to see what input we are using and apply it
		checkInput();
		
		
	}
	
	void  LateUpdate (){
		//this function makes the visual 3d wheels rotate and turn
		rotateVisualWheels();
		
		//engineSound();
		
	}
	
	void  setUpWheels (){
		if((null==frontLeftWheel || null == frontRightWheel || null == rearLeftWheel || null == rearRightWheel))
		{
			Debug.LogError("One or more the the wheel transforms have not been plugged in");
			Debug.Break();
			
		}
		else
		{
			//set up the tank's wheel transforms
			wheelTransform[0] = frontLeftWheel;
			wheelTransform[1] = rearLeftWheel;
			wheelTransform[2] = frontRightWheel;
			wheelTransform[3] = rearRightWheel;
		}
	}
	
	private Vector3 rotationAmount;



	void  rotateVisualWheels (){
		//front wheels visual rotation while steering
	

		LFWheelTransform.localEulerAngles = new Vector3(0, horizontal * 45 , 0);
		RFWheelTransform.localEulerAngles = new Vector3(0, horizontal * 30 , 0);
		
		rotationAmount = tankRight * (relativeVelocity.z * 1.6f * Time.deltaTime * Mathf.Rad2Deg);
		
		wheelTransform[0].Rotate(rotationAmount);
		wheelTransform[1].Rotate(rotationAmount);
		wheelTransform[2].Rotate(rotationAmount);
		wheelTransform[3].Rotate(rotationAmount);
	}	
	
	private float deviceAccelerometerSensitivity = 2;
	
	void  checkInput (){

			horizontal = Input.GetAxis("Horizontal");
			throttle = Input.GetAxis("Vertical");
		
	}
	
	void  tankPhysicsUpdate (){
		//grab all the physics infor
		myRight = tankTransform.right;
		
		//find out velicity
		velo = tankRigidbody.velocity;
		
		tempVEC = (new Vector3(velo.x,0f,velo.z));
		
		//figure out our velocity without y movement 
		flatVelo = tempVEC;
		
		//find out which direction we are moving
		dir = transform.TransformDirection(tankFwd);
		
		tempVEC = (new Vector3(dir.x,0f,dir.z));
		
		//calculate direction, removing y movement
		flatDir = Vector3.Normalize(tempVEC);
		
		//calculate relative velocity
		relativeVelocity = tankTransform.InverseTransformDirection(flatVelo);
		
		//calculate slide
		slideSpeed = Vector3.Dot(myRight,flatVelo);
		
		//calculate current speed
		mySpeed = flatVelo.magnitude;
		
		//check to see if we are moving in reverse
		rev = Mathf.Sign(Vector3.Dot(flatVelo,flatDir));
		
		//calculate engine force 
		engineForce = (flatDir * (power * throttle) * tankMass);
		
		//turning
		actualTurn = horizontal;
		
		//if we're in reverse, we reverse the turning direction
		if(rev < 0.1f)
		{
			actualTurn =- actualTurn;
		}
		
		//calculate torque for applying to rigidbody
		turnVec = ((( tankUp * turnSpeed ) * actualTurn ) * tankMass) * 800;
		
		actualGrip = Mathf.Lerp(100, tankGrip, (mySpeed * 0.02f));
		imp = myRight *(-slideSpeed * tankMass * actualGrip);
		
	}
	
	void  slowVelocity (){
		tankRigidbody.AddForce(-flatVelo * 0.8f);
	}
	
	//this controls the sound of the engine
	//function engineSound()
	//{
	//	audio.pitch = 0.0f + mySpeed * 0.025f;
	//	
	//	if(mySpeed > 30)
	//	{
	//		audio.pitch = 0.25f + mySpeed * 0.015f;
	//	}
	//	if(mySpeed > 40)
	//	{
	//		audio.pitch = 0.20f + mySpeed * 0.013f;
	//	}
	//	if(mySpeed > 49)
	//	{
	//		audio.pitch = 0.15f + mySpeed * 0.011f;
	//	}
	//	if(AudioClip.pitch > 2.0f){
	//		audio.pitch = 2.0f;
	//	}
	
	
	//}
	
	void  FixedUpdate (){
		if(mySpeed < maxSpeed)
		{
			//apply the engine force to the rigidbody
			tankRigidbody.AddForce(engineForce * Time.deltaTime);
		}
		//if we're going too slow to allow tank to rotate
		if(mySpeed > maxSpeedToTurn)
		{
			tankRigidbody.AddTorque(turnVec * Time.deltaTime);
		}
		else if(mySpeed < maxSpeedToTurn)
		{
			return;
		}
		//apply forces to our rigidbody for grip
		tankRigidbody.AddForce(imp * Time.deltaTime);
		
	}
}
	
