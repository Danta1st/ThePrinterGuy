using UnityEngine;
using System.Collections;

public class SmokeTrail : MonoBehaviour {
	
	[SerializeField]
	private int _pathSize;
	[SerializeField]
	private GameObject _smokePrefab;
	[SerializeField]
	private GameObject[] _smokeTrails;
	[SerializeField]
	private Vector3 _offset;
	[SerializeField]
	private float _speed;
	[SerializeField]
	private float _spawnTimer;
	[SerializeField]
	private iTween.EaseType _easeType;
	
	private Vector3[] _path;
	private GameObject _smokeObject = null;
	private float _timer;
	private bool _isFade = false;
	
	// Use this for initialization
	void Start () {
		_path = new Vector3[_pathSize];
	}
	
	// Update is called once per frame
	void Update () {
		_timer += Time.deltaTime;
	
		if(_timer > _spawnTimer)
		{
			_timer = 0.0f;
			SpawnTrail();
		}
	}
	
	private void SpawnTrail()
	{
		int i = 0;
		foreach(GameObject _trail in _smokeTrails)
		{
			Vector3 _randomOffset = new Vector3(Random.Range(-_offset.x, _offset.x),
												Random.Range(-_offset.y, _offset.y),
												Random.Range(-_offset.z, _offset.z));
			_path[i] = _smokeTrails[i].transform.position+_randomOffset;
			i++;
		}
		
		_smokeObject = (GameObject)Instantiate(_smokePrefab, transform.position, Quaternion.identity);
		iTween.MoveTo(_smokeObject, iTween.Hash("path", _path, "speed", _speed, "easeType", _easeType,
												"onComplete", "Cooldown", "onCompleteTarget", gameObject, "onCompleteParams", _smokeObject));
	}
	
	private void Cooldown(GameObject _go)
	{
		//iTween.FadeTo(_go, iTween.Hash("alpha", 255f, "speed", _speed));
		StartCoroutine(DestroyTrail(_go));
	}
	
	private IEnumerator DestroyTrail(GameObject _go)
	{
		_isFade = true;
		float _step = 0.0f;
		while(_isFade)
		{
			_step += Time.deltaTime;
			
			Color c = _go.renderer.material.GetColor("_TintColor");
			c.a = Mathf.Lerp(0.5f, 0.0f, _step);
			_go.renderer.material.SetColor("_TintColor", c);
			
			if(_step >= 1.0f)
			{
				_isFade = false;
			}
			
			yield return new WaitForSeconds(Time.deltaTime);
		}
		
		yield return new WaitForSeconds(10f);
		
		if(_go != null)
		{
			Destroy(_go);
			_go = null; 
		}
	}
}
