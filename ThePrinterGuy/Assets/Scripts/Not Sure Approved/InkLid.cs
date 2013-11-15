using UnityEngine;
using System.Collections;

public class InkLid : MonoBehaviour {
	
	[SerializeField]
	private float _rotationAmount = 90;
	[SerializeField]
	private float _rotationTime = 0.2f;
	[SerializeField]
	private iTween.EaseType _easeType;
	[SerializeField]
	private float _timeBetweenOpenClose = 0.5f;
	
	private bool _isOpen;
	private float _openRot;
	private float _closedRot;
	private float _timeDelay = 0.5f;
	private float _initTime;
	private bool _isInit = false;
	private Quaternion _startRot;

	void Update()
	{
		if(_isInit)
		{
			if(_initTime + _timeDelay <= Time.time)
			{
				StartCoroutine("OpenCloseLid");
				_isInit = false;
			}
		}
		
	}
	
	public void InitializeLid(bool isOpen) {
		_isOpen = isOpen;
		_isInit = true;
		
		_startRot = this.gameObject.transform.parent.gameObject.transform.rotation;
		
		if(_isOpen)
		{
			_openRot = this.gameObject.transform.parent.gameObject.transform.rotation.x;
			_closedRot = this.gameObject.transform.parent.gameObject.transform.rotation.x + _rotationAmount;
		}
		else
		{
			_openRot = this.gameObject.transform.parent.gameObject.transform.rotation.x - _rotationAmount;
			_closedRot = this.gameObject.transform.parent.gameObject.transform.rotation.x;
		}
	}
	
	public void SwapLidState()
	{
		if(_isOpen)
		{
			iTween.RotateTo(this.gameObject.transform.parent.gameObject, iTween.Hash("x", _closedRot,
				"easetype", _easeType, "time", _rotationTime));	
			
			_isOpen = false;
		}
		else
		{
			iTween.RotateTo(this.gameObject.transform.parent.gameObject, iTween.Hash("x", _openRot,
				"easetype", _easeType, "time", _rotationTime));	

			_isOpen = true;
		}
	}

	public void DisableLid()
	{
		_isInit = false;
		StopCoroutine("OpenCloseLid");
	}
	
	public bool IsOpen()
	{
		return _isOpen;
	}
	
	public void StartTime(float timeBetween)
	{
		_initTime = Time.time;
		_timeDelay = timeBetween;
		//StartCoroutine("OpenCloseLid");
	}
	
	public void StartTime()
	{
		_initTime = Time.time;
		//StartCoroutine("OpenCloseLid");
	}
	
	public void StopLid()
	{
		this.gameObject.transform.parent.gameObject.transform.rotation = _startRot;
		StopCoroutine("OpenCloseLid");
	}
	
	IEnumerator OpenCloseLid()
	{
		while(true)
		{
			SwapLidState();
			
			yield return new WaitForSeconds(_timeBetweenOpenClose);
		}
	}
}
