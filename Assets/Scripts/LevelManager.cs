using System.Collections;
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
		StartCoroutine(FinishLevelCoroutine());
	}

	private IEnumerator FinishLevelCoroutine()
	{
		if (NumberOfExtractedCreatures >= RequiredNumberOfExtractedCreatures)
		{
			int levelUnlocked = PlayerPrefs.GetInt(Utils.LevelUnlockedPrefsKey);
			if (LevelIndex > levelUnlocked)
			{
				PlayerPrefs.SetInt(Utils.LevelUnlockedPrefsKey, LevelIndex);
			}

			if (NumberOfExtractedCreatures == NumberOfCreatures)
			{
				GameManager.Instance.DisplayMessage("Brilliant job, we're outta here!", MessageSource.Driver);
			}
			else
			{
				GameManager.Instance.DisplayMessage("Didn't get them all, but still job done, let's go!", MessageSource.Driver);
			}
		}
		else
		{
			GameManager.Instance.DisplayMessage("All of this is a bit too much for you, isn't it? Don't worry, we'll find you a nice simple job in accounting...", MessageSource.Driver);
		}

		yield return new WaitForSeconds(3);
		SceneLoader.LoadScene(0);
	}
}
