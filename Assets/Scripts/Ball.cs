using UnityEngine;

public class Ball : MonoBehaviour
{
	[SerializeField]
	Paddle  paddle1;				// Linked to Paddle in Unity Editor
	[SerializeField]
	float   xPush = 2f;
	[SerializeField]
	float   yPush = 15f;
	[SerializeField]
	AudioClip[] ballSounds;			// Assigned in Unity Editor
	[SerializeField]
	float   randomFactor = 0.5f;    // Factor to prevent boring ball loops default value from example video
	[SerializeField]
	float minVelocity = 15f;		// Tunable min velocity limit (15 seems to be the lowest the works)
	[SerializeField]
	float maxVelocity = 25f;		// Tunable max velocity limit

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
	GameStatus  gameStatus;

	/***
	*       Start() is only used to set the distance vector between the ball and the paddle.
	***/
	void Start()
	{
		paddleToBallVector = transform.position - paddle1.transform.position;
		myAudioSource = GetComponent<AudioSource>();
		myRigidBody2D = GetComponent<Rigidbody2D>();
		gameStatus = FindObjectOfType<GameStatus>();
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
		if (gameStatus.IsAutoPlayEnabled())
			LaunchBall();	// Launch ball automatically on a level where this flag is set
	}   // LockBallToPaddle()

	/***
	*       LaunchBallOnMouseClick() will make the ball launch off the paddle and disable it
	*   sticking to the paddle after that.
	***/
	void LaunchBallOnMouseClick()
	{
		if (Input.GetMouseButtonDown(0))
		{   // Left mouse button was pressed
			LaunchBall();
		}   // if
	}   // LaunchBallOnMouseClick()

	/***
	*       LaunchBall() will make the ball launch off the paddle and disable it sticking to
	*   the paddle after that.  It is called in multiple places, so it was broken out from
	*   LaunchBallOnMouseClick() and made a method.
	***/
	public void LaunchBall()
	{
		ballLaunched = true;
		myRigidBody2D.velocity = new Vector2(xPush, yPush);
		if (myRigidBody2D.velocity.magnitude < minVelocity)
			ClampVelocity(minVelocity); // Make sure it is going at the minimum velocity to start
	}   // LaunchBall()

	/***
    *       OnCollisionEnter2D() will make the sound we program it for in the Editor when the
    *   ball collides with something.
    *		Note:  Uses veloctity tweaks from Udemy community posts:
    *		https://community.gamedev.tv/t/question-about-the-velocity-tweak-in-this-lecture/117674
    *		https://community.gamedev.tv/t/velocitytweak-fixed/125114
    *		https://community.gamedev.tv/t/autoplay-loses-the-game-and-ball-smashing-through-the-walls/83251
    ***/
	private void OnCollisionEnter2D()
	{
		if (ballLaunched)
		{
			Vector2 tweakVelocityOfBoth = new Vector2(Random.Range(-randomFactor, randomFactor),
				Random.Range(-randomFactor, randomFactor)); // Replaced 0.1f with -randomFactor here
			Vector2 tweakVelocityOfX = new Vector2(Random.Range(-randomFactor, randomFactor), 0);
			Vector2 tweakVelocityOfY= new Vector2(0, Random.Range(-randomFactor, randomFactor));

			myAudioSource.PlayOneShot(ballSounds[Random.Range(0, ballSounds.Length)]);
			if (myRigidBody2D.velocity.x >= 0 && myRigidBody2D.velocity.y >= 0)
			{
				myRigidBody2D.velocity += tweakVelocityOfBoth;
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

			if (myRigidBody2D.velocity.magnitude < minVelocity)
			{
				//Debug.Log("Raising Magnitude above minimum");
				ClampVelocity(minVelocity);
			}	// if
			else if (myRigidBody2D.velocity.magnitude > maxVelocity)
			{
				//Debug.Log("Clamping Magnitude to maximum");
				ClampVelocity(maxVelocity);
			}	// else
		}   // if
	}   // OnCollisionEnter2D()

	/***
	*       ClampVelocity() will set the velocity to the passed value.  If requested
	*   velocity change would be more than a value of 1 difference it is limited to
	*   only adding one to the current velocity.  That way the change is gradual and
	*   it will get ramped up further next time if it is still outside the desired
	*   range.  Without such a limit the ball can accelerate much too quickly
	*   sometimes and the player won't be able to react to it easily.
	***/
	private void ClampVelocity(float velocityLimit)
	{
		Debug.Log("Before magnitude = " + myRigidBody2D.velocity.magnitude +
			", x = " + myRigidBody2D.velocity.x + ", y = " + myRigidBody2D.velocity.y);
		if (Mathf.Abs(velocityLimit - myRigidBody2D.velocity.magnitude) > 1.0f)
		{   // Don't change it too much at one time
			if (velocityLimit > myRigidBody2D.velocity.magnitude)
				velocityLimit = myRigidBody2D.velocity.magnitude + 1.0f;    // Increase by 1.0f
			else
				velocityLimit = myRigidBody2D.velocity.magnitude - 1.0f;	// Decrease by 1.0f
		}   // if

		ChangeVelocity(velocityLimit / myRigidBody2D.velocity.magnitude);
		Debug.Log("After magnitude = " + myRigidBody2D.velocity.magnitude +
			", x = " + myRigidBody2D.velocity.x + ", y = " + myRigidBody2D.velocity.y);
	}   // ClampVelocity()

	/***
	*       ChangeVelocity() will make the ball velocity change by the multiple of the
	*   new value given.  The change is not exact and will tend to be a bit over or
	*   under the amount asked for, due to rounding, but it will be fairly close.
	*		It is suggested that you don't make large changes from the current velocity,
	*	because they will appear too abrupt and the player can't react to it easily.
	*		Examples:
	*		ChangeVelocity(0.9f) will slow the ball down by about 1/10th
	*		ChangeVelocity(1.1f) will speed the ball up by about 1/10th
	***/
	public void ChangeVelocity(float magnitudeChange)
	{
		myRigidBody2D.velocity *= magnitudeChange;
	}   // ChangeVelocity()
}   // class Ball