using UnityEngine;
using System.Collections;

public class HighscoreScreenLoader : MonoBehaviour 
{
	[SerializeField] private int _starOneScore = 10;
	[SerializeField] private int _starTwoScore = 20;
	[SerializeField] private int _starThreeScore = 30;
	
	public class HighScoreClass 
	{
		public static int starScoreOne;
		public static int starScoreTwo;
		public static int starScoreThree;
		public static int perfectInk;
		public static int perfectPaper;
		public static int perfectUran;
		public static int failedInk;
		public static int failedPaper;
		public static int failedUran;
		public static int totalNodes;
	}
	
	private int perfectInk = 0;
	private int perfectPaper = 0;
	private int perfectUran = 0;
	private int failedInk = 0;
	private int failedPaper = 0;
	private int failedUran = 0;
	private int _totalNodes = 0;
	
	private HighscoreSceneScript hss;
	
	void OnEnable()
    {
		GUIGameCamera.OnGameEnded += DisplayEndScreenWin;
        GUIGameCamera.OnTaskEnd += TaskEndUpdate;
		BpmSequencerItem.OnFailedWithItem += TaskFailed;
		StressOMeter.OnGameFailed += DisplayEndScreenLoose;
    }
	
	// Use this for initialization
	void OnDisable()
	{
		GUIGameCamera.OnGameEnded -= DisplayEndScreenWin;
		GUIGameCamera.OnTaskEnd -= TaskEndUpdate;
        StressOMeter.OnGameFailed -= DisplayEndScreenLoose;
	}
	
	void Start()
	{
		hss = gameObject.AddComponent<HighscoreSceneScript>();
		HighscoreSceneScript._targetScore.failedInk = 0;
		HighscoreSceneScript._targetScore.failedPaper = 0;
		HighscoreSceneScript._targetScore.failedUran = 0;
		HighscoreSceneScript._targetScore.perfectInk = 0;
		HighscoreSceneScript._targetScore.perfectPaper = 0;
		HighscoreSceneScript._targetScore.perfectUran = 0;
		HighscoreSceneScript._targetScore.starScoreOne = _starOneScore;
		HighscoreSceneScript._targetScore.starScoreTwo = _starTwoScore;
		HighscoreSceneScript._targetScore.starScoreThree = _starThreeScore;
	}
	
	public void SetTotalNodes(int _amountOfNodes)
	{
		HighscoreSceneScript._targetScore._totalNodes = _amountOfNodes;
	}
	
	public void DisplayEndScreenWin(int _score)
	{
		SoundManager.StopAllSoundEffects();
        SoundManager.FadeAllMusic();
		hss.GoToHighScoreScreen(Application.loadedLevel - 2, _score, true);
	}
	
	public void DisplayEndScreenLoose(int _score)
	{
		SoundManager.StopAllSoundEffects();
        SoundManager.FadeAllMusic();
		hss.GoToHighScoreScreen(Application.loadedLevel - 2, _score, false);
	}
	
	private void TaskEndUpdate(string type, int zone)
	{
		if(zone == 3)
		{
			switch(type)
			{
				case "Ink":
					HighscoreSceneScript._targetScore.perfectInk++;
					break;
				case "Rods":
					HighscoreSceneScript._targetScore.perfectUran++;
					break;
				case "Paper":
					HighscoreSceneScript._targetScore.perfectPaper++;
					break;
			}
		}
	}
	
	public void TaskFailed(string type)
	{
		switch(type)
			{
				case "Ink":
					HighscoreSceneScript._targetScore.failedInk++;
					break;
				case "Rods":
					HighscoreSceneScript._targetScore.failedUran++;
					break;
				case "Paper":
					HighscoreSceneScript._targetScore.failedPaper++;
					break;
			}
	}
	
	public int GetStarOneScore()
	{
		return _starOneScore;
	}
	public int GetStarTwoScore()
	{
		return _starTwoScore;
	}
	public int GetStarThreeScore()
	{
		return _starThreeScore;
	}
}
