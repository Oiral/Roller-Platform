using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum pickupType {ropeLength, grapple, parachute, grappleWind};

public class PickupScript : MonoBehaviour {

    Vector3 startingPos;

    public pickupType typeOfPickup;

    private void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update () {
        Vector3 pos = startingPos;

        pos.y = startingPos.y + Mathf.Sin(Time.time);

        pos.y = Mathf.Clamp(pos.y, startingPos.y - 0.5f, startingPos.y + 0.5f);

        transform.position = pos;
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerBase")
        {
            UnlockScript unlocks = other.GetComponentInParent<UnlockScript>();
            Debug.Log("collision");
            if (unlocks == null)
            {
                return;
            }

            switch (typeOfPickup)
            {
                case pickupType.ropeLength:
                    //check if the player has a grapple
                        Debug.Log("Rope Length");
                        unlocks.gameObject.GetComponentInChildren<SwingScript>().maxLength += 1;
                        Destroy(gameObject);
                    break;
                case pickupType.grapple:
                    //check if the player has the rope
                    unlocks.grapple = true;
                    Destroy(gameObject);
                    break;
                case pickupType.parachute:
                    //check if the player doesnt have a glider script attached
                    unlocks.glider = true;
                    Destroy(gameObject);
                    break;
                case pickupType.grappleWind:
                    //Check if the player has the grapple
                    unlocks.reel = true;
                    Destroy(gameObject);
                    break;
                default:
                    break;
            }
            
        }
    }
}
