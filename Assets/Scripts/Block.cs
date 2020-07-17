using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Block : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
	{
        Destroy(gameObject);
        //Debug.Log(collision.gameObject.name);   // Log what collided with the block
    }   // OnCollisionEnter2D()
    /***
    *       Start is not used yet.
    ***/
    void Start()
    {

    }   // Start()

    /***
    *       Update is not used yet.
    ***/
    void Update()
    {

    }   // Update()
}   // class Block