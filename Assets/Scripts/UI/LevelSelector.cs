using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
	public Button[] LevelButtons;
	public TextMeshProUGUI[] LevelTexts;
	public SceneLoader SceneLoader;

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
	}

	public void SelectLevel(int index)
	{
		SceneLoader.LoadScene(index);
	}
}
