using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
	public Slider PercentageSlider;

	public void LoadScene(int index)
	{
		StartCoroutine(LoadAsynchronously(index));
	}

	private IEnumerator LoadAsynchronously(int index)
	{
		if (PercentageSlider != null)
			PercentageSlider.gameObject.SetActive(true);

		AsyncOperation operation = SceneManager.LoadSceneAsync(index);
		while (operation.isDone == false)
		{
			float progress = Mathf.Clamp01(operation.progress / 0.9f);
			if (PercentageSlider != null)
				PercentageSlider.value = (int)progress;

			yield return null;
		}

		if (PercentageSlider != null)
			PercentageSlider.gameObject.SetActive(false);
	}
}
