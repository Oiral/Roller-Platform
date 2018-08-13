using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour {

    Vector3 startingPos;

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
            Debug.Log("Points woo");
            GameObject.FindGameObjectWithTag("Player").GetComponent<SwingScript>().maxLength += 1;
            Destroy(gameObject);
        }
    }
}
