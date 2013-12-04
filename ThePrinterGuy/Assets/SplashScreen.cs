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
		yield return new WaitForSeconds (1.5f);
		
		#if UNITY_ANDROID
		//Play WhyNotJingle logo splash screen
		//Allow players to skip if this is not the first time the game begins
		if(PlayerPrefs.HasKey("Tutorial") && PlayerPrefs.GetString("Tutorial") == "Answered")
		{
			Handheld.PlayFullScreenMovie("IntroSplash.mp4", Color.clear, FullScreenMovieControlMode.CancelOnInput, 
											FullScreenMovieScalingMode.Fill);
		}
		else
			Handheld.PlayFullScreenMovie("IntroSplash.mp4", Color.clear, FullScreenMovieControlMode.Hidden, 
											FullScreenMovieScalingMode.Fill);
		#endif
				
		//Load the main lobby
		LoadingScreen.Load(1, true);
		
	}
	
	
	[System.Serializable]
	public class GuiTextures
	{
		public GameObject loading;
		public GameObject background;
	}
}
