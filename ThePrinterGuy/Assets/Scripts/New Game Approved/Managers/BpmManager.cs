using UnityEngine;
using System.Collections;

public class BpmManager : MonoBehaviour
{	
	#region Privates
	private double _bpm = 120.0f;
	private double _waitTime;
	private double _tempTime = 0;
	private bool _isBeating = true;
	#endregion
	
	#region Delegates & Events
	public delegate void OnBeatAction ();
	public static event OnBeatAction OnBeat;
	#endregion
	
	void Awake ()
	{
		_waitTime = 30.0f / _bpm;
		_tempTime = AudioSettings.dspTime;
	}
	
	void Update ()
	{
		if (_isBeating) {
			if (AudioSettings.dspTime - _tempTime >= _waitTime) {
				_tempTime += _waitTime;
				
				if (OnBeat != null)
					OnBeat ();
			}
		}
	}
}

