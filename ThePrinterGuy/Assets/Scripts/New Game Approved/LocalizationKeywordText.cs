using UnityEngine;
using System.Collections;

public class LocalizationKeywordText : MonoBehaviour {

    [SerializeField] private string _stringText = "";
	
    public void LocalizeText()
    {
		if(guiText != null)
			guiText.text = LocalizationText.GetText(_stringText);
		else
        	gameObject.GetComponent<TextMesh>().text = LocalizationText.GetText(_stringText);
    }
	
	public void SetStringText(string LocaleText)
	{
		_stringText = LocaleText;
	}
	
	public void setText(string text)
	{
		if(guiText != null)
			guiText.text = text;
		else
			gameObject.GetComponent<TextMesh>().text = text;
		
	}
}
