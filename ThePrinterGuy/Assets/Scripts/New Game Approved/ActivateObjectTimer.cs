using UnityEngine;
using System.Collections;

public class ActivateObjectTimer : MonoBehaviour {
	
	[SerializeField] private float _timeStamp = 0.0f;
	[SerializeField] private float _duration = 0.3f;
	
	private Vector3 _startPosition;
	private float _currentTimeStamp;
	private bool _once = false;
	
	// Use this for initialization
	void Start () {
		_startPosition = gameObject.transform.position;
		gameObject.transform.position = new Vector3(-10000, 0, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		_currentTimeStamp = Time.timeSinceLevelLoad;
		
		if(_currentTimeStamp > _timeStamp)
		{
			_once = true;
			StartCoroutine("UpdatePosition");
		}
	}
	
	private IEnumerator UpdatePosition()
	{
		gameObject.transform.position = _startPosition;
		yield return new WaitForSeconds(_duration);
		gameObject.SetActive(false);
	}
}
