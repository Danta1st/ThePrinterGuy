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
	
	public void GreenOn()
	{
		renderer.material.mainTexture = _greenOn;
	}
	
	public void GreenOff()
	{
		renderer.material.mainTexture = _greenOff;
	}
}
