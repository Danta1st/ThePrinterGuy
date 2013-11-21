﻿using UnityEngine;
using System.Collections;


public class BpmCube : MonoBehaviour {
	
	public AudioClip _audioTo;
	public AudioClip _audioFrom;
	
	private Vector3 _beginPos;
	private Vector3 _targetPos;
	private AudioSource _aSource;
	
	void Start()
	{
		_aSource = gameObject.AddComponent<AudioSource>();
		_beginPos = transform.position;
		_targetPos = transform.position + Vector3.right;
	}

	void OnEnable()
	{
		BeatController.OnBeat8th5 += Move;
		BeatController.OnBeat8th7 += MoveBack;
	}
	void OnDisable()
	{
		BeatController.OnBeat8th5 -= Move;
		BeatController.OnBeat8th7 -= MoveBack;
	}
	
	void Move()
	{
		//Insert sound
		//_aSource.PlayOneShot(_audio);
		audio.PlayOneShot(_audioTo);
		
		Debug.Log("To: "+AudioSettings.dspTime);
		iTween.MoveTo(gameObject, iTween.Hash("position", _targetPos, "time", 0.5f));
	}
	
	void MoveBack()
	{
		//Insert Sound
		audio.PlayOneShot(_audioFrom);
			
		Debug.Log("From: "+AudioSettings.dspTime);
		iTween.MoveTo(gameObject, iTween.Hash("position", _beginPos, "time", 0.5f));
	}
}
