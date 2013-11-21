using UnityEngine;
using System.Collections;

public class BeatController : MonoBehaviour {
	
	#region Privates
	private int _beatCounter = 0; 
	#endregion
	
	#region Delegates & Events
	public delegate void OnBeatAction();
	//3 tact beats
	public static event OnBeatAction OnBeat3rd1;
	public static event OnBeatAction OnBeat3rd2;
	public static event OnBeatAction OnBeat3rd3;
	//4 tact beats
	public static event OnBeatAction OnBeat4th1;
	public static event OnBeatAction OnBeat4th2;
	public static event OnBeatAction OnBeat4th3;
	public static event OnBeatAction OnBeat4th4;
	//6 tact Beats
	public static event OnBeatAction OnBeat6th1;
	public static event OnBeatAction OnBeat6th2;
	public static event OnBeatAction OnBeat6th3;
	public static event OnBeatAction OnBeat6th4;
	public static event OnBeatAction OnBeat6th5;
	public static event OnBeatAction OnBeat6th6;
	//8 tact Beats
	public static event OnBeatAction OnBeat8th1;
	public static event OnBeatAction OnBeat8th2;
	public static event OnBeatAction OnBeat8th3;
	public static event OnBeatAction OnBeat8th4;
	public static event OnBeatAction OnBeat8th5;
	public static event OnBeatAction OnBeat8th6;
	public static event OnBeatAction OnBeat8th7;
	public static event OnBeatAction OnBeat8th8;
	#endregion
	
	void OnEnable()
	{
		BpmManager.OnBeat += UpdateBeatCounter;
	}
	void OnDisable()
	{
		BpmManager.OnBeat -= UpdateBeatCounter;
	}
	
	#region Class Methods
	//Primary beat counter
	private void UpdateBeatCounter()
	{
		if(_beatCounter +1 <= 240)
			_beatCounter++;
		else
			_beatCounter = 0;
		
		CheckBeat3rd();
		CheckBeat4th();
		CheckBeat6th();
		CheckBeat8th();
	}
	
	//3 tact event calls
	private void CheckBeat3rd()
	{
		switch(_beatCounter%6)
		{
		case 0:
			//0ms & 2000ms?
			if(OnBeat3rd1 != null)
				OnBeat3rd1();
			break;
		case 2:
			//667ms
			if(OnBeat3rd2 != null)
				OnBeat3rd2();
			break;
		case 4:
			//1333ms
			if(OnBeat3rd3 != null)
				OnBeat3rd3();
			break;
		default:
			break;
		}
	}
	
	//4 tact event calls
	private void CheckBeat4th()
	{
		switch(_beatCounter%4)
		{
		case 0:
			//0ms & 2000ms?
			if(OnBeat4th1 != null)
				OnBeat4th1();
			break;
		case 2:
			//500ms
			if(OnBeat4th2 != null)
				OnBeat4th2();
			break;
		case 4:
			//1000ms
			if(OnBeat4th3 != null)
				OnBeat4th3();
			break;
		case 6:
			//1500ms
			if(OnBeat4th4 != null)
				OnBeat4th4();
			break;
		default:
			break;
		}
	}
	
	//6 tact event calls
	private void CheckBeat6th()
	{
		switch(_beatCounter%12)
		{
		case 0:
			//0ms & 2000ms?
			if(OnBeat6th1 != null)
				OnBeat6th1();
			break;
		case 2:
			//333ms
			if(OnBeat6th2 != null)
				OnBeat6th2();
			break;
		case 4:
			//667ms
			if(OnBeat6th3 != null)
				OnBeat6th3();
			break;
		case 6:
			//1000ms
			if(OnBeat6th4 != null)
				OnBeat6th4();
			break;
		case 8:
			//1333ms
			if(OnBeat6th5 != null)
				OnBeat6th5();
			break;
		case 10:
			//1667ms
			if(OnBeat6th6 != null)
				OnBeat6th6();
			break;
		default:
			break;
		}
	}
	
	//8 tact event calls
	private void CheckBeat8th()
	{
		switch(_beatCounter%8)
		{
		case 0:
			//0ms & 2000ms?
			if(OnBeat8th1 != null)
				OnBeat8th1();
			break;
		case 1:
			//250ms
			if(OnBeat8th2 != null)
				OnBeat8th2();
			break;
		case 2:
			//500ms
			if(OnBeat8th3 != null)
				OnBeat8th3();
			break;
		case 3:
			//750ms
			if(OnBeat8th4 != null)
				OnBeat8th4();
			break;
		case 4:
			//1000ms
			if(OnBeat8th5 != null)
				OnBeat8th5();
			break;
		case 5:
			//1250ms
			if(OnBeat8th6 != null)
				OnBeat8th6();
			break;
		case 6:
			//1500ms
			if(OnBeat8th7 != null)
				OnBeat8th7();
			break;
		case 7:
			//1750ms
			if(OnBeat8th8 != null)
				OnBeat8th8();
			break;
		default:
			break;
		}
	}
	#endregion
}