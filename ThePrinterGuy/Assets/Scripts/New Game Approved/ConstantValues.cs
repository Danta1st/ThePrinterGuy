﻿using UnityEngine;
using System.Collections;

public static class ConstantValues
{
	public static int GetStartScene
	{
		get { return 0; }
	}
	public static int GetLevel1
	{
		get { return 2; }
	}
	public static int GetLastLevel
	{
		get { return 11; }
	}
	public static int GetHighScoreScreenLevel
	{
		get { return 18; }
	}
	public static int GetLoadingLevel
	{
		get { return 12; }
	}
	public static int GetLoadedLevelMinusStartLevels(int loadedLevel)
	{
		return loadedLevel - GetLevel1;
	}
}
