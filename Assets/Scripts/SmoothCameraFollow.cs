using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
	private Vector3 _offset;
	[SerializeField]
	private float _smoothTime;
	private Vector3 _currentVelocity = Vector3.zero;

	[SerializeField]
	private float _rollOffsetY; // Adjust this value to your preference
	[SerializeField]
	private float _deathBumpDistance; // Distance to bump the camera back on character's death, on the Z-axis
	[SerializeField]
	private Vector3 _fixedOffset = new Vector3(0, 5, -10); // Fixed distance offset from the player

	private bool _isRolling;
	private Character _playerController;
	private DeathTrigger _deathTrigger;

	private void Start()
	{
		_playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
		_deathTrigger = GameObject.FindGameObjectWithTag("Player").GetComponent<DeathTrigger>();
	}

	private void LateUpdate()
	{
		if (_deathTrigger.IsDead)
		{
			Vector3 deathBumpPosition = transform.position;
			deathBumpPosition.z -= _deathBumpDistance;
			transform.position = Vector3.SmoothDamp(transform.position, deathBumpPosition, ref _currentVelocity, _smoothTime);
			return;
		}

		_isRolling = _playerController.InRoll;
		Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
		Vector3 targetPosition = playerPosition + _fixedOffset;

		if (_isRolling)
		{
			targetPosition.y += _rollOffsetY;
		}

		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, _smoothTime);
	}
}
