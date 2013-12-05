using UnityEngine;
using System.Collections;

public class CreditsMovie : MonoBehaviour
{	
	[SerializeField] private float waitTime = 1.0f;
	
	void Start()
	{
		StartCoroutine(Begin());
	}
	
	IEnumerator Begin()
	{	
		//Wait before beginning playback
		yield return new WaitForSeconds (waitTime);	
		
		#if UNITY_ANDROID
		//Play Credits and allow players to skip
		Handheld.PlayFullScreenMovie("Credits.mp4", Color.clear, FullScreenMovieControlMode.CancelOnInput, 
										FullScreenMovieScalingMode.Fill);
		#endif
				
		//Load the main lobby
		LoadingScreen.Load(1, true);		
	}
}
