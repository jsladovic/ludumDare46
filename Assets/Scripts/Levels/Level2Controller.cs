using System.Collections;
using UnityEngine;

public class Level2Controller : MonoBehaviour
{
	void Start()
	{
		StartCoroutine(DisplayIntroMessage());
	}

	private IEnumerator DisplayIntroMessage()
	{
		yield return new WaitForSeconds(1.0f);
		GameManager.Instance.DisplayMessage("Right, this time take the right at the intersection and we'll find Hutchinson", MessageSource.Driver);
	}
}
