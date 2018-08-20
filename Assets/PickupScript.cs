using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum pickupType {ropeLength, grapple, parachute, grappleWind};

public class PickupScript : MonoBehaviour {

    Vector3 startingPos;

    public pickupType typeOfPickup;

    public Sprite[] reticules;


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
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                return;
            }

            switch (typeOfPickup)
            {
                case pickupType.ropeLength:
                    //check if the player has a grapple
                    if (player.GetComponent<SwingScript>() != null)
                    {
                        Debug.Log("Rope Length");
                        player.GetComponent<SwingScript>().maxLength += 1;
                        Destroy(gameObject);
                    }
                    break;
                case pickupType.grapple:
                    //check if the player has the rope
                    if (player.GetComponent<SwingScript>() == null)
                    {
                        //add a grapple to the player
                        SwingScript swing = player.AddComponent<SwingScript>();
                        swing.hitMarkers = reticules;
                    }
                    break;
                case pickupType.parachute:
                    //check if the player doesnt have a glider script attached
                    if (player.GetComponent<GliderScript>() == null)
                    {
                        player.AddComponent<GliderScript>();
                    }
                    break;
                case pickupType.grappleWind:
                    //Check if the player has the grapple
                    if (player.GetComponent<SwingScript>() != null)
                    {
                        player.GetComponent<SwingScript>().retractSpeed = 1;
                    }
                    break;
                default:
                    break;
            }
            
        }
    }
}
