﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public CharacterController Controller;
	private const float Speed = 2.0f;

	private const float PickUpRadius = 1.5f;

	public Transform CarryPosition;
	public Transform DropPosition;
	private CreatureController CarriedCreature;

    void Start()
    {
        
    }

    void Update()
	{
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
			transform.rotation = Quaternion.LookRotation(direction);
			Controller.Move(direction * Speed * Time.deltaTime);
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			HandlePickUp();
		}
	}

	private void HandlePickUp()
	{
		if (CarriedCreature == null)
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
		else
		{
			// Drop creature
			CarriedCreature.transform.SetParent(transform.parent);
			CarriedCreature.transform.position = DropPosition.position;
			CarriedCreature = null;
		}
	}
}
