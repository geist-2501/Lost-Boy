using UnityEngine;


public abstract class MotorBaseClass : MonoBehaviour
{
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
    public virtual void Move(Vector3 _velocity)
    {
        if (canMove)
            velocity = _velocity;
    }

    //Gets a y-axis rotational vector from controller.
    public virtual void Rotate(Vector3 _rotation)
    {
        if (canMove)
            rotation = _rotation;
    }

    //Gets a jump vector from controller.
    public virtual void Jump(float _jumpForce)
    {
        if (canMove)
            jumpForce = Vector3.up * _jumpForce;
    }

    //Gets a x-axis roational vector form controller.
    public virtual void RotateCamera(Vector3 _cameraRotaion)
    {
        if (canMove)
            cameraRotaion = _cameraRotaion;
    }

	//Gets an interact signal from controller.
    public virtual void Interact()
    {
        isInteracting = true;
    }


    private void FixedUpdate()
    {
        if (canMove)
        {
            PerformMovement();
            PerformRotation();
            PerformInteractions();
        }
    }


	//These classes are abstract, and each child that inherits from this class
	//must implement them.

    public abstract void PerformMovement();

    public abstract void PerformRotation();

    public abstract void PerformInteractions();
}
