using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    static float ballY = 0.7f;
    [SerializeField]
    Paddle  paddle1;            // Linked to Paddle in Unity editor
    Vector2 paddleToBallVector; // Distance to paddle and ball
    [SerializeField]
    bool ballLaunched = false;
    [SerializeField]
    float   xPush = 2f;
    [SerializeField]
    float   yPush = 15f;

/***
*       Start is only used to set the distance vector between the ball and the paddle.
***/
    void Start()
    {
        paddleToBallVector = transform.position - paddle1.transform.position;
    }   // Start()

/***
*       Update in this class will make the ball stick to the paddle using the distance
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
*       LockBallToPaddle will make the ball stick to the paddle using the distance
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
*       LaunchBallOnMouseClick will make the ball launch off the paddle and disable it
*   sticking to the paddle after that.
***/
    void LaunchBallOnMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
		{   // Left mouse button was pressed
            ballLaunched = true;
            GetComponent<Rigidbody2D>().velocity =  new Vector2(xPush, yPush);
		}   // if
    }   // LaunchBallOnMouseClick()
}   // class Ball