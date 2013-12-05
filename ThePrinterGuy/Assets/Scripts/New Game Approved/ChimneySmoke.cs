using UnityEngine;
using System.Collections;

public class ChimneySmoke : MonoBehaviour {
	
	[SerializeField]
	private bool _is3Beats = false;
	[SerializeField]
	private bool _is4Beats = true;
	[SerializeField]
	private bool _is6Beats = false;
	[SerializeField]
	private bool _is8Beats = false;
	
	
	void OnEnable()
	{
		if(_is3Beats){
			BeatController.OnAll3Beats += ParticleSpawn;
		}
		else if(_is4Beats)
		{
			BeatController.OnAll4Beats += ParticleSpawn;
		}
		else if(_is6Beats)
		{
			BeatController.OnAll6Beats += ParticleSpawn;
		}
		else if(_is8Beats)
		{
			BeatController.OnAll8Beats += ParticleSpawn;
		}
		else
		{
			Debug.Log("You need to choose beat type for Pointer Rotation script");	
		}
			
	}
	void OnDisable()
	{
		if(_is3Beats){
			BeatController.OnAll3Beats -= ParticleSpawn;
		}
		else if(_is4Beats)
		{
			BeatController.OnAll4Beats -= ParticleSpawn;
		}
		else if(_is6Beats)
		{
			BeatController.OnAll6Beats -= ParticleSpawn;
		}
		else if(_is8Beats)
		{
			BeatController.OnAll8Beats -= ParticleSpawn;
		}
		else
		{
			Debug.Log("Something wrong with Rotation pointer script");	
		}
	}
		
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void ParticleSpawn()
	{
		gameObject.particleSystem.Play();
	}
}
