using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform LightHolder;
	public Light Light;
	public LayerMask ViewConeLayerMask;

	public float Offset = 45.0f;
	private const float RotationSpeed = 25.0f;
	private bool IncreasingRotation;
	private float TotalRotation;
	public float StartingRaycastOffset = -1.5f;
	public float EndingRaycastOffset = -0.4f;
	private float MiddleOffset => (StartingRaycastOffset + EndingRaycastOffset) / 2.0f;

	public float FieldOfView = 55.0f;
	public int RayCount = 15;
	private float AngleIncrease => FieldOfView / RayCount;
	public float ViewDistance = 10.0f;

	private bool DetectedSomething;
	private float DetectionTimer;
	private const float MaxDetectionTimer = 1.0f;

	void Start()
	{
		DetectionTimer = MaxDetectionTimer;
	}

	void Update()
	{
		if (DetectedSomething == false)
		{
			Light.transform.Rotate(Vector3.up, (IncreasingRotation ? 1.0f : -1.0f) * RotationSpeed * Time.deltaTime);
			TotalRotation += (IncreasingRotation ? 1.0f : -1.0f) * RotationSpeed * Time.deltaTime;
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

			if (Physics.Raycast(Light.transform.position, Utils.GetVectorFromAngle(angle, MiddleOffset), out hit, ViewDistance, ViewConeLayerMask))
			{
				detectedSomething = true;
			}
			Debug.DrawRay(Light.transform.position, Utils.GetVectorFromAngle(angle, MiddleOffset) * ViewDistance, Color.cyan);
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
				Debug.Log($"game over");
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

		Light.color = Color.Lerp(Utils.DetectedFlashlightColor, Utils.DefaultFlashlightColor, DetectionTimer);
	}
}
