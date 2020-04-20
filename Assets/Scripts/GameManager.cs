using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public bool Alarmed { get; private set; }
	public bool Paused { get; private set; }

	public Canvas PauseMenuCanvas;

	public Canvas MessageCanvas;
	public TextMeshProUGUI MessageText;
	public Image CallerSprite;
	public Sprite PlayerSprite;
	public Sprite DriverSprite;
	public Sprite GuardSprite;
	public Sprite CameraSprite;

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

	public void DisplayMessage(string message, MessageSource source, int seconds = 5)
	{
		if (DisplayMessageCoroutine != null)
		{
			StopCoroutine(DisplayMessageCoroutine);
		}
		MessageText.text = message;
		CallerSprite.sprite = GetSourceSprite(source);
		MessageCanvas.enabled = true;
		DisplayMessageCoroutine = StartCoroutine(HideMessageCanvas(seconds));
	}

	public Sprite GetSourceSprite(MessageSource source)
	{
		switch (source)
		{
			case MessageSource.Camera:
				return CameraSprite;
			case MessageSource.Driver:
				return DriverSprite;
			case MessageSource.Guard:
				return GuardSprite;
			case MessageSource.Player:
				return PlayerSprite;
		}
		throw new UnityException($"Unknown source {source.ToString()}");
	}

	private IEnumerator HideMessageCanvas(int seconds)
	{
		yield return new WaitForSeconds(seconds);
		MessageCanvas.enabled = false;
	}

	public void DisplayDefeatMessage(bool playerDetected, MessageSource source)
	{
		Outcome outcome = Outcome.Defeat;
		if (source == MessageSource.Guard)
		{
			if (playerDetected)
			{
				DisplayMessage("You there, the one with the thief outfit! Stop or I'll shoot!", source, 3);
			}
			else
			{
				outcome = Outcome.CreatureCaptured;
				DisplayMessage("Runaway creature detected, moving it to maximum security!", source, 3);
			}
		}
		else if (source == MessageSource.Camera)
		{
			if (playerDetected)
			{
				DisplayMessage("Stop or I'll shoot... My camera rays! Yeah, you don't want to test me!", source, 3);
			}
			else
			{
				outcome = Outcome.CreatureCaptured;
				DisplayMessage("Can one of you lazy bastards come and pick this creature up?", source, 3);
			}
		}
		else
		{
			throw new UnityException($"Player can't be defeated by {source.ToString()}");
		}

		StartCoroutine(DefeatCoroutine(outcome));
	}

	private IEnumerator DefeatCoroutine(Outcome outcome)
	{
		yield return new WaitForSeconds(4);
		LevelManager.Instance.MissionOverCanvas.Display(outcome);
	}
}

public enum MessageSource
{
	Player = 1,
	Driver = 2,
	Guard = 3,
	Camera = 4
}
