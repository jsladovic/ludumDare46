using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public static PlayerController Instance;

	public Animator Animator;

	public CharacterController Controller;
	private float Speed => CarriedCreature == null ? DefaultSpeed : CarryingSpeed;
	private float RotationSpeed => CarriedCreature == null ? DefaultRotationSpeed : CarryingRotationSpeed;

	private const float DefaultSpeed = 3.0f;
	private const float DefaultRotationSpeed = 15.0f;

	private const float CarryingSpeed = 2.75f;
	private const float CarryingRotationSpeed = 12.5f;

	private const float PickUpRadius = 1.5f;

	public Transform CarryPosition;
	public Transform DropPosition;
	private CreatureController CarriedCreature;

	private ExtractionPoint ExtractionPoint;
	private bool ExtractionStarted;
	private float ExtractionTimeRemaining;
	private const float MaxExtractionTime = 1.0f;

	public bool Defeated { get; private set; }

	private void Awake()
	{
		Instance = this;
	}

	void Update()
	{
		if (Defeated)
			return;

		if (ExtractionStarted == true)
		{
			ExtractionTimeRemaining -= Time.deltaTime;
			if (ExtractionTimeRemaining <= 0.0f)
			{
				Debug.Log("Player extracted");
				LevelManager.Instance.FinishLevel();
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

		Animator.SetBool(AnimatorParams.Moving, direction != Vector3.zero);

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

					GameManager.Instance.RaiseSecurityAwareness();
					CarriedCreature.transform.SetParent(transform);
					CarriedCreature.transform.position = CarryPosition.position;
					Animator.SetBool(AnimatorParams.Carrying, true);
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

			Animator.SetBool(AnimatorParams.Carrying, false);
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

	public void Defeat()
	{
		Defeated = true;
		Animator.SetTrigger(AnimatorParams.Defeat);
	}
}
