using UnityEngine;
using System.Collections;

public class GACustomID : MonoBehaviour {
    float randomNumber;
    float timeStamp;
    float runningTime;
    string randomID;

    void Awake () {
        randomNumber = Random.value;
        timeStamp = System.DateTime.Now.Ticks;
        runningTime = Time.timeSinceLevelLoad;
        randomID = (randomNumber * timeStamp * runningTime).ToString();

        GA.SettingsGA.SetCustomUserID(randomID);
    }
}
