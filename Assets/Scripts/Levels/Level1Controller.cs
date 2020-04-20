using System.Collections;
using UnityEngine;

public class Level1Controller : MonoBehaviour
{
	void Start()
	{
		StartCoroutine(DisplayIntroMessage());
	}

	private IEnumerator DisplayIntroMessage()
	{
		yield return new WaitForSeconds(1.0f);
		GameManager.Instance.DisplayMessage("Okay, according to info, the creature should be just straight ahead, then left at the crossroad.", MessageSource.Driver);
	}
}
