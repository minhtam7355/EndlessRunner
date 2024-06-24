using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Side { Left, Mid, Right }

public class Character : MonoBehaviour
{
	public Side CurrentSide = Side.Mid;
	private float _newXPosition = -1f;
	[HideInInspector]
	public bool SwipeLeft, SwipeRight, SwipeUp, SwipeDown;
	public float XValue = 1;
	private CharacterController _characterController;
	private Animator _animator;
	private float _horizontalVelocity;
	public float DodgeSpeed;
	public float JumpPower = 7f;
	private float _verticalVelocity;
	public bool InJump, InRoll;
	private float _collisionHeight;
	private float _collisionCenterY;
	public AudioManager AudioManager;
	public GameObject bulletPrefab;
	public float bulletSpeed = 10;
	public float bulletSpawnOffset = 1.0f; // Distance in front of the character to spawn the bullet
	public float bulletSpawnHeightOffset = 0.5f; // Height offset for bullet spawn position

	void Awake()
	{
		AudioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
	}

	void Start()
	{
		_characterController = GetComponent<CharacterController>();
		_collisionHeight = _characterController.height;
		_collisionCenterY = _characterController.center.y;
		_animator = GetComponent<Animator>();
		transform.position = new Vector3(-1f, 0, 0);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			// Calculate the spawn position in front of the character
			Vector3 bulletSpawnPosition = transform.position + transform.forward * bulletSpawnOffset + Vector3.up * bulletSpawnHeightOffset;
			// Instantiate the bullet at the calculated position and character's rotation
			var bullet = Instantiate(bulletPrefab, bulletSpawnPosition, transform.rotation);
			bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
		}
		SwipeLeft = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
		SwipeRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
		SwipeUp = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
		SwipeDown = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
		if (SwipeLeft && !InRoll)
		{
			if (CurrentSide == Side.Mid)
			{
				_newXPosition = -6.5f;
				CurrentSide = Side.Left;
				AudioManager.PlaySFX(AudioManager.Dodge);
				_animator.Play("Dodge_Left");
			}
			else if (CurrentSide == Side.Right)
			{
				_newXPosition = -1f;
				CurrentSide = Side.Mid;
				AudioManager.PlaySFX(AudioManager.Dodge);
				_animator.Play("Dodge_Left");
			}
		}
		else if (SwipeRight && !InRoll)
		{
			if (CurrentSide == Side.Mid)
			{
				_newXPosition = 4.5f;
				CurrentSide = Side.Right;
				AudioManager.PlaySFX(AudioManager.Dodge);
				_animator.Play("Dodge_Right");
			}
			else if (CurrentSide == Side.Left)
			{
				_newXPosition = -1f;
				CurrentSide = Side.Mid;
				AudioManager.PlaySFX(AudioManager.Dodge);
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
		// Check if currently in the Falling animation and transition to Landing
		if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
		{
			_animator.Play("Landing");
			InJump = false;
		}

		// Allow jump only when grounded and SwipeUp is pressed
		if (_characterController.isGrounded && SwipeUp)
		{
			_verticalVelocity = JumpPower;
			AudioManager.PlaySFX(AudioManager.Jump);
			_animator.CrossFadeInFixedTime("Jump", 0.1f);
			InJump = true;
		}
		else
		{
			// Apply gravity when not jumping or already in the air
			_verticalVelocity -= JumpPower * 2 * Time.deltaTime;

			// Check if character is falling and play the Falling animation
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
			_rollCounter = 0.8f;
			_verticalVelocity -= 10f;
			_characterController.center = new Vector3(0, _collisionCenterY / 2f, 0);
			_characterController.height = _collisionHeight / 2f;
			AudioManager.PlaySFX(AudioManager.Dodge);
			_animator.CrossFadeInFixedTime("Roll", 0.6f);
			_animator.speed = 2f; // Speed up the Roll animation
			InRoll = true;
			InJump = false;
		}
	}
}
