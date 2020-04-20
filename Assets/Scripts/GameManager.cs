using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public bool Alarmed { get; private set; }
	public bool Paused { get; private set; }

	public Canvas PauseMenuCanvas;

	public Canvas MessageCanvas;
	public TextMeshProUGUI MessageText;

	private Coroutine DisplayMessageCoroutine;

	private void Awake()
	{
		Instance = this;
		Alarmed = false;
		PauseMenuCanvas.enabled = false;
		MessageCanvas.enabled = false;
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
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			TogglePause();
		}
    }

	private void TogglePause()
	{
		Paused = !Paused;
		if (Paused == true)
		{
			// Pausing game
			Time.timeScale = 0.0f;
			PauseMenuCanvas.enabled = true;
		}
		if (Paused == false)
		{
			// Resuming game
			Time.timeScale = 1.0f;
			PauseMenuCanvas.enabled = false;
		}
	}

	public void ResumeGame()
	{
		if (Paused == true)
			TogglePause();
	}

	public void BackToMainMenu()
	{
		LevelManager.Instance.SceneLoader.LoadScene(0);
	}

	public void RestartLevel()
	{
		LevelManager.Instance.SceneLoader.LoadScene(LevelManager.Instance.LevelIndex);
	}

	public void RaiseSecurityAwareness()
	{
		Alarmed = true;
	}

	private void LowerSecurityAwareness()
	{
		Alarmed = false;
	}

	public void DisplayMessage(string message, MessageSource source)
	{
		if (DisplayMessageCoroutine != null)
		{
			StopCoroutine(DisplayMessageCoroutine);
		}
		MessageText.text = message;
		MessageCanvas.enabled = true;
		DisplayMessageCoroutine = StartCoroutine(HideMessageCanvas(5));
	}

	private IEnumerator HideMessageCanvas(int seconds)
	{
		yield return new WaitForSeconds(seconds);
		MessageCanvas.enabled = false;
	}

	public void DisplayDefeatMessage(bool playerDetected, MessageSource source)
	{
		if (source == MessageSource.Guard)
		{
			if (playerDetected)
			{
				DisplayMessage("Hey there! You! Stop or I'll shoot!", source);
			}
			else
			{
				DisplayMessage("Calling all guards,runaway creature found, moving to maximum security!", source);
			}
		}
		else if (source == MessageSource.Camera)
		{
			if (playerDetected)
			{
				DisplayMessage("Hey there! You! Stop or I'll shoot... bolts of electricity... Yeah, you don't want to test me!", source);
			}
			else
			{
				DisplayMessage("Calling all guards, found a runaway creature, please transport to maximum security!", source);
			}
		}
		else
		{
			throw new UnityException($"Player can't be defeated by {source.ToString()}");
		}
	}
}

public enum MessageSource
{
	Player = 1,
	Driver = 2,
	Guard = 3,
	Camera = 4
}
