using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProgressBarExampleCode : MonoBehaviour
{
    #region Test variables
    [SerializeField]
    private float duration;
    
    TimerUtilities printers;
    float startTime;
    #endregion

    #region Unity functions
	void Start ()
    {
        printers = gameObject.AddComponent<TimerUtilities>();
        printers.StartTimer(duration);
        
	}

	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Keypad1) && this.gameObject.name == "Cube1")
        {
            printers.StartTimer(duration);
        }
        if(Input.GetKeyDown(KeyCode.Keypad2) && this.gameObject.name == "Cube2")
        {
            printers.StartTimer(duration);
        }
        if(Input.GetKeyDown(KeyCode.Keypad3) && this.gameObject.name == "Cube3")
        {
            printers.StartTimer(duration);
        }
        if(Input.GetKeyDown(KeyCode.Keypad4) && this.gameObject.name == "Cube4")
        {
            printers.StartTimer(duration);
        }
        this.gameObject.renderer.material.SetFloat("_Progress", printers.GetTimeLeftInPctDecimal());
	}
    #endregion
}
