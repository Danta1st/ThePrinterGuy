using UnityEngine;
using System.Collections;

public class LightSample : MonoBehaviour
{
    private LightController _led;
	// Use this for initialization
	void Start ()
    {
	    _led = gameObject.AddComponent<LightController>();
        _led.InitializeLight(this.gameObject, true);
	}
	
	// Update is called once per frame
	void Update ()
    {
  	}
}
