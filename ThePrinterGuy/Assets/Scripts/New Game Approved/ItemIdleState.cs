using UnityEngine;
using System.Collections;

public class ItemIdleState : MonoBehaviour {
	
	[SerializeField]
	private Vector3 _floatAmount;
	[SerializeField]
	private float _speed;
	[SerializeField]
	private iTween.EaseType _easeType;
	
	private Vector3 _startPosition;
	private Vector3 _newPosition;
	
	// Use this for initialization
	void Start () {
		_startPosition = transform.position;
		
		StartIdle();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void StartIdle()
	{
		Vector3 _offset = new Vector3(Random.Range(-_floatAmount.x, _floatAmount.x), 
										Random.Range(-_floatAmount.y, _floatAmount.y),
										Random.Range(-_floatAmount.z, _floatAmount.z));
										
		_newPosition = _startPosition+_offset;
		iTween.MoveTo(gameObject, iTween.Hash("position", _newPosition, "speed", _speed, "easeType", _easeType,
										"onComplete", "StartIdle", "onCompleteTarget", gameObject));
	}
}
