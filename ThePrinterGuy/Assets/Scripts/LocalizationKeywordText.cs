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
        gameObject.GetComponent<TextMesh>().text = _stringText;
    }
}
