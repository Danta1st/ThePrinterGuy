using UnityEngine;
using System.Collections;

public class JamSmoke : MonoBehaviour {

	private ParticleSystem[] _smokes;
	
	void Awake()
	{
		_smokes = gameObject.GetComponents<ParticleSystem>();
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
		if(go.transform == gameObject.transform.root)
		{
			foreach(ParticleSystem p in _smokes)
				p.Play();
		}
	}
	
}
