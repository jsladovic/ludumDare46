using UnityEngine;

public class LevelSelector : MonoBehaviour
{
	public SceneLoader SceneLoader;

	public void SelectLevel(int index)
	{
		SceneLoader.LoadScene(index);
	}
}
