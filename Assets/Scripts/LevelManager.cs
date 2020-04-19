using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance;
	public SceneLoader SceneLoader;
	public int LevelIndex;

	public int NumberOfCreatures;
	public int RequiredNumberOfExtractedCreatures;
	private int NumberOfExtractedCreatures;

	private void Awake()
	{
		Instance = this;
	}

	public void CreatureExtracted()
	{
		NumberOfExtractedCreatures++;
		if (NumberOfExtractedCreatures >= NumberOfCreatures)
		{
			Debug.Log("All creatures extracted, leave the level");
		}
		else if (NumberOfExtractedCreatures >= RequiredNumberOfExtractedCreatures)
		{
			Debug.Log("Level can be completed, but there are still creatures to rescue");
		}
	}

	public void FinishLevel()
	{
		if (NumberOfExtractedCreatures >= RequiredNumberOfExtractedCreatures)
		{
			int levelUnlocked = PlayerPrefs.GetInt(Utils.LevelUnlockedPrefsKey);
			if (LevelIndex > levelUnlocked)
			{
				PlayerPrefs.SetInt(Utils.LevelUnlockedPrefsKey, LevelIndex);
			}
		}

		SceneLoader.LoadScene(0);
	}
}
