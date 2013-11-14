using UnityEngine;
using System.Collections;

public class GUISpeechText : MonoBehaviour 
{
	
	private int _currentPosition = 0;
	private float _delay = 0.01f;
	private string _text = "";
	[SerializeField]
	private string[] _additionalLines;
	private float _scaleMultiplierY;
	

	// Use this for initialization
	void Start () {
		
		_scaleMultiplierY = Screen.height / 1200f;
		
		StartCoroutine("LetterWriter");
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.M))
		{
			
			WriteText("SpeechBubbleExample");
			
		}
	}
	
	void WriteText(string _str)
	{
		TextMesh _test = GameObject.Find("SpeechText").GetComponent<TextMesh>();
		
		_test.text = LocalizationText.GetText(_str);
		
		string _aText = _test.text;
		
		GetComponent<TextMesh>().fontSize = Mathf.CeilToInt(100f * _scaleMultiplierY);
		
		GetComponent<TextMesh>().text = "";
		_currentPosition = 0;
		_text = _aText;
		
	}
	
	IEnumerator LetterWriter()
	{
		
		foreach(string _s in _additionalLines)
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
