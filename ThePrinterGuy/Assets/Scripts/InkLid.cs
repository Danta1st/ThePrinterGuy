using UnityEngine;
using System.Collections;

public class InkLid : MonoBehaviour {
	
	[SerializeField]
	private float _rotationAmount = 90;
	[SerializeField]
	private float _rotationSpeed = 0.2f;
	[SerializeField]
	private iTween.EaseType _easeType;
	
	private bool _isOpen;
	private float _openRot;
	private float _closedRot;

	public void InitializeLid(bool isOpen) {
		_isOpen = isOpen;
		
		if(_isOpen)
		{
			_openRot = this.gameObject.transform.rotation.x;
			_closedRot = this.gameObject.transform.rotation.x + _rotationAmount;
		}
		else
		{
			_openRot = this.gameObject.transform.rotation.x - _rotationAmount;
			_closedRot = this.gameObject.transform.rotation.x;
		}
	}
	
	public void SwapLidState()
	{
		if(_isOpen)
		{
			iTween.RotateTo(this.gameObject, iTween.Hash("x", _closedRot,
				"easetype", _easeType));
			_isOpen = false;
		}
		else
		{
			iTween.RotateTo(this.gameObject, iTween.Hash("x", _openRot,
				"easetype", _easeType));
			_isOpen = true;
		}
	}
	
	public bool IsOpen()
	{
		return _isOpen;
	}
}
