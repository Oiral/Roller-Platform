using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BB8MoveScript : MonoBehaviour {

    Rigidbody rb;
    public GameObject bb8Base;

    public float moveSpeed = 2;
    public float vertRotSpeed = 2;
    public float horiRotSpeed = 1;

    public float jumpForce = 1;

    public float inAirSpeedMultiplier;

    bool onGround;

    private void Start()
    {
        rb = bb8Base.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    float verticalInput;
    float horizontalInput;

    float mouseX;
    float mouseY;

    // Update is called once per frame
    void Update () {
        CheckIfGrounded();

        if (onGround)
        {
            verticalInput = Input.GetAxisRaw("Vertical");
            horizontalInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            verticalInput = Input.GetAxisRaw("Vertical") * inAirSpeedMultiplier;
            horizontalInput = Input.GetAxisRaw("Horizontal") * inAirSpeedMultiplier;
        }
        
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
        if (Input.GetButtonDown("Jump") & onGround)
        {
            Jump();
        }
        
    }

    private void FixedUpdate()
    {
        Move();
        Look();
    }

    void Move()
    {
        //add force relative to direction of look
        verticalInput = verticalInput * moveSpeed;
        horizontalInput = horizontalInput * moveSpeed;

        Vector3 forwardPush = transform.forward * verticalInput;
        Vector3 sidewaysPush = transform.right * horizontalInput;

        rb.AddForce(forwardPush + sidewaysPush, ForceMode.Acceleration);
    }

    void Look()
    {
        //rotate head around body


        transform.Rotate(new Vector3(-mouseY * vertRotSpeed, 0));
        transform.Rotate(Vector3.up, mouseX * horiRotSpeed, Space.World);
        //transform.Rotate(transform.right, -mouseY);
    }

    void CheckIfGrounded()
    {
        //Test to see if there is a hit using a BoxCast
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, new Vector3(0.2f, 0.2f, 0.2f), -Vector3.up, out hit, Quaternion.identity, 0.5f))
        {
            //Output the name of the Collider your Box hit
            //Debug.Log("Hit : " + hit.collider.name);
            onGround = true;
        }
        else
        {
            onGround = false;
        }
        
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }
}
