using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionPoint : MonoBehaviour
{
	public Transform SafeZone;
	private Transform[] SafeZoneSpots;
	int TakenSafeZoneSpots;

	private void Start()
	{
		if (SafeZone != null)
		{
			SafeZoneSpots = new Transform[SafeZone.childCount];
			for (int i = 0; i < SafeZone.childCount; i++)
			{
				SafeZoneSpots[i] = SafeZone.GetChild(i);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Player")
			return;

		PlayerController player = other.GetComponentInParent<PlayerController>();
		player.EnteredExtractionPoint(this);
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag != "Player")
			return;

		PlayerController player = other.GetComponentInParent<PlayerController>();
		player.ExitedExtractionPoint();
	}

	public void ReleaseCreature(CreatureController creature)
	{
		creature.transform.position = SafeZoneSpots[TakenSafeZoneSpots].position;
		TakenSafeZoneSpots++;
	}
}
