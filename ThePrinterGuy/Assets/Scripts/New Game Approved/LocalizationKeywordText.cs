using UnityEngine;
using System.Collections;

public class LocalizationKeywordText : MonoBehaviour {

    [SerializeField]
    private string _stringText = "";

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void LocalizeText()
    {
        gameObject.GetComponent<TextMesh>().text = LocalizationText.GetText(_stringText);
    }
	
	public void SetStringText(string LocaleText)
	{
		_stringText = LocaleText;
	}
}
