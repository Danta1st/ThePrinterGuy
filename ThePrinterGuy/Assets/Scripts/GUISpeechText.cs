using UnityEngine;
using System.Collections;

public class GUISpeechText : MonoBehaviour 
{
	private TextMesh _speechTextMesh;
	private int _currentPosition = 0;
	private float _delay = 0.01f;
	private string _outputText = "";
	private string _inputText = "";

	// Use this for initialization
	void Start () {
		_speechTextMesh = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void WriteText(string _str)
	{
		_inputText = LocalizationText.GetText(_str);
		_outputText = "";
		GetComponent<TextMesh>().text = _outputText;
		_currentPosition = 0;

		StartCoroutine("LetterWriter");
	}
	
	IEnumerator LetterWriter()
	{
		while(_outputText.Length < _inputText.Length)
		{
			if(_currentPosition < _inputText.Length)
			{
				_outputText += _inputText[_currentPosition++];
				GetComponent<TextMesh>().text = _outputText;
			}
				yield return new WaitForSeconds(_delay);
		}

	}
}
