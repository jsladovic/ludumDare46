using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public CharacterController Controller;
	private const float Speed = 2.0f;
	private const float RotationSpeed = 15.0f;

	private const float PickUpRadius = 1.5f;

	public Transform CarryPosition;
	public Transform DropPosition;
	private CreatureController CarriedCreature;

	private ExtractionPoint ExtractionPoint;
	private bool ExtractionStarted;
	private float ExtractionTimeRemaining;
	private const float MaxExtractionTime = 1.0f;

	void Start()
	{

	}

	void Update()
	{
		if (ExtractionStarted == true)
		{
			ExtractionTimeRemaining -= Time.deltaTime;
			if (ExtractionTimeRemaining <= 0.0f)
			{
				Debug.Log("Player extracted");
				ExtractionStarted = false;
			}
		}

		Vector3 direction = Vector3.zero;
		if (Input.GetAxisRaw("Horizontal") != 0.0f)
		{
			direction.x = Input.GetAxisRaw("Horizontal");
		}
		if (Input.GetAxisRaw("Vertical") != 0.0f)
		{
			direction.z = Input.GetAxisRaw("Vertical");
		}
		if (direction != Vector3.zero)
		{
			direction.Normalize();

			Quaternion lookRotation = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);

			Controller.Move(direction * Speed * Time.deltaTime);
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			HandlePickUp();
		}
		if (Input.GetKeyUp(KeyCode.E))
		{
			if (ExtractionStarted == true)
				ExtractionStarted = false;
		}
	}

	private void HandlePickUp()
	{
		if (CarriedCreature == null)
		{
			if (ExtractionPoint != null)
			{
				ExtractionStarted = true;
				ExtractionTimeRemaining = MaxExtractionTime;
			}
			else
			{
				Collider[] colliders = Physics.OverlapSphere(transform.position, PickUpRadius);
				if (colliders.Any(c => c.tag == "Creature"))
				{
					CreatureController creature = colliders.First(c => c.tag == "Creature").GetComponentInParent<CreatureController>();
					CarriedCreature = creature ?? throw new UnityException("Found creature, but it doesn't have a controller");

					CarriedCreature.transform.SetParent(transform);
					CarriedCreature.transform.position = CarryPosition.position;
				}
			}
		}
		else
		{
			if (ExtractionPoint != null)
			{
				ExtractionPoint.ReleaseCreature(CarriedCreature);
			}
			else
			{
				CarriedCreature.transform.position = DropPosition.position;
			}

			CarriedCreature.transform.SetParent(transform.parent);
			CarriedCreature = null;
		}
	}

	public void EnteredExtractionPoint(ExtractionPoint point)
	{
		ExtractionPoint = point;
	}

	public void ExitedExtractionPoint()
	{
		ExtractionPoint = null;
	}
}
