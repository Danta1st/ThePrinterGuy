using UnityEngine;
using System.Collections;

public class HighscoreScreenLoader : MonoBehaviour 
{
	private int perfectInk = 0;
	private int perfectPaper = 0;
	private int perfectUran = 0;
	private int failedInk = 0;
	private int failedPaper = 0;
	private int failedUran = 0;
    private int _totalNodes = 0;
    private int _totalInkNodes = 0;
    private int _totalRodNodes = 0;
    private int _totalPaperNodes = 0;
    private int _totalBarometerNodes = 0;
    private int _maxHighscore;

    private ScoreManager _scoreManger;
    private StressOMeter _stressOMeter;

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
		BpmSequencerItem.OnFailedWithItem -= TaskFailed;
        StressOMeter.OnGameFailed -= DisplayEndScreenLoose;
	}
	
	void Start()
	{
		Debug.Log ("WAT");
        _scoreManger = gameObject.GetComponent<ScoreManager>();
        _stressOMeter = gameObject.GetComponentInChildren<StressOMeter>();
        updateMaxHighscoreForCurrentLevel(); // Something is wrong here
        _maxHighscore = GetPerfectScore();//SaveGame.GetMaxHighscores()[Application.loadedLevel - 2];
		hss = gameObject.AddComponent<HighscoreSceneScript>();
		HighscoreSceneScript._targetScore.failedInk = 0;
		HighscoreSceneScript._targetScore.failedPaper = 0;
		HighscoreSceneScript._targetScore.failedUran = 0;
		HighscoreSceneScript._targetScore.perfectInk = 0;
		HighscoreSceneScript._targetScore.perfectPaper = 0;
		HighscoreSceneScript._targetScore.perfectUran = 0;
		HighscoreSceneScript._targetScore._totalNodesHit = 0;
		HighscoreSceneScript._targetScore.starScoreOne = GetStarOneScore();
		HighscoreSceneScript._targetScore.starScoreTwo = GetStarTwoScore();
		HighscoreSceneScript._targetScore.starScoreThree = GetStarThreeScore();
	}
	
    public void SetTotalNodes(int _amountOfNodes)
    {
        HighscoreSceneScript._targetScore._totalNodes = _amountOfNodes;
        _totalNodes = _amountOfNodes;
    }

    public void SetRodTotalNodes(int _amountOfNodes)
    {
        _totalRodNodes = _amountOfNodes;
    }

    public void SetInkTotalNodes(int _amountOfNodes)
    {
        _totalInkNodes = _amountOfNodes;
    }

    public void SetPaperTotalNodes(int _amountOfNodes)
    {
        _totalPaperNodes = _amountOfNodes;
    }

    public void SetBarometerTotalNodes(int _amountOfNodes)
    {
        _totalBarometerNodes = _amountOfNodes;
    }

	public void DisplayEndScreenWin(int _score)
	{
		SoundManager.StopAllSoundEffects();
        SoundManager.FadeAllMusic();
		hss.GoToHighScoreScreen(Application.loadedLevel - 2, _score, true, GetPerfectScore());
	}
	
	public void DisplayEndScreenLoose(int _score)
	{
		SoundManager.StopAllSoundEffects();
        SoundManager.FadeAllMusic();
		hss.GoToHighScoreScreen(Application.loadedLevel - 2, _score, false, GetPerfectScore());
	}

    public int GetPerfectScore()
    {
        float result = 0;
        float happyZone = _stressOMeter.GetHappyZone();
        float zonePoints = _stressOMeter.GetZonePoints();
        float perfectTasksBeforeBestMultiplier = zonePoints - ((-1 * happyZone) % zonePoints);
        float rodPointBase = _scoreManger.GetRodPointsBase();
        float remainingNodes = _totalNodes - perfectTasksBeforeBestMultiplier;

        if(remainingNodes < 0)
        {
            perfectTasksBeforeBestMultiplier += remainingNodes;
            remainingNodes = 0;
        }

        for (int i = 0; i < perfectTasksBeforeBestMultiplier; i++)
        {
            result += rodPointBase * _scoreManger.GetGreenZoneModifier() * _scoreManger.GetScoreMultipliers().slightlyHappyZoneMultiplier;
        }

        for (int i = 0; i < remainingNodes; i++)
        {
            result += rodPointBase * _scoreManger.GetGreenZoneModifier() * _scoreManger.GetScoreMultipliers().happyZoneMultiplier;
        }

        return System.Convert.ToInt32(result);
    }

    public void updateMaxHighscoreForCurrentLevel()
    {
        int[] maxHighscores = SaveGame.GetMaxHighscores();
        maxHighscores[Application.loadedLevel - 2] = GetPerfectScore();
        SaveGame.SetMaxHighscores(maxHighscores);
    }
	
	private void TaskEndUpdate(string type, int zone)
	{
		HighscoreSceneScript._targetScore._totalNodesHit++;
		Debug.Log (HighscoreSceneScript._targetScore._totalNodesHit);
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
				case "UraniumRod":
					HighscoreSceneScript._targetScore.failedUran++;
					break;
				case "Paper":
					HighscoreSceneScript._targetScore.failedPaper++;
					break;
			}
	}
	
	public int GetStarOneScore()
	{
		return System.Convert.ToInt32(_maxHighscore * 0.25f);
	}
	public int GetStarTwoScore()
	{
		return System.Convert.ToInt32(_maxHighscore * 0.50f);
	}
	public int GetStarThreeScore()
	{
		return System.Convert.ToInt32(_maxHighscore * 0.75f);
	}
}
