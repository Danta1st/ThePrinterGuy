using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	[SerializeField] private GuiTextures guiTextures;
	
	void Start()
	{
		StartCoroutine(Begin());
	}
	
	IEnumerator Begin()
	{	
//		loading
		yield return new WaitForSeconds (1.0f);	
		
//		fadeTo green
		iTween.FadeTo(guiTextures.loading, 0.0f, 1.0f);
		
		//Wait for fade
		yield return new WaitForSeconds (1.0f);
		
		//Play WhyNotJingle logo splash screen
		Handheld.PlayFullScreenMovie("WhyNotJingle.mp4", Color.clear, FullScreenMovieControlMode.Hidden, 
										FullScreenMovieScalingMode.Fill);
		
		yield return new WaitForSeconds (1.0f);
		
//		FadeTo Dadiu
		iTween.FadeTo(guiTextures.dadiu, 1.0f, 1.0f);
		
//		Load Lobby		
		yield return new WaitForSeconds (2.0f);
		
		//Fade Dadiu out
		iTween.FadeTo(guiTextures.dadiu, 0.0f, 1.0f);
		
		yield return new WaitForSeconds (1.0f);
		
		//TODO: Load the main lobby
		//LoadingScreen.Load(0, true);
		
	}
	
	
	[System.Serializable]
	public class GuiTextures
	{
		public GameObject loading;
		public GameObject background;
		public GameObject dadiu;		
	}
}
