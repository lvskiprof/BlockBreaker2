using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    static float ballY = 0.7f;
    [SerializeField]
    float  paddle1;        // Linked to Paddle in Unity editor

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    static float    paddleWidthInUnits = 2f;
    [SerializeField]
    static float   screenWidthInUnits = 16f;            // Camera size of 6 in a 3:4 ratio screen
    [SerializeField]
    static float   minX = paddleWidthInUnits / 2;       // Paddle is 2 units wide, so width / 2
    [SerializeField]
    static float   maxX = screenWidthInUnits - minX;    // Since minX = paddleWidthInUnits / 2, simply subtract that from screenWidthInUnits

    // Start is called before the first frame update
    void Start()
    {

    }   // Start()

    // Update is called once per frame
    void Update()
    {
        float   mousePosInUnits = Input.mousePosition.x / Screen.width * screenWidthInUnits;
        Vector2 paddlePos = new Vector2(transform.position.x, transform.position.y);
        paddlePos.x = Mathf.Clamp(mousePosInUnits, minX, maxX);
        transform.position = paddlePos;
    }   // Update()
}   // class Paddle