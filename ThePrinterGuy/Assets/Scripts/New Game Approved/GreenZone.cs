using UnityEngine;
using System.Collections;

public class GreenZone : MonoBehaviour {
	
	[SerializeField]
	private Texture2D _greenOn;
	[SerializeField]
	private Texture2D _greenOff;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "ActionNode")
		{
			renderer.material.mainTexture = _greenOn;
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "ActionNode")
		{
			renderer.material.mainTexture = _greenOff;
		}
	}
	
	public void GreenOn()
	{
		renderer.material.mainTexture = _greenOn;
	}
	
	public void GreenOff()
	{
		renderer.material.mainTexture = _greenOff;
	}
}
