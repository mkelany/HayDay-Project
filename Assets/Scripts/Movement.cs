﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof (CharacterController))] 
[AddComponentMenu("Third Person Player/Third Person Controller")]
public class Movement : MonoBehaviour {
	public float rotationDamping = 20f;
	public float Speed = 10f;
	public int gravity = 20;
	public float jumpSpeed = 8;
	
	bool canJump;
	float moveSpeed;
	float verticalVel;  // Used for continuing momentum while in air    
	CharacterController controller;
	
	void Start()
	{
		controller = (CharacterController)GetComponent(typeof(CharacterController));
	}
	float UpdateMovement()
	{
		// Movement
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");
		
		Vector3 inputVec = new Vector3(x, 0, z);
		inputVec *= Speed;
		
		controller.Move((inputVec + Vector3.up * -gravity + new Vector3(0, verticalVel, 0)) * Time.deltaTime);
		
		// Rotation
		if (inputVec != Vector3.zero)
			transform.rotation = Quaternion.Slerp(transform.rotation, 
			                                      Quaternion.LookRotation(inputVec), 
			                                      Time.deltaTime * rotationDamping);
		
		return inputVec.magnitude;
	}
	void Update()
	{
		// Check for jump
		if (controller.isGrounded )
		{
			canJump = true;
			if ( canJump && Input.GetKeyDown("space") )
			{
				// Apply the current movement to launch velocity
				verticalVel = jumpSpeed;
				canJump = false;
			}
		}
		else
		{           
			// Apply gravity to our velocity to diminish it over time
			verticalVel += Physics.gravity.y * Time.deltaTime;
		}
		
		// Actually move the character
		moveSpeed = UpdateMovement();

		Debug.Log ("Move Speed: " + moveSpeed.ToString());
		
		if ( controller.isGrounded )
			verticalVel = 0f;// Remove any persistent velocity after landing
	}
}