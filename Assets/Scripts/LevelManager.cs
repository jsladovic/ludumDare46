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

	public MissionOverCanvasController MissionOverCanvas;

	private void Awake()
	{
		Instance = this;
		Time.timeScale = 1.0f;
	}

	public void CreatureExtracted()
	{
		NumberOfExtractedCreatures++;
		if (NumberOfExtractedCreatures >= NumberOfCreatures)
		{
			GameManager.Instance.DisplayMessage("Right, that's all of them, now hold that E button and you're done!", MessageSource.Driver);
		}
		else if (NumberOfExtractedCreatures >= RequiredNumberOfExtractedCreatures)
		{
			GameManager.Instance.DisplayMessage("That should be enough to get the job done, but if you want there are still around here.", MessageSource.Driver);
		}
		else
		{
			GameManager.Instance.DisplayMessage("One more they won't be able to display in those filthy cages", MessageSource.Driver);
		}
	}

	public void FinishLevel()
	{
		StartCoroutine(FinishLevelCoroutine());
	}

	private IEnumerator FinishLevelCoroutine()
	{
		Outcome outcome = Outcome.Success;
		if (NumberOfExtractedCreatures >= RequiredNumberOfExtractedCreatures)
		{
			int levelUnlocked = PlayerPrefs.GetInt(Utils.LevelUnlockedPrefsKey);
			if (LevelIndex > levelUnlocked)
			{
				PlayerPrefs.SetInt(Utils.LevelUnlockedPrefsKey, LevelIndex);
			}

			if (NumberOfExtractedCreatures == NumberOfCreatures)
			{
				GameManager.Instance.DisplayMessage("Brilliant job, we're outta here!", MessageSource.Driver, 3);
			}
			else
			{
				GameManager.Instance.DisplayMessage("Didn't get them all, but still job done, let's go!", MessageSource.Driver, 3);
			}
		}
		else
		{
			outcome = Outcome.Coward;
			GameManager.Instance.DisplayMessage("All of this is a bit too much for you, isn't it? Don't worry, we'll find you a nice simple job in accounting...", MessageSource.Driver, 3);
		}

		yield return new WaitForSeconds(4);
		MissionOverCanvas.Display(outcome);
	}
}
