using UnityEngine;

public class Ball : MonoBehaviour
{
	[SerializeField]
	Paddle  paddle1;					// Linked to Paddle in Unity Editor
	[SerializeField]
	float   xPush = 2f;
	[SerializeField]
	float   yPush = 15f;
	[SerializeField]
	AudioClip[] ballSounds;				// Assigned in Unity Editor
	[SerializeField]
	float   randomFactor = 0.5f;        // Factor to prevent boring ball loops default value from example video
	[SerializeField]
	float maxVelocity = 50f;			// Tunable max velocity limit

	/***
	*		Current state values.
	***/

	Vector2 paddleToBallVector; // Distance to paddle and ball
	bool ballLaunched = false;

	/***
	*		Cached componenet references.
	***/

	AudioSource myAudioSource;
	Rigidbody2D myRigidBody2D;

	/***
	*       Start() is only used to set the distance vector between the ball and the paddle.
	***/
	void Start()
	{
		paddleToBallVector = transform.position - paddle1.transform.position;
		myAudioSource = GetComponent<AudioSource>();
		myRigidBody2D = GetComponent<Rigidbody2D>();
	}   // Start()

	/***
	*       Update() in this class will make the ball stick to the paddle using the distance
	*   vector calculated in Start().  To make it so it doesn't slide around when you move
	*   the Paddle quickly, Go to Edit --> Project Settings in the Unity Editor and by
	*   clicking on the plus sign you can add Paddle and Ball scripts.  Have Paddle be first
	*   and set the value for Paddle to 20 and Ball to 50.  Now it should stick better to
	*   the paddle, even when you move it fast.
	***/
	void Update()
	{
		if (!ballLaunched)
			LockBallToPaddle();

		LaunchBallOnMouseClick();
	}   // Update()

	/***
	*       LockBallToPaddle() will make the ball stick to the paddle using the distance
	*   vector calculated in Start().  To make it so it doesn't slide around when you move
	*   the Paddle quickly, Go to Edit --> Project Settings in the Unity Editor and by
	*   clicking on the plus sign you can add Paddle and Ball scripts.  Have Paddle be first
	*   and set the value for Paddle to 20 and Ball to 50.  Now it should stick better to
	*   the paddle, even when you move it fast.
	***/
	void LockBallToPaddle()
	{
		Vector2 paddlePos = new Vector2(paddle1.transform.position.x, paddle1.transform.position.y);
		transform.position = paddlePos + paddleToBallVector;
	}   // LockBallToPaddle()

	/***
	*       LaunchBallOnMouseClick() will make the ball launch off the paddle and disable it
	*   sticking to the paddle after that.
	***/
	void LaunchBallOnMouseClick()
	{
		if (Input.GetMouseButtonDown(0))
		{   // Left mouse button was pressed
			ballLaunched = true;
			myRigidBody2D.velocity = new Vector2(xPush, yPush);
		}   // if
	}   // LaunchBallOnMouseClick()

	/***
    *       OnCollisionEnter2D() will make the sound we program it for in the Editor when the
    *   ball collides with something.
    ***/
	private void OnCollisionEnter2D()
	{
		if (ballLaunched)
		{
			//Vector2 velocityTweak = new Vector2(Random.Range(0f, randomFactor), Random.Range(0f, randomFactor));
			Vector2 tweakVelocityOfBoth = new Vector2(Random.Range(0.1f, randomFactor), Random.Range(0.1f, randomFactor));
			Vector2 tweakVelocityOfX = new Vector2(Random.Range(0.1f, randomFactor), 0);
			Vector2 tweakVelocityOfY= new Vector2(0, Random.Range(0.1f, randomFactor));

			myAudioSource.PlayOneShot(ballSounds[Random.Range(0, ballSounds.Length)]);
			//myRigidBody2D.velocity += velocityTweak;
			//myRigidBody2D.velocity = myRigidBody2D.velocity.normalized * ballSpeed;
			if (myRigidBody2D.velocity.x >= 0 && myRigidBody2D.velocity.y >= 0)
			{

				myRigidBody2D.velocity += tweakVelocityOfBoth;

				//Debug.Log(myRigidBody2d.velocity);
			}
			else if (myRigidBody2D.velocity.x <= 0 && myRigidBody2D.velocity.y <= 0)
			{
				myRigidBody2D.velocity -= tweakVelocityOfBoth;
			}
			else if (myRigidBody2D.velocity.x >= 0 && myRigidBody2D.velocity.y <= 0)
			{
				myRigidBody2D.velocity += tweakVelocityOfX;
				myRigidBody2D.velocity -= tweakVelocityOfY;
			}
			else if (myRigidBody2D.velocity.x <= 0 && myRigidBody2D.velocity.y >= 0)
			{
				myRigidBody2D.velocity += tweakVelocityOfY;
				myRigidBody2D.velocity -= tweakVelocityOfX;
			}

			myRigidBody2D.velocity = Vector2.ClampMagnitude(myRigidBody2D.velocity, maxVelocity);	// Don't exceed the max valocity setting
		}	// if
	}   // OnCollisionEnter2D()
}   // class Ball