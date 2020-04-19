using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
	public CharacterController Controller;
	public Transform PathParent;
	private Transform[] Path;

	public float Speed => GameManager.Instance.Alarmed ? AlarmedSpeed : DefaultSpeed;
	public float RotationSpeed => GameManager.Instance.Alarmed ? AlarmedRotationSpeed : DefaultRotationSpeed;

	public float DefaultSpeed = 2.5f;
	public float DefaultRotationSpeed = 10.0f;

	public float AlarmedSpeed = 5.0f;
	public float AlarmedRotationSpeed = 15.0f;

	private const float RotationDeadZone = 5.0f;
	public LayerMask ViewConeLayerMask;

	public Light Flashlight;

	public float FieldOfView = 55.0f;
	public int RayCount = 25;
	private float AngleIncrease => FieldOfView / RayCount;
	public float ViewDistance = 8.0f;

	private int CurrentPathNode;
	private Transform CurrentPathTransform => Path[CurrentPathNode];

	private bool DetectedSomething;
	private float DetectionTimer;
	private const float MaxDetectionTimer = 1.0f;

	private bool Stunned;

	void Start()
	{
		if (PathParent != null)
		{
			Path = new Transform[PathParent.childCount];
			for (int i = 0; i < PathParent.childCount; i++)
			{
				Path[i] = PathParent.GetChild(i);
			}
		}

		CurrentPathNode = 0;
		DetectionTimer = MaxDetectionTimer;
	}

	void Update()
	{
		if (Stunned)
			return;

		if (Path != null && DetectedSomething == false)
		{
			Vector3 direction = CurrentPathTransform.position - transform.position;
			direction.y = 0.0f;
			direction.Normalize();

			Quaternion lookRotation = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);

			if (Quaternion.Angle(transform.rotation, lookRotation) < RotationDeadZone)
				Controller.Move(direction * Speed * Time.deltaTime);
		}

		HandleRaycasts();
	}

	private void HandleRaycasts()
	{
		float angle = Utils.GetAngleFromVector(transform.forward) + FieldOfView / 2.0f;
		if (angle > 360.0f)
			angle -= 360.0f;

		bool detectedSomething = false;
		Vector3 originUpper = transform.position;
		Vector3 originLower = transform.position;
		originUpper.y += 0.35f;
		originLower.y -= 0.35f;
		for (int i = 0; i < RayCount; i++)
		{
			if (Physics.Raycast(originUpper, Utils.GetVectorFromAngle(angle), out RaycastHit hit, ViewDistance, ViewConeLayerMask))
			{
				detectedSomething = true;
			}
			Debug.DrawRay(originUpper, Utils.GetVectorFromAngle(angle) * ViewDistance, Color.cyan);

			if (Physics.Raycast(originLower, Utils.GetVectorFromAngle(angle), out hit, ViewDistance, ViewConeLayerMask))
			{
				detectedSomething = true;
			}
			Debug.DrawRay(originLower, Utils.GetVectorFromAngle(angle) * ViewDistance, Color.cyan);
			angle -= AngleIncrease;
		}

		HandleDetection(detectedSomething);
	}

	private void HandleDetection(bool detected)
	{
		if (DetectedSomething == true && detected == false)
		{
			// back to normal
			DetectedSomething = false;
		}
		else if (DetectedSomething == true && detected == true)
		{
			DetectionTimer -= Time.deltaTime;
			if (DetectionTimer <= 0.0f)
			{
				DetectionTimer = 0.0f;
				// Game over
				//Debug.Log($"game over");
			}
		}
		else if (DetectedSomething == false && detected == true)
		{
			DetectedSomething = true;
		}
		else if (DetectionTimer < MaxDetectionTimer)
		{
			DetectionTimer += Time.deltaTime;
			if (DetectionTimer > MaxDetectionTimer)
				DetectionTimer = MaxDetectionTimer;
		}

		Flashlight.color = Color.Lerp(Utils.DetectedFlashlightColor, Utils.DefaultFlashlightColor, DetectionTimer);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform == CurrentPathTransform)
		{
			CurrentPathNode = (CurrentPathNode + 1) % Path.Length;
		}
	}

	public void Stun()
	{
		Stunned = true;
		DetectedSomething = false;
		DetectionTimer = MaxDetectionTimer;
		Flashlight.enabled = false;
	}

	public void UnStun()
	{
		Stunned = false;
		Flashlight.enabled = true;
	}
}
