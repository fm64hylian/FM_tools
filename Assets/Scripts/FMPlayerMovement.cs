using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMPlayerMovement : MonoBehaviour
{
	public float movementSpeed;
	public float jumpingSpeed;
	public Vector3 gravity;

	private Transform cam;
	private Vector3 movementDirection;
	private Vector3 verticalDirection;
	private CharacterController characterController;
	//private Animator anim;

	// Estados
	private bool isMoving;
	private bool isJumping;
	private bool isFalling;
	private bool isDead;


	private AudioSource audioSource;

	private void Awake()
    {
		audioSource = GetComponent<AudioSource>();
		characterController = GetComponent<CharacterController>();
		//anim = GetComponent<Animator>();
	}
    void Start()
	{
		SetCamera(Camera.main.transform);
	}

	void Update()
	{
		if (isDead) { return; }
		// Calcula la dirección a moverse en base a inputs.
		if (cam != null)
		{
			Move();
		}

		isJumping = !characterController.isGrounded && verticalDirection.y >= 0f;
		isFalling = !characterController.isGrounded && verticalDirection.y < 0f;


		bool wasJumping = isJumping;

		if (!isJumping && !isFalling  && Input.GetKeyDown(KeyCode.Z))
		{
			// Calcula la dirección vertical a moverse en base a saltos y caidas.
			Jump();
		}

		bool didJump = !wasJumping && isJumping;

		if (isJumping || isFalling)
		{
			// Vector3 gravity contiene el signo negativo.
			verticalDirection += gravity * Time.deltaTime;

			//TODO add attack
			//if (Input.GetKey(KeyCode.X))
			//{

			//}

		}


		Animate();
	}

	private void FixedUpdate()
	{
		//if (dodgingTime <= 0f)
			DoMove();
		//else
		//{
		//	float value = 1 - dodgingTime / dodgeTime;
		//	float dashSpeed = dodgeStrength * (-2 * value + 2);
		//	DoDash(dashSpeed);

		//}
	}

	private void Jump()
	{
		audioSource.Play();
		isJumping = true;
		verticalDirection = jumpingSpeed * Vector3.up;
	}

	private void Move()
	{
		movementDirection = Input.GetAxisRaw("Vertical") * (cam.forward - cam.forward.y * Vector3.up).normalized;
		movementDirection += Input.GetAxisRaw("Horizontal") * cam.right;

		isMoving = movementDirection.magnitude > 0f;
	}

	private void DoMove()
	{
		characterController.Move(movementDirection.normalized * movementSpeed * Time.deltaTime + verticalDirection * Time.deltaTime);
	}

	//no se está usando?
	public void SetCamera(Transform _cam)
	{
		cam = _cam;
	}

	public bool IsMoving()
	{
		return isMoving;
	}

	float lastMovX=0f;
	float lastMovY=0f;
	public void Animate()
	{
		float movX = Mathf.Lerp(lastMovX, Input.GetAxisRaw("Horizontal"), 0.5f);
		float movY = Mathf.Lerp(lastMovY, Input.GetAxisRaw("Vertical"), 0.5f);
		lastMovX = movX;
		lastMovY = movY;

		//anim.SetFloat("movV", movY);
		//anim.SetFloat("movH", movX);
		//anim.SetBool("OnAir", isFalling || isJumping);
	}

	public void SetDeadState(bool _newState) {
		isDead = _newState;
	}
}