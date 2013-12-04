using UnityEngine;
using System.Collections;

public static class ConstantValues
{
	public static int GetStartScene
	{
		get { return 1; }
	}
	public static int GetLevel1
	{
		get { return 3; }
	}
	public static int GetLastLevel
	{
		get { return 12; }
	}
	public static int GetHighScoreScreenLevel
	{
		get { return 19; }
	}
	public static int GetLoadingLevel
	{
		get { return 13; }
	}
	public static int GetLoadedLevelMinusStartLevels(int loadedLevel)
	{
		return loadedLevel - GetLevel1;
	}
}
