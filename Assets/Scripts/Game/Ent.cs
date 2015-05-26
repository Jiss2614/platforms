﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Ent : MonoBehaviour {

	public Controller2D controller;
	
	public float jumpHeight = 0.8f;
	public float timeToJumpApex = 0.3f;
	public float accelerationTimeAirborne = .2f;
	public float accelerationTimeGrounded = .1f;
	public float moveSpeed = 1.0f;
	public float runSpeed = 3.0f;
	
	protected Vector2 input;
	protected float speed = 1.0f;
	protected float gravity;
	protected float jumpVelocity;
	protected Vector2 velocity;
	protected float velocityXSmoothing;
	protected float velocityYSmoothing;

	protected bool jumping = false;
	protected bool jumpingDown = false;


	public virtual void Awake () {
		controller = GetComponent<Controller2D>();

		gravity = -((2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2));
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
	}


	public virtual void Update () {
		Reset();
		SetInput();
		SetSpeed();
		SetMove();
	}


	protected void Reset () {
		if (controller.collisions.above || controller.collisions.below) { //} || IsOnLadder()) {
			if (jumping) { velocity.x = 0; }
			velocity.y = 0;
			jumping = false;
			jumpingDown = false;
		}

		if (velocity.y < -2f) { 
			jumpingDown = false; 
		}
	}

	protected virtual void SetInput () {
		input = Vector2.zero;
	}


	protected virtual void SetSpeed () {
		speed = moveSpeed;
	}


	protected void SetMove () {
		// set velocity x
		float targetVelocityX = input.x * speed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);

		// set velocity y (apply gravity)
		velocity.y += gravity * Time.deltaTime;

		// set 2d controller move
		controller.Move (velocity * Time.deltaTime, jumpingDown);
	}


	protected void SetJump (bool isJumpingDown, float intensity = 1) {
		if (jumping) { return; }

		velocity.y = jumpVelocity * intensity; 

		jumping = true;
		if (isJumpingDown) {
			jumpingDown = true; 
			velocity.y *= 0.5f;
		}
	}
}
