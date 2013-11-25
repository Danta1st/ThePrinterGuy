using UnityEngine;
using System.Collections;

public class PlayerAnalytics : MonoBehaviour {
    float beatsSinceLevelLoad = 0;

	// Use this for initialization
	void Start () {
        BpmManager.OnBeat += UpdateBeat;
        GUIGameCamera.OnPause += PlayerPause;
        GUIGameCamera.OnRestart += PlayerRestartLevel;
        GUIGameCamera.OnToMainMenuFromLevel += PlayerToMainMenu;
        HighscoreSceneScript.OnFailedLevel += PlayerFailedLevel;
        HighscoreSceneScript.OnCompletedLevel += PlayerCompletedLevel;
        StressOMeter.OnStressIncrease += StressIncrease;
        StressOMeter.OnStressDecrease += StressDecrease;
        StressOMeter.OnGameFailed += PlayerFailedLevel;
	}

    private void UpdateBeat()
    {
        beatsSinceLevelLoad++;
    }

    private void PlayerFailedLevel(float score) //
    {
        GA.API.Design.NewEvent("FailedLevel", Time.timeSinceLevelLoad, beatsSinceLevelLoad, FindLevel(Application.loadedLevelName), score);
    }

    private void PlayerCompletedLevel(float score)//
    {
        GA.API.Design.NewEvent("CompletedLevel", Time.timeSinceLevelLoad, beatsSinceLevelLoad, FindLevel(Application.loadedLevelName), score);
    }

    private void PlayerToMainMenuFromLevel()//
    {
        GA.API.Design.NewEvent("ToMainMenuFromLevel", Time.timeSinceLevelLoad, beatsSinceLevelLoad, FindLevel(Application.loadedLevelName));
    }

    private void PlayerRestartLevel()//
    {
        GA.API.Design.NewEvent("RestartLevel", Time.timeSinceLevelLoad, beatsSinceLevelLoad, FindLevel(Application.loadedLevelName));
    }

    private void PlayerPause()//
    {
        GA.API.Design.NewEvent("Pause", Time.timeSinceLevelLoad, beatsSinceLevelLoad, FindLevel(Application.loadedLevelName));
    }

    private void StressIncrease()//
    {
        GA.API.Design.NewEvent("StressIncrease", Time.timeSinceLevelLoad, beatsSinceLevelLoad, FindLevel(Application.loadedLevelName));
    }

    private void StressDecrease()//
    {
        GA.API.Design.NewEvent("StressDecrease", Time.timeSinceLevelLoad, beatsSinceLevelLoad, FindLevel(Application.loadedLevelName));
    }

  /*private void Task()
    {
        GA.API.Design.NewEvent("Task", Time.timeSinceLevelLoad, beatsSinceLevelLoad, Application.loadedLevelName);
    }*/

    private int FindLevel(string level)
    {
        switch (level) {
            case "level1":
                return 1;
                break;
            case "level2":
                return 2;
                break;
            case "level3":
                return 3;
                break;
            default:
                return 0;
                break;
        }
    }
}
