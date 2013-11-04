using UnityEngine;
using System.Collections;

public class GUIPopupText : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	
	
	public void PopupText(string _str)
	{
		
		GetComponent<TextMesh>().text = _str;
		
	}
	
	private IEnumerator CleanUp() 
	{
		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
		
	}
}