using UnityEngine;

public static class Utils
{
	public static readonly Color DefaultFlashlightColor = Color.white;
	public static readonly Color DetectedFlashlightColor = Color.red;

	public const string LevelUnlockedPrefsKey = "LevelUnlocked";

	public static Vector3 GetVectorFromAngle(float angle, float yOffset = 0.0f)
	{
		float angleRad = angle * Mathf.Deg2Rad;
		return new Vector3(Mathf.Cos(angleRad), yOffset, Mathf.Sin(angleRad)).normalized;
	}

	public static float GetAngleFromVector(Vector3 vector)
	{
		vector.y = 0.0f;
		vector = vector.normalized;
		float n = Mathf.Atan2(vector.z, vector.x) * Mathf.Rad2Deg;
		if (n < 0.0f)
			n += 360.0f;
		return n;
	}
}

public static class AnimatorParams
{
	public const string Moving = "Moving";
	public const string Carrying = "Carrying";
}
