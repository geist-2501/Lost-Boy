﻿using UnityEngine;


public abstract class MotorBaseClass : MonoBehaviour {

	//TODO change to properties
	public bool canMove = true;
	public Vector3 velocity = Vector3.zero;
	public Vector3 rotation = Vector3.zero;
	public Vector3 cameraRotaion = Vector3.zero;
	public bool canJump = true;
	public Vector3 jumpForce;
	public bool isInteracting = false;

	public Rigidbody rb;
	public CapsuleCollider col;




	//Gets a movement vector from controller.
	public virtual void Move(Vector3 _velocity) {
		velocity = _velocity;
	}

	//Gets a y-axis rotational vector from controller.
	public virtual void Rotate(Vector3 _rotation) {
		rotation = _rotation;
	}

	//Gets a jump vector from controller.
	public virtual void Jump(float _jumpForce) {
		jumpForce = Vector3.up * _jumpForce;
	}

	//Gets a x-axis roational vector form controller.
	public virtual void RotateCamera(Vector3 _cameraRotaion) {
		cameraRotaion = _cameraRotaion;
	}

	public virtual void Interact() {
		isInteracting = true;
	}


	private void FixedUpdate() {
		if (canMove) {
			PerformMovement();
			PerformRotation();
			PerformInteractions();
		}
	}



	public abstract void PerformMovement();

	public abstract void PerformRotation();

	public abstract void PerformInteractions();
}
