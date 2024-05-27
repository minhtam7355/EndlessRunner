using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Side { Left, Mid, Right }

public class Character : MonoBehaviour
{
	public Side CurrentSide = Side.Mid;
	private float _newXPosition = 0f;
	[HideInInspector]
	public bool SwipeLeft, SwipeRight, SwipeUp, SwipeDown;
	public float XValue;
	private CharacterController _characterController;
	private Animator _animator;
	private float _horizontalVelocity;
	public float DodgeSpeed;
	public float JumpPower = 7f;
	private float _verticalVelocity;
	public bool InJump, InRoll;
	private float _collisionHeight;
	private float _collisionCenterY;
	void Start()
	{
		_characterController = GetComponent<CharacterController>();
		_collisionHeight = _characterController.height;
		_collisionCenterY = _characterController.center.y;
		_animator = GetComponent<Animator>();
		transform.position = Vector3.zero;
	}

	// Update is called once per frame
	void Update()
	{
		SwipeLeft = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
		SwipeRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
		SwipeUp = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
		SwipeDown = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
		if (SwipeLeft && !InRoll)
		{
			if (CurrentSide == Side.Mid)
			{
				_newXPosition = -XValue;
				CurrentSide = Side.Left;
				_animator.Play("Dodge_Left");
			}
			else if (CurrentSide == Side.Right)
			{
				_newXPosition = 0;
				CurrentSide = Side.Mid;
				_animator.Play("Dodge_Left");
			}
		}
		else if (SwipeRight && !InRoll)
		{
			if (CurrentSide == Side.Mid)
			{
				_newXPosition = XValue;
				CurrentSide = Side.Right;
				_animator.Play("Dodge_Right");
			}
			else if (CurrentSide == Side.Left)
			{
				_newXPosition = 0;
				CurrentSide = Side.Mid;
				_animator.Play("Dodge_Right");
			}
		}
		Vector3 moveVector = new Vector3(_horizontalVelocity - transform.position.x, _verticalVelocity * Time.deltaTime, 0);
		_horizontalVelocity = Mathf.Lerp(_horizontalVelocity, _newXPosition, Time.deltaTime * DodgeSpeed);
		_characterController.Move(moveVector);
		Jump();
		Roll();
	}
	public void Jump()
	{
		if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
		{
			_animator.Play("Landing");
			InJump = false;
		}
		if (SwipeUp)
		{
			_verticalVelocity = JumpPower;
			_animator.CrossFadeInFixedTime("Jump", 0.1f);
			InJump = true;
		}
		else
		{
			_verticalVelocity -= JumpPower * 2 * Time.deltaTime;
			if (_characterController.velocity.y < -0.1f)
				_animator.Play("Falling");
		}
	}
	internal float _rollCounter;
	public void Roll()
	{
		_rollCounter -= Time.deltaTime;
		if (_rollCounter <= 0f)
		{
			_rollCounter = 0f;
			_characterController.center = new Vector3(0, _collisionCenterY, 0);
			_characterController.height = _collisionHeight;
			InRoll = false;
			_animator.speed = 1.0f; // Reset the animation speed to normal
		}
		if (SwipeDown)
		{
			_rollCounter = 0.2f;
			_verticalVelocity -= 10f;
			_characterController.center = new Vector3(0, _collisionCenterY / 2f, 0);
			_characterController.height = _collisionHeight / 2f;
			_animator.CrossFadeInFixedTime("Roll", 0.1f);
			_animator.speed = 5.5f; // Speed up the Roll animation
			InRoll = true;
			InJump = false;
		}
	}
}
