using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GliderScript : MonoBehaviour {

    BB8MoveScript moveScript;

    private void Start()
    {
        moveScript = GetComponent<BB8MoveScript>();
    }

    private void Update()
    {
        if (moveScript.onGround == false && Input.GetButtonDown("Jump"))
        {
            Debug.Log("Glide");
            //toggle the glider
            //slow down fall speed
        }
    }
}
