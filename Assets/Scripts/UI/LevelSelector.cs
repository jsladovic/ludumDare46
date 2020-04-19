using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
	public Button[] LevelButtons;
	public SceneLoader SceneLoader;

	private int LevelUnlocked;

	private void Start()
	{
		LevelUnlocked = PlayerPrefs.GetInt(Utils.LevelUnlockedPrefsKey);
		for (int i = 0; i < LevelButtons.Length; i++)
		{
			LevelButtons[i].interactable = i <= LevelUnlocked;
		}
	}

	public void SelectLevel(int index)
	{
		SceneLoader.LoadScene(index);
	}
}
