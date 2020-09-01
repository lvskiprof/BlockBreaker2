using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    static float   paddleWidthInUnits = 2f;
    static float   screenWidthInUnits = 16f;     // Camera size of 6 in a 3:4 ratio screen
    [SerializeField]
    float   minX = paddleWidthInUnits / 2;       // Paddle is 2 units wide, so width / 2
    [SerializeField]
    float   maxX = screenWidthInUnits - (paddleWidthInUnits / 2);   // subtract that from screenWidthInUnits

    /***
	*		Cached component references.
	***/
    GameStatus  gameStatus;
    Ball        ball;

    // Start is called before the first frame update
    void Start()
    {
        gameStatus = FindObjectOfType<GameStatus>();
        ball = FindObjectOfType<Ball>();
    }   // Start()

    // Update is called once per frame
    void Update()
    {
        Vector2 paddlePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            transform.position.y);

        paddlePos.x = Mathf.Clamp(GetXPos(), minX, maxX);
        transform.position = paddlePos;
    }   // Update()

    private float GetXPos()
	{
        if (gameStatus.IsAutoPlayEnabled())
        {   // Make sure the paddle is always where the ball is in auto play mode
            return ball.transform.position.x;
        }   // if
        else
        {   // Default to following the cursor if not in Automatic mode
            return Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        }   // else
    }   // GetXPos()
}   // class Paddle