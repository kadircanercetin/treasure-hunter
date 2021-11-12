using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpSpeed;

    public Rigidbody rigidBody;

    public Vector2 moveValue;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        //Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {      
       //Debug.Log("Update");
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);
        rigidBody.velocity = new Vector3(movement.x * moveSpeed, rigidBody.velocity.y, movement.z * moveSpeed);  
    }

    void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
        //Debug.Log("OnMove");

    }
 
    void OnJump(InputValue value)
    { 
        rigidBody.velocity = new Vector3(rigidBody.velocity.x , jumpSpeed, rigidBody.velocity.z );        
    }
}
