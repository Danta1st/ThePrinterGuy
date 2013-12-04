using UnityEngine;
using System.Collections;

public class GreenZone : MonoBehaviour {
	
	[SerializeField] private Texture2D _green;
	[SerializeField] private Texture2D _yellow;
	[SerializeField] private Texture2D _red;
//	[SerializeField] private Texture2D _greenOff;
//    [SerializeField] private GameObject _greenInnerGlow;
//    [SerializeField] private GameObject _yellowInnerGlow;
    [SerializeField] private GameObject _ring;
    [SerializeField] private Texture2D _ringLit;
    [SerializeField] private Texture2D _ringUnlit;
	
	private enum ZoneStates { Green, Yellow, Red };
	private ZoneStates zoneState = ZoneStates.Red;
	
	void Start () {
//	    _greenInnerGlow.renderer.enabled = false;
//		_yellowInnerGlow.renderer.enabled = false;
	}	
	void Update () {
//		_greenInnerGlow.transform.Rotate(0, 0, Time.deltaTime * 40.0f);
//		_yellowInnerGlow.transform.Rotate(0, 0, Time.deltaTime * 30.0f);
	}
	
	
	public void GreenOn()
	{
//		YellowOff();
		renderer.material.mainTexture = _green;
        _ring.renderer.material.mainTexture = _ringLit;
		zoneState = ZoneStates.Green;
//        _greenInnerGlow.renderer.enabled = true;
	}	
	public void GreenOff()
	{
//		YellowOff();
		renderer.material.mainTexture = _red;
        _ring.renderer.material.mainTexture = _ringUnlit;
		zoneState = ZoneStates.Red;
//        _greenInnerGlow.renderer.enabled = false;
	}
	
	
	public void YellowOn()
	{
		if(zoneState == ZoneStates.Green)
			return;
		
		renderer.material.mainTexture = _yellow;
        _ring.renderer.material.mainTexture = _ringLit;
		zoneState = ZoneStates.Yellow;

//        _yellowInnerGlow.renderer.enabled = true;
	}
	public void YellowOff()
	{
		renderer.material.mainTexture = _red;
        _ring.renderer.material.mainTexture = _ringUnlit;
		zoneState = ZoneStates.Red;
//        _yellowInnerGlow.renderer.enabled = true;
	}
	
}
