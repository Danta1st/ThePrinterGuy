using UnityEngine;
using System.Collections;

public class TapToPlayMainMenu : MonoBehaviour {
	
	void OnEnable()
	{
		GestureManager.OnTap += Disable;
	}
	void OnDisable()
	{
		GestureManager.OnTap -= Disable;
	}
	
	private void Disable(GameObject go, Vector2 screenPosition)
	{
		if(go.name == gameObject.name)
		{			
			gameObject.SetActive(false);			
		}
	}
}
