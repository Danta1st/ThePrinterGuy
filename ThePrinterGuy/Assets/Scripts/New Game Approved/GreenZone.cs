using UnityEngine;
using System.Collections;

public class GreenZone : MonoBehaviour {
	
	[SerializeField] private Texture2D _greenOn;
	[SerializeField] private Texture2D _greenOff;
    [SerializeField] private GameObject _greenInnerGlow;
    [SerializeField] private GameObject _yellowInnerGlow;
    [SerializeField] private GameObject _ring;
    [SerializeField] private Texture2D _ringLit;
    [SerializeField] private Texture2D _ringUnlit;
		
	// Use this for initialization
	void Start () {
	    _greenInnerGlow.renderer.enabled = false;
		_yellowInnerGlow.renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		_greenInnerGlow.transform.Rotate(0, 0, Time.deltaTime * 40.0f);
		_yellowInnerGlow.transform.Rotate(0, 0, Time.deltaTime * 30.0f);
	}
	
	public void GreenOn()
	{
		YellowOff();
		renderer.material.mainTexture = _greenOn;
        _ring.renderer.material.mainTexture = _ringLit;
        _greenInnerGlow.renderer.enabled = true;
	}
	
	public void GreenOff()
	{
		YellowOff();
		renderer.material.mainTexture = _greenOff;
        _ring.renderer.material.mainTexture = _ringUnlit;
        _greenInnerGlow.renderer.enabled = false;
	}
	
	public void YellowOn()
	{
		renderer.material.mainTexture = _greenOn;
        _ring.renderer.material.mainTexture = _ringLit;
        _yellowInnerGlow.renderer.enabled = true;
	}
	public void YellowOff()
	{
		renderer.material.mainTexture = _greenOff;
        _ring.renderer.material.mainTexture = _ringUnlit;
        _yellowInnerGlow.renderer.enabled = false;
	}
	
}
