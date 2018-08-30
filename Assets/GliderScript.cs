using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GliderScript : MonoBehaviour {

    BB8MoveScript moveScript;

    public float glideDrag = 0.1f;

    bool gliderActive;

    Rigidbody rb;

    public GameObject gliderPrefab;
    GameObject spawnedGlider;

    UnlockScript unlocks;

    private void Start()
    {
        moveScript = GetComponent<BB8MoveScript>();
        rb = moveScript.bb8Base.GetComponent<Rigidbody>();
        unlocks = GetComponentInParent<UnlockScript>();
    }

    private void Update()
    {
        //Toggle the glider if the player is in the air and they jump
        //This means they can enable it and also disable it
        if (moveScript.onGround == false && Input.GetButtonDown("Jump") && unlocks.glider)
        {
            Debug.Log("Glide");
            //toggle the glider
            ToggleGlider(!gliderActive);
        }


        if (gliderActive)
        {//first check if we are on the ground
            if (moveScript.onGround)
            {
                //toggle the gloder
                ToggleGlider(false);
            }
            //Slow down the fall speed
            Vector3 vel = rb.velocity;

            if (vel.y < 0)//only glide if the player is going down
            {
                //Debug.Log("Going down");
                //Reduce y component of velocity
                vel.y *= 1.0f - glideDrag;
            }

            rb.velocity = vel;
        }


    }

    void ToggleGlider(bool toggle)
    {
        gliderActive = toggle;
        //toggle visuals for glider

        //check if there is a glider prefab
        if (gliderPrefab != null)
        {
            if (toggle)//if were turning the glider on
            {
                spawnedGlider = Instantiate(gliderPrefab, transform.position, Quaternion.LookRotation(transform.forward,Vector3.up) , transform);


            }
            else
            {
                Destroy(spawnedGlider);
            }


        }
    }
}
