using UnityEngine;
using System.Collections;

public class HighscoreScreenLoader : MonoBehaviour 
{
	void OnEnable()
    {
		GUIGameCamera.OnGameEnded += DisplayEndScreenWin;
        StressOMeter.OnGameFailed += DisplayEndScreenLoose;
    }
	
	// Use this for initialization
	void OnDisable()
	{
		GUIGameCamera.OnGameEnded -= DisplayEndScreenWin;
        StressOMeter.OnGameFailed -= DisplayEndScreenLoose;
	}
	
	public void DisplayEndScreenWin(int _score)
	{
		HighscoreSceneScript hss = new HighscoreSceneScript();
		hss.GoToHighScoreScreen(Application.loadedLevel, _score, true, 100, 300, 500);
	}
	
	public void DisplayEndScreenLoose(int _score)
	{
		HighscoreSceneScript hss = new HighscoreSceneScript();
		hss.GoToHighScoreScreen(Application.loadedLevel, _score, false, 100, 300, 500);
	}
}
