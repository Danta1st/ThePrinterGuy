using UnityEngine;
using System.Collections;

public class JamSmoke : MonoBehaviour {

	private ParticleSystem[] _smokes;
	
	void Awake()
	{
		_smokes = gameObject.GetComponentsInChildren<ParticleSystem>();
	}
	
	void OnEnable()
	{
		PrinterManager.OnPagePrinted += Smoke;
	}
	
	void OnDisable()
	{
		PrinterManager.OnPagePrinted -= Smoke;
	}
	
	private void Smoke(GameObject go)
	{
		if(go.Equals(transform.root.gameObject))
		{
			foreach(ParticleSystem p in _smokes)
				p.Play();
		}
	}
	
}
