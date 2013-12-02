using UnityEngine;
using System.Collections;

public class GAPlayerAnalytics : MonoBehaviour {
    float beatsSinceLevelLoad = 0;

	// Use this for initialization
	void OnEnable()
    {
        BpmManager.OnBeat += UpdateBeat;
        GUIGameCamera.OnPause += PlayerPause;
        GUIGameCamera.OnRestart += PlayerRestartLevel;
        GUIGameCamera.OnToMainMenuFromLevel += PlayerToMainMenuFromLevel;
        HighscoreSceneScript.OnFailedLevel += PlayerFailedLevel;
        HighscoreSceneScript.OnCompletedLevel += PlayerCompletedLevel;
        StressOMeter.OnStressIncrease += StressIncrease;
        StressOMeter.OnStressDecrease += StressDecrease;
        GUIGameCamera.OnTaskEnd += TaskEnd;
	}

    void OnDisable()
    {
        BpmManager.OnBeat -= UpdateBeat;
        GUIGameCamera.OnPause -= PlayerPause;
        GUIGameCamera.OnRestart -= PlayerRestartLevel;
        GUIGameCamera.OnToMainMenuFromLevel -= PlayerToMainMenuFromLevel;
        GUIEndScreenCamera.OnFailedLevel -= PlayerFailedLevel;
        GUIEndScreenCamera.OnCompletedLevel -= PlayerCompletedLevel;
        StressOMeter.OnStressIncrease -= StressIncrease;
        StressOMeter.OnStressDecrease -= StressDecrease;
        GUIGameCamera.OnTaskEnd -= TaskEnd;
    }

    private void UpdateBeat()
    {
        beatsSinceLevelLoad++;
    }

    private void PlayerFailedLevel(float score)
    {
        GA.API.Design.NewEvent("FailedLevel" + FindLevel(Application.loadedLevelName), Time.timeSinceLevelLoad, beatsSinceLevelLoad, FindLevel(Application.loadedLevelName), score);
    }

    private void PlayerCompletedLevel(float score)
    {
        GA.API.Design.NewEvent("CompletedLevel" + FindLevel(Application.loadedLevelName), Time.timeSinceLevelLoad, beatsSinceLevelLoad, FindLevel(Application.loadedLevelName), score);
    }

    private void PlayerToMainMenuFromLevel()
    {
        GA.API.Design.NewEvent("ToMainMenuFromLevel", Time.timeSinceLevelLoad, beatsSinceLevelLoad, FindLevel(Application.loadedLevelName));
    }

    private void PlayerRestartLevel()
    {
        GA.API.Design.NewEvent("RestartLevel", Time.timeSinceLevelLoad, beatsSinceLevelLoad, FindLevel(Application.loadedLevelName));
    }

    private void PlayerPause()
    {
        GA.API.Design.NewEvent("Pause", Time.timeSinceLevelLoad, beatsSinceLevelLoad, FindLevel(Application.loadedLevelName));
    }

    private void StressIncrease()
    {
        GA.API.Design.NewEvent("StressIncrease", Time.timeSinceLevelLoad, beatsSinceLevelLoad, FindLevel(Application.loadedLevelName));
    }

    private void StressDecrease()
    {
        GA.API.Design.NewEvent("StressDecrease", Time.timeSinceLevelLoad, beatsSinceLevelLoad, FindLevel(Application.loadedLevelName));
    }

    private void TaskEnd(string type, int zone)
    {
        GA.API.Design.NewEvent("Task " + type + " ended in ", zone, Time.timeSinceLevelLoad, beatsSinceLevelLoad, FindLevel(Application.loadedLevelName));
    }

    private int FindLevel(string level)
    {
        int lvl;
        switch (level) {
            case "level1":
                lvl = 1;
                break;
            case "level2":
                lvl = 2;
                break;
            case "level3":
                lvl = 3;
                break;
            case "level4":
                lvl = 4;
                break;
            case "level5":
                lvl = 5;
                break;
            case "level6":
                lvl = 6;
                break;
            case "level7":
                lvl = 7;
                break;
            case "level8":
                lvl = 8;
                break;
            case "level9":
                lvl = 9;
                break;
            case "level10":
                lvl = 10;
                break;
            default:
                lvl = 0;
                break;
        }
        return lvl;
    }
}
