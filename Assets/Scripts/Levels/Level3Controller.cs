using System.Collections;
using UnityEngine;

public class Level3Controller : MonoBehaviour
{
	void Start()
	{
		StartCoroutine(DisplayIntroMessage());
	}

	private IEnumerator DisplayIntroMessage()
	{
		yield return new WaitForSeconds(1.0f);
		GameManager.Instance.DisplayMessage("George Gilbert should be straight ahead. Good luck.", MessageSource.Driver);
	}
}
