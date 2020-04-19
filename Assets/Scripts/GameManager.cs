using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public bool Alarmed { get; private set; }

	private void Awake()
	{
		Instance = this;
		Alarmed = false;
	}

	void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
		{
			RaiseSecurityAwareness();
		}
		if (Input.GetKeyDown(KeyCode.O))
		{
			LowerSecurityAwareness();
		}
    }

	private void RaiseSecurityAwareness()
	{
		Alarmed = true;
	}

	private void LowerSecurityAwareness()
	{
		Alarmed = false;
	}
}
