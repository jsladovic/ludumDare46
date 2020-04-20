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
		GameManager.Instance.DisplayMessage("According to info, Phillip should be straight ahead, then left at the crossroad.", MessageSource.Driver);
	}
}
