using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StuntTrackerScript : MonoBehaviour {

    public Rigidbody rb;

    public float score;
    public Text scoreText;

    public float minSpeed;
    private void Update()
    {
        if (rb.velocity.magnitude > minSpeed)
        {                                                                     
            score += 1;
        }
        else
        {
            score = 0;
        }

        if (score > 0)
        {
            scoreText.text = score.ToString();
        }
        else scoreText.text = "";

    }
}
