using UnityEngine;
using System.Collections;

public class LoadingWheel : MonoBehaviour 
{
	[SerializeField]
	private bool loading = false;
	[SerializeField]
	private Texture loadingTexture;
	[SerializeField]
	private float size = 70.0f;
	[SerializeField]
	private float rotSpeed = 300.0f;
	
	private float rotAngle = 0.0f;
	// Use this for initialization
	void Update () 
	{
		if(loading)
		{
			rotAngle += rotSpeed * Time.deltaTime;
		}	
	}

	void OnGUI ()
	{
		if(loading)
		{
			Vector2 pivot = new Vector2(size / 2, size / 2); //new Vector2(Screen.width/2, Screen.height/2);
			GUIUtility.RotateAroundPivot(rotAngle % 360, pivot);
			GUI.DrawTexture(new Rect (0 , 0, size, size), loadingTexture); 
			//GUI.DrawTexture(new Rect ((Screen.width - size)/2 , (Screen.height - size)/2, size, size), loadingTexture); 
		}
	}
	
	public void setLoading(bool isloading)
	{
		loading = isloading;
	}
}
