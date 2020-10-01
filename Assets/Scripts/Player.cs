﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	//privates for player control
	[SerializeField]
	private float moveForce = 1000f;
	private Rigidbody rb;
	[SerializeField]
	private float maxVelocity = 1f;
	[SerializeField]
	private float jumpForce = 10f;
	[SerializeField]
	private GameObject jumpCheck;
	[SerializeField]
	private bool canJump = false;
	[SerializeField]
	private float rotationSpeed = 5f;

	//layers
	private int groundLayer;
	private int SwapLayer;
	private int wallLayer;
	private int testLayer;

	//privates for swaps
	private GameObject objectInRange = null;
	private GameObject swapped = null;

	void Start() {
		rb = GetComponent<Rigidbody>(); //used later to make code more readable

		//layer dependancies
		groundLayer = LayerMask.NameToLayer("Ground");
		testLayer = LayerMask.NameToLayer("Test");
		SwapLayer = LayerMask.NameToLayer("Swappable");
		wallLayer = LayerMask.NameToLayer("Wall");

	}

	void FixedUpdate() {
		float horizontal =  Input.GetAxis("Horizontal"); //movment
		float vertical =  Input.GetAxis("Vertical"); //movment

		float jump = jumpForce * Input.GetAxis("Jump"); //jump
		float interact = Input.GetAxis("Interact"); //swap

		
		if (horizontal != 0f || vertical != 0f) {
			Rotate(horizontal, vertical);
			Move(horizontal, vertical);
		}

		//Jump
		bool grounded = Physics.Linecast(transform.position, jumpCheck.transform.position, (1 << groundLayer)); //checks if player is grounded
																									/*~ == one's compliment*/
																									/*1 << groundLayer == 1<<8*/
																									/*1<<8 == 11111111 11111111 11111110 11111111*/
		if (grounded && canJump) {
  			Debug.Log("why");
			rb.AddRelativeForce(Vector3.up * jump * Time.fixedDeltaTime);
		}
	}

	void LateUpdate() {
		LimitVelocity();
	}

	private void Rotate(float horizontal, float vertical) {
		// look towards the move direction
		Quaternion lookHere = new Quaternion();
		lookHere.SetLookRotation(new Vector3(horizontal, 0, vertical));

		// smooth rotation
		transform.rotation = Quaternion.Slerp(transform.rotation, lookHere, Time.deltaTime * rotationSpeed);
	}

	private void Move(float horizontal, float vertical) {
		float force = Mathf.Max(Mathf.Abs(horizontal), Mathf.Abs(vertical));
		rb.AddRelativeForce(Vector3.forward * force * moveForce * Time.fixedDeltaTime);
	}

	private void LimitVelocity() {
		if (rb.velocity.magnitude > maxVelocity) {
			rb.velocity = rb.velocity.normalized * maxVelocity;
		}
	}
}