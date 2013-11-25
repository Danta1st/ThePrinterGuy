using UnityEngine;
using System.Collections;

public class CreditsScript : MonoBehaviour 
{
	[SerializeField]
	private int _creditsSpeed = 50;
	
	private bool _isRunning = false;
	private GameObject _credits;
	private Vector3 _startPos;
	private Vector3 _endPos;
	private Vector3 _difference;
	// Use this for initialization
	void Start () 
	{
		_credits = GameObject.Find("Credits");
		_startPos = _credits.transform.localPosition;
		_startPos.y -= 50;
		_endPos = _startPos;
		_endPos.y = 1600;
		_difference = _startPos;
		_difference.y = _endPos.y - _startPos.y;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(_isRunning)
		{
			iTween.MoveAdd(_credits, iTween.Hash("amount", _endPos, "speed", _creditsSpeed, "easetype", iTween.EaseType.linear));
			Debug.Log(_credits.transform.localPosition.y);
			if(_credits.transform.localPosition.y >= 1600)
			{
				_credits.transform.position -= _difference;
			}
		}
	}
	
	public void SetCreditsRunning(bool run)
	{
		if(run)
		{
			_credits.transform.localPosition = _startPos;
		}
		_isRunning = run;
	}
}
