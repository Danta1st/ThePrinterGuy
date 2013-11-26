using UnityEngine;
using System.Collections;

public class ItemIdleState : MonoBehaviour {
	
	[SerializeField] private Vector3 _floatAmount;
	[SerializeField] private float _speedFloat;
	[SerializeField] private iTween.EaseType _easeTypeFloat;
	
	//[SerializeField]
	private Vector3 _rotationAmount;
	//[SerializeField]
	private float _speedRotate;
	//[SerializeField]
	private iTween.EaseType _easeTypeRotate;
	
	private Vector3 _startPosition;
	private Vector3 _newPosition;
	private Quaternion _startRotation;
	private Vector3 _newRotation;
	
	// Use this for initialization
	void Start () {
		_startPosition = transform.position;
		_startRotation = transform.localRotation;
		
		StartFloat();
		//StartRotate();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void StartFloat()
	{
		Vector3 _offsetPosition = new Vector3(Random.Range(-_floatAmount.x, _floatAmount.x), 
										Random.Range(-_floatAmount.y, _floatAmount.y),
										Random.Range(-_floatAmount.z, _floatAmount.z));
		
		_newPosition = _startPosition+_offsetPosition;

		iTween.MoveTo(gameObject, iTween.Hash("position", _newPosition, "speed", _speedFloat, "easeType", _easeTypeFloat,
										"onComplete", "StartFloat", "onCompleteTarget", gameObject));
	}
	
	private void StartRotate()
	{
		Vector3 _offsetRotation = new Vector3(Random.Range(-_rotationAmount.x, _rotationAmount.x),
										Random.Range(-_rotationAmount.y, _rotationAmount.y),
										Random.Range(-_rotationAmount.z, _rotationAmount.z));
		
		_newRotation = new Vector3(_startPosition.x+_offsetRotation.x,
									_startPosition.y+_offsetRotation.y,
									_startPosition.z+_offsetRotation.z);
		
		iTween.RotateTo(gameObject, iTween.Hash("rotation", _newRotation, "speed", _speedRotate, "easeType", _easeTypeRotate,
										"onComplete", "StartRotate", "onCompleteTarget", gameObject));
	}
}
