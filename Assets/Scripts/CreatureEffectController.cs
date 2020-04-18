using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureEffectController : MonoBehaviour
{
	public CreatureController Creature;
	public Ability Ability => Creature.Ability;

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log($"on trigger enter {other.tag}");
		if (Ability == Ability.Electric)
		{
			if (other.tag == "Camera")
			{
				CameraController camera = other.GetComponentInParent<CameraController>();
				camera.Disable();
			}
		}
		else if (Ability == Ability.Poison)
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
		Debug.Log($"on trigger exit {other.tag}");
		if (Ability == Ability.Electric)
		{
			if (other.tag == "Camera")
			{
				CameraController camera = other.GetComponentInParent<CameraController>();
				camera.Enable();
			}
		}
		else if (Ability == Ability.Poison)
		{
			if (other.tag == "Guard")
			{
				GuardController guard = other.GetComponentInParent<GuardController>();
				guard.UnStun();
			}
		}
	}
}
