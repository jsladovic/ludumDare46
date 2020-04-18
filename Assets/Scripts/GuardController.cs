using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
	public CharacterController Controller;
	public Transform PathParent;
	private Transform[] Path;

	private const float Speed = 2.5f;
	public LayerMask ViewConeLayerMask;

	public Light Flashlight;
	private Color DefaultFlashlightColor = Color.white;
	private Color DetectedFlashlightColor = Color.red;

	public float FieldOfView = 55.0f;
	public int RayCount = 50;
	private float AngleIncrease => FieldOfView / RayCount;
	public float ViewDistance = 8.0f;

	private int CurrentPathNode;
	private Transform CurrentPathTransform => Path[CurrentPathNode];

	private bool DetectedSomething;
	private float DetectionTimer;
	private const float MaxDetectionTimer = 1.0f;

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
		if (Path != null && DetectedSomething == false)
		{
			Vector3 direction = CurrentPathTransform.position - transform.position;
			direction.y = 0.0f;
			direction.Normalize();

			Controller.Move(direction * Speed * Time.deltaTime);
			transform.rotation = Quaternion.LookRotation(direction);
		}

		float angle = GetAngleFromVector(transform.forward) + FieldOfView / 2.0f;
		if (angle > 360.0f)
			angle -= 360.0f;

		bool detectedSomething = false;
		for (int i = 0; i < RayCount; i++)
		{
			if (Physics.Raycast(transform.position, GetVectorFromAngle(angle), out RaycastHit hit, ViewDistance, ViewConeLayerMask))
			{
				detectedSomething = true;
			}
			Debug.DrawRay(transform.position, GetVectorFromAngle(angle) * ViewDistance, Color.cyan);
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

		Flashlight.color = Color.Lerp(DetectedFlashlightColor, DefaultFlashlightColor, DetectionTimer);
	}

	private static Vector3 GetVectorFromAngle(float angle)
	{
		float angleRad = angle * Mathf.Deg2Rad;
		return new Vector3(Mathf.Cos(angleRad), 0.0f, Mathf.Sin(angleRad)).normalized;
	}

	private static float GetAngleFromVector(Vector3 vector)
	{
		vector.y = 0.0f;
		vector = vector.normalized;
		float n = Mathf.Atan2(vector.z, vector.x) * Mathf.Rad2Deg;
		if (n < 0.0f)
			n += 360.0f;
		return n;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform == CurrentPathTransform)
		{
			CurrentPathNode = (CurrentPathNode + 1) % Path.Length;
		}
	}
}
