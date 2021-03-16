using UnityEngine;
using UnityEngine.Events;
using System;

public class CharacterController2D : Singleton<CharacterController2D>
{
	[SerializeField] private float jumpForce = 400f;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float rotationAirSpeed;
	[SerializeField] private float rotationOfDeath;
	[SerializeField] private float moveSmooth = .05f;
	[SerializeField] private LayerMask groundLayer;
	float groundDistance = 0.1f;
	private bool isGrounded;
	private Rigidbody2D rigid;
	private bool facingRight = true;
	float moveValue;
	Collider2D collider;
	private void Awake()
	{
		PlayerInputSystem.Instance.OnJumpAction += Jump;

		collider = GetComponent<Collider2D>();
		rigid = GetComponent<Rigidbody2D>();

	}

	private void FixedUpdate()
	{
		Move(PlayerInputSystem.Instance.moveValue);
	}
	public void Move(float move)
	{
		bool wasGround = isGrounded;
		moveValue = move;

		isGrounded = CheckGround();
		if (!wasGround && isGrounded) CheckLandingRotation();

		if (isGrounded)
		{
			if (move != 0)
			{
				rigid.freezeRotation = true;
				Vector3 targetVelocity = new Vector2(move * moveSpeed * Time.deltaTime, rigid.velocity.y);
				transform.position = Vector2.Lerp(transform.position, transform.position + targetVelocity, moveSmooth);
			}

			if (move > 0 && !facingRight)
			{
				Flip();
			}
			else if (move < 0 && facingRight)
			{
				Flip();
			}
		}
		else
		{
			rigid.freezeRotation = false;
			Vector3 rotateVector = new Vector3(0, 0, rotationAirSpeed * Time.deltaTime * moveValue);
			transform.localEulerAngles -= rotateVector;
		}
	}
	private bool CheckGround()
	{
		if (Physics2D.Raycast(collider.bounds.center, Vector2.down, collider.bounds.extents.y + groundDistance, groundLayer).collider) return true;
		return false;
	}
	private void Jump()
    {
		if (!isGrounded) return;
			rigid.AddForce(new Vector2(jumpForce * moveValue/2, jumpForce));
	}
	public void CheckLandingRotation()
    {
		rigid.velocity = Vector3.zero;
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
	private void Flip()
	{
		facingRight = !facingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
