using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
	public Button[] LevelButtons;
	public TextMeshProUGUI[] LevelTexts;
	public SceneLoader SceneLoader;

	public bool AudioActivated;
	public Sprite AudioOn;
	public Sprite AudioOff;
	public Button AudioToggleButton;
	public AudioSource Audio;

	private readonly Color ActiveColor = Color.white;
	private readonly Color InactiveColor = Color.grey;

	private int LevelUnlocked;

	private void Start()
	{
		LevelUnlocked = PlayerPrefs.GetInt(Utils.LevelUnlockedPrefsKey);
		for (int i = 0; i < LevelButtons.Length; i++)
		{
			LevelTexts[i].color = i <= LevelUnlocked ? ActiveColor : InactiveColor;
			LevelButtons[i].interactable = i <= LevelUnlocked;
		}

		AudioActivated = PlayerPrefs.GetInt(Utils.AudioOnPrefsKey, 1) == 1;
		SetAudioActivated();
	}

	private void SetAudioActivated()
	{
		PlayerPrefs.SetInt(Utils.AudioOnPrefsKey, AudioActivated ? 1 : 0);
		AudioToggleButton.image.sprite = AudioActivated ? AudioOn : AudioOff;
		Audio.mute = AudioActivated == false;
	}

	public void ToggleAudioActivated()
	{
		AudioActivated = !AudioActivated;
		SetAudioActivated();
	}

	public void SelectLevel(int index)
	{
		SceneLoader.LoadScene(index);
	}
}
