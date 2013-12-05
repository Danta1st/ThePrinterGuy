using UnityEngine;
using System.Collections;

public class PointerRotation : MonoBehaviour {
	
	[SerializeField]
	private Vector3 _rotationAmmount = new Vector3(30f,0f,0f);
	[SerializeField]
	private float _rotationTime = 0.24f;
	
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
			BeatController.OnAll3Beats += RotatePointer;
		}
		else if(_is4Beats)
		{
			BeatController.OnAll4Beats += RotatePointer;
		}
		else if(_is6Beats)
		{
			BeatController.OnAll6Beats += RotatePointer;
		}
		else if(_is8Beats)
		{
			BeatController.OnAll8Beats += RotatePointer;
		}
		else
		{
			Debug.Log("You need to choose beat type for Pointer Rotation script");	
		}
			
	}
	void OnDisable()
	{
		if(_is3Beats){
			BeatController.OnAll3Beats -= RotatePointer;
		}
		else if(_is4Beats)
		{
			BeatController.OnAll4Beats -= RotatePointer;
		}
		else if(_is6Beats)
		{
			BeatController.OnAll6Beats -= RotatePointer;
		}
		else if(_is8Beats)
		{
			BeatController.OnAll8Beats -= RotatePointer;
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
	
	private void RotatePointer()
	{
		iTween.RotateAdd(gameObject, _rotationAmmount, _rotationTime);
	}
}
