using UnityEngine;
using System.Collections;

public class LocalizationSample : MonoBehaviour {

	// Use this for initialization
	void Start () {
        LocalizationText.SetLanguage("EN");
        TextMesh gameTitle = GameObject.Find("GameTitle").GetComponent<TextMesh>();
	    gameTitle.text = LocalizationText.GetText("GameTitle");
	}
	
	// Update is called once per frame
	void Update () {

	}
}
