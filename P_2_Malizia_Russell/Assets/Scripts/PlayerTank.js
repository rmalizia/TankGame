
// tank physics
private var accel : Vector3;
public var throttle : float;
private var deadZone : float = .001;
private var myRight : Vector3;
private var velo : Vector3;
private var flatVelo : Vector3;
private var relativeVelocity : Vector3;
private var dir : Vector3;
private var flatDir : Vector3;
private var tankUp : Vector3;
private var tankTransform : Transform;
private var tankRigidbody : Rigidbody;
private var engineForce : Vector3;

private var turnVec : Vector3;
private var imp : Vector3;
private var rev : float;
private var actualTurn : float;
private var tankMass : float;
private var wheelTransform : Transform[] = new
Transform[4]; //these are the transforms for the 4 wheels
public var actualGrip : float;
public var horizontal : float; //horizontal input control
private var maxSpeedToTurn : float = .2;

//the physical transforms for the tank's wheels
public var frontLeftWheel : Transform;
public var frontRightWheel : Transform;
public var rearLeftWheel : Transform;
public var rearRightWheel : Transform;

//these transform parents allow the wheels to turn
public var LFWheelTransform : Transform;
public var RFWheelTransform : Transform;

//tank physics
public var power : float = 300;
public var maxSpeed : float = 50;
public var tankGrip : float = 70;
public var turnSpeed : float = 3.0;

private var slideSpeed : float = 5.0;
public var mySpeed : float;

private var tankRight : Vector3;
private var tankFwd : Vector3;
private var tempVEC : Vector3;


function Start () 
{
	Initialize();
}

function Initialize()
{
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
	tankRigidbody.centerOfMass = Vector3(0,-0.7,.35);
	
}

function Update () 
{
	//car the function to start processing all vehicle physics
	tankPhysicsUpdate();
	
	//call the function to see what input we are using and apply it
	checkInput();
	
		
}

function LateUpdate()
{
	//this function makes the visual 3d wheels rotate and turn
	rotateVisualWheels();
	
	//engineSound();
	  
}

function setUpWheels()
{
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

private var rotationAmount : Vector3;

function rotateVisualWheels()
{
	//front wheels visual rotation while steering
	LFWheelTransform.localEulerAngles.y = horizontal * 30;
	RFWheelTransform.localEulerAngles.y = horizontal * 30;
	
	rotationAmount = tankRight * (relativeVelocity.z * 1.6 * Time.deltaTime * Mathf.Rad2Deg);
	
	wheelTransform[0].Rotate(rotationAmount);
	wheelTransform[1].Rotate(rotationAmount);
	wheelTransform[2].Rotate(rotationAmount);
	wheelTransform[3].Rotate(rotationAmount);
}	

private var deviceAccelerometerSensitivity : float = 2;

function checkInput()
{
	if(Application.platform == RuntimePlatform.WindowsEditor || RuntimePlatform.WindowsWebPlayer)
	{
		horizontal = Input.GetAxis("Horizontal");
		throttle = Input.GetAxis("Vertical");
	}
}

function tankPhysicsUpdate()
{
	//grab all the physics infor
	myRight = tankTransform.right;
	
	//find out velicity
	velo = tankRigidbody.velocity;
	
	tempVEC = Vector3(velo.x,0,velo.z);
	
	//figure out our velocity without y movement 
	flatVelo = tempVEC;
	
	//find out which direction we are moving
	dir = transform.TransformDirection(tankFwd);
	
	tempVEC = Vector3(dir.x,0,dir.z);
	
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
	
	actualGrip = Mathf.Lerp(100, tankGrip, (mySpeed * 0.02));
	imp = myRight *(-slideSpeed * tankMass * actualGrip);
	
}

function slowVelocity()
{
	tankRigidbody.AddForce(-flatVelo * 0.8);
}

//this controls the sound of the engine
//function engineSound()
//{
//	audio.pitch = 0.0 + mySpeed * 0.025;
//	
//	if(mySpeed > 30)
//	{
//		audio.pitch = 0.25 + mySpeed * 0.015;
//	}
//	if(mySpeed > 40)
//	{
//		audio.pitch = 0.20 + mySpeed * 0.013;
//	}
//	if(mySpeed > 49)
//	{
//		audio.pitch = 0.15 + mySpeed * 0.011;
//	}
//	if(AudioClip.pitch > 2.0){
//		audio.pitch = 2.0;
//	}
	

//}

function FixedUpdate()
{
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






