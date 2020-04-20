using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureEffectController : MonoBehaviour
{
	public CreatureController Creature;
	public Ability Ability => Creature.Ability;

	private void OnTriggerEnter(Collider other)
	{
		if (Ability == Ability.Electric)
		{
			if (other.tag == "Camera")
			{
				CameraController camera = other.GetComponentInParent<CameraController>();
				camera.Disable();
			}
		}
		else if (Ability == Ability.Smelly)
		{
			if (other.tag == "Guard")
			{
				GuardController guard = other.GetComponentInParent<GuardController>();
				guard.Stun();
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (Ability == Ability.Electric)
		{
			if (other.tag == "Camera")
			{
				CameraController camera = other.GetComponentInParent<CameraController>();
				camera.Enable();
			}
		}
		else if (Ability == Ability.Smelly)
		{
			if (other.tag == "Guard")
			{
				GuardController guard = other.GetComponentInParent<GuardController>();
				guard.UnStun();
			}
		}
	}
}
