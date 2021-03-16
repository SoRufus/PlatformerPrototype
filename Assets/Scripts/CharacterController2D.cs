using UnityEngine;
using UnityEngine.Events;
using System;

public class CharacterController2D : Singleton<CharacterController2D>
{
	[SerializeField] private float jumpForce = 400f;                          // Amount of force added when the player jumps.
	[SerializeField] private float moveSpeed;
	[SerializeField] private float rotationAirSpeed;
	[SerializeField] private float rotationOfDeath;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	public Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	float moveValue;

    public UnityEvent OnLandEvent;

	private void Awake()
	{
		PlayerInputSystem.Instance.OnJumpAction += Jump;

		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null) OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		Move(PlayerInputSystem.Instance.moveValue);
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
		if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
		{
			CheckRotation();
		}
	}

	public void Move(float move)
	{
		moveValue = move;
		if (m_Grounded)
		{
			if (move != 0)
			{
				m_Rigidbody2D.freezeRotation = true;
				Vector3 targetVelocity = new Vector2(move * moveSpeed * Time.deltaTime, m_Rigidbody2D.velocity.y);
				transform.position = Vector2.Lerp(transform.position, transform.position + targetVelocity, m_MovementSmoothing);
			}

			if (move > 0 && !m_FacingRight)
			{
				Flip();
			}
			else if (move < 0 && m_FacingRight)
			{
				Flip();
			}
		}
		else if(m_AirControl)
        {
			m_Rigidbody2D.freezeRotation = false;
			Vector3 rotateVector =  new Vector3(0, 0, rotationAirSpeed * Time.deltaTime * moveValue);
			transform.localEulerAngles -= rotateVector;
        }
	}
	private void Jump()
    {
		if (m_Grounded)
		{
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(jumpForce * moveValue, jumpForce));
		}
	}

	private void Flip()
	{
		m_FacingRight = !m_FacingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	public void CheckRotation()
    {
		var angle = Mathf.Abs(transform.rotation.eulerAngles.z) % 360;
		if (angle > 180) angle = 360 - angle;
		if (angle < rotationOfDeath)
        {
			transform.localEulerAngles = Vector3.zero;
		}
		else
        {
			PlayerController.Instance.SetPlayerDeath();
		}
    }
}
