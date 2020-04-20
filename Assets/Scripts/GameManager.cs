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

	private void RaiseSecurityAwareness()
	{
		Alarmed = true;
	}

	private void LowerSecurityAwareness()
	{
		Alarmed = false;
	}

	public void DisplayMessage(string message, MessageSource source)
	{
		MessageText.text = message;
		MessageCanvas.enabled = true;
		StartCoroutine(HideMessageCanvas(3));
	}

	private IEnumerator HideMessageCanvas(int seconds)
	{
		yield return new WaitForSeconds(seconds);
		MessageCanvas.enabled = false;
	}
}

public enum MessageSource
{
	Player = 1,
	Driver = 2,
	Guard = 3,
	Camera = 4
}
