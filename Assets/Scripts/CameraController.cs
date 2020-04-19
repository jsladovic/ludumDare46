using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Light Light;
	public LayerMask ViewConeLayerMask;
	public Transform Camera;

	private float Offset => GameManager.Instance.Alarmed ? AlarmedOffset : DefaultOffset;
	private float RotationSpeed => GameManager.Instance.Alarmed ? AlarmedRotationSpeed : DefaultRotationSpeed;

	public float DefaultOffset = 45.0f;
	public float DefaultRotationSpeed = 25.0f;

	public float AlarmedOffset = 60.0f;
	public float AlarmedRotationSpeed = 45.0f;

	private bool IncreasingRotation;
	private float TotalRotation;

	private const float StartingRaycastOffset = -1.5f;
	private const float MiddleUpperRaycastOffset = -0.9f;
	private const float MiddleLowerRaycastOffset = -0.6f;
	private const float EndingRaycastOffset = -0.42f;

	public float FieldOfView = 65.0f;
	public int RayCount = 15;
	private float AngleIncrease => FieldOfView / RayCount;
	public float ViewDistance = 10.0f;

	private bool DetectedSomething;
	private float DetectionTimer;
	private const float MaxDetectionTimer = 1.0f;

	private float DetectionTimerSpeed => GameManager.Instance.Alarmed ? AlarmedDefectionTimerSpeed : DefaultDetectionTimerSpeed;
	private const float DefaultDetectionTimerSpeed = 0.5f;
	private const float AlarmedDefectionTimerSpeed = 1.0f;
	private const float DetectionTimerRecoverySpeed = 0.5f;

	private bool Disabled;

	void Start()
	{
		DetectionTimer = MaxDetectionTimer;
	}

	void Update()
	{
		if (Disabled == true || PlayerController.Instance.Defeated)
			return;

		if (DetectedSomething == false)
		{
			float changeInRotation = (IncreasingRotation ? 1.0f : -1.0f) * RotationSpeed * Time.deltaTime;
			Light.transform.Rotate(Vector3.up, changeInRotation);
			Camera.Rotate(Vector3.back, changeInRotation);
			TotalRotation += changeInRotation;
			if (IncreasingRotation == true && TotalRotation >= Offset)
			{
				IncreasingRotation = false;
			}
			else if (IncreasingRotation == false && TotalRotation <= -1.0f * Offset)
			{
				IncreasingRotation = true;
			}
		}

		HandleRaycasts();
	}

	private void HandleRaycasts()
	{
		float angle = Utils.GetAngleFromVector(Light.transform.forward) + FieldOfView / 2.0f;
		if (angle > 360.0f)
			angle -= 360.0f;

		bool detectedSomething = false;
		for (int i = 0; i < RayCount; i++)
		{
			if (Physics.Raycast(Light.transform.position, Utils.GetVectorFromAngle(angle, StartingRaycastOffset), out RaycastHit hit, ViewDistance, ViewConeLayerMask))
			{
				detectedSomething = true;
			}
			Debug.DrawRay(Light.transform.position, Utils.GetVectorFromAngle(angle, StartingRaycastOffset) * ViewDistance, Color.cyan);

			if (Physics.Raycast(Light.transform.position, Utils.GetVectorFromAngle(angle, EndingRaycastOffset), out hit, ViewDistance, ViewConeLayerMask))
			{
				detectedSomething = true;
			}
			Debug.DrawRay(Light.transform.position, Utils.GetVectorFromAngle(angle, EndingRaycastOffset) * ViewDistance, Color.cyan);

			if (Physics.Raycast(Light.transform.position, Utils.GetVectorFromAngle(angle, MiddleUpperRaycastOffset), out hit, ViewDistance, ViewConeLayerMask))
			{
				detectedSomething = true;
			}
			Debug.DrawRay(Light.transform.position, Utils.GetVectorFromAngle(angle, MiddleUpperRaycastOffset) * ViewDistance, Color.cyan);

			if (Physics.Raycast(Light.transform.position, Utils.GetVectorFromAngle(angle, MiddleLowerRaycastOffset), out hit, ViewDistance, ViewConeLayerMask))
			{
				detectedSomething = true;
			}
			Debug.DrawRay(Light.transform.position, Utils.GetVectorFromAngle(angle, MiddleLowerRaycastOffset) * ViewDistance, Color.cyan);
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
			DetectionTimer -= Time.deltaTime * DetectionTimerSpeed;
			if (DetectionTimer <= 0.0f)
			{
				DetectionTimer = 0.0f;
				PlayerController.Instance.Defeat();
			}
		}
		else if (DetectedSomething == false && detected == true)
		{
			DetectedSomething = true;
		}
		else if (DetectionTimer < MaxDetectionTimer)
		{
			DetectionTimer += Time.deltaTime * DetectionTimerRecoverySpeed;
			if (DetectionTimer > MaxDetectionTimer)
				DetectionTimer = MaxDetectionTimer;
		}

		Light.color = Color.Lerp(Utils.DetectedFlashlightColor, Utils.DefaultFlashlightColor, DetectionTimer);
	}

	public void Disable()
	{
		Disabled = true;
		DetectedSomething = false;
		DetectionTimer = MaxDetectionTimer;
		Light.enabled = false;
	}

	public void Enable()
	{
		Disabled = false;
		Light.enabled = true;
	}
}
