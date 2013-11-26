using UnityEngine;
using System.Collections;

public class GreenZone : MonoBehaviour {
	
	[SerializeField]
	private Texture2D _greenOn;
	[SerializeField]
	private Texture2D _greenOff;
    [SerializeField]
    private GameObject _greenInnerGlow;
    [SerializeField]
    private GameObject _ring;
    [SerializeField]
    private Texture2D _ringLit;
    [SerializeField]
    private Texture2D _ringUnlit;
	
	// Use this for initialization
	void Start () {
	    _greenInnerGlow.renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
         _greenInnerGlow.transform.Rotate(0, 0, Time.deltaTime*40);
	}

	public void GreenOn()
	{
        _greenInnerGlow.renderer.enabled = true;
		renderer.material.mainTexture = _greenOn;
        _ring.renderer.material.mainTexture = _ringLit;
	}
	
	public void GreenOff()
	{
		renderer.material.mainTexture = _greenOff;
        _ring.renderer.material.mainTexture = _ringUnlit;
        _greenInnerGlow.renderer.enabled = false;
	}
}
