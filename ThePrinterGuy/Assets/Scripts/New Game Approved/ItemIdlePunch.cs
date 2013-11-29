using UnityEngine;
using System.Collections;

public class ItemIdlePunch : MonoBehaviour {
	
	[SerializeField]
	private Vector3 _amount;
	[SerializeField]
	private float _time;
	[SerializeField]
	private iTween.EaseType _easeType;
	
	// Use this for initialization
	void Start () {
		UpdateScale();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void UpdateScale()
	{
		iTween.PunchScale(gameObject, iTween.Hash("amount", _amount, "time", _time, "easeType", _easeType,
													"onComplete", "UpdateScale", "onCompleteTarget", gameObject));
	}
}
