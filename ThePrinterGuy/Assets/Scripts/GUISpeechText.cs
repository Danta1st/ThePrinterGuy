﻿using UnityEngine;
using System.Collections;

public class GUISpeechText : MonoBehaviour 
{
	
	private int _currentPosition = 0;
	private float _delay = 0.01f;
	private string _text = "";
	public string[] additionalLines;
	private float _scaleMultiplierY;
	

	// Use this for initialization
	void Start () {
		
		_scaleMultiplierY = Screen.height / 1200f;
		
		StartCoroutine("Loopy");
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.M))
		{
			WriteText("This is the first line bla bla bla bla \nthis is the second line bla bla bla bla \nbla bla bla bla maybe a third?");
		}
	}
	
	void WriteText(string _aText)
	{
		GetComponent<TextMesh>().fontSize = Mathf.CeilToInt(100f * _scaleMultiplierY);
		GetComponent<TextMesh>().text = "";
		_currentPosition = 0;
		_text = _aText;
		
	}
	
	IEnumerator Loopy()
	{
		
		foreach(string _s in additionalLines)
		{
			//Debug.Log("HFRUWE!");
			_text += "\n" + _s;

			while(true)
			{
				if(_currentPosition < _text.Length)
				{
					GetComponent<TextMesh>().text += _text[_currentPosition++];
					//guiText.text += _text[_currentPosition++];
				}
				yield return new WaitForSeconds(_delay);
			}
		}
	}
}
