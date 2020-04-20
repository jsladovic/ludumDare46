using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
	public string Text;
	public MessageSource MessageSource;
	public bool AlarmedReequired;
	private bool Triggered;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Player")
			return;

		if (Triggered)
			return;

		if (AlarmedReequired == true && GameManager.Instance.Alarmed == false)
			return;

		Triggered = true;
		GameManager.Instance.DisplayMessage(Text, MessageSource);
	}
}
