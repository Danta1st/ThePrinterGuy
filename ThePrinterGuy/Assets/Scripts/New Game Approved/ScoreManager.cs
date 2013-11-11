using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour 
{
	#region Publics
	[SerializeField]
	private int InkPointsBase = 100;
	[SerializeField]
	private int PaperPointsBase = 100;
	[SerializeField]
	private int BarometerPointsBase = 100;
	[SerializeField]
	private int RodPointsBase = 100;
	[SerializeField]
	private float YellowZoneModifier = 1.2f;
	[SerializeField]
	private float GreenZoneModifier = 1.5f;
	[SerializeField]
	private List<string> Feedback;
	#endregion
	
	#region Privates
	private GUIGameCamera GUIList;
	#endregion
	
	#region Delegates and Events
	public delegate void OnTaskCompleted();
	public static event OnTaskCompleted TaskCompleted;
	#endregion
	
	#region Unity Methods
	void Start () 
	{
		GUIList = GameObject.Find("GUI List").GetComponent<GUIGameCamera>();
	}
	
	void OnEnable()
	{
		InkController.OnInkInsertedSuccess += InkSuccess;
		PaperInsertion.OnCorrectPaperInserted += PaperSuccess;
		Barometer.OnBarometerFixed += BarometerSuccess;
		UraniumRods.OnRodHammered += RodSuccess;
	}
	
	void OnDisable()
	{
		InkController.OnInkInsertedSuccess -= InkSuccess;
		PaperInsertion.OnCorrectPaperInserted -= PaperSuccess;
		Barometer.OnBarometerFixed -= BarometerSuccess;
		UraniumRods.OnRodHammered -= RodSuccess;
	}
	#endregion
	
	#region Public Methods
	public void InkSuccess()
	{
		StartCoroutine_Auto(GOCRAZY(InkPointsBase));
	}
	public void PaperSuccess()
	{
		StartCoroutine_Auto(GOCRAZY(PaperPointsBase));
	}
	public void BarometerSuccess()
	{
		StartCoroutine_Auto(GOCRAZY(BarometerPointsBase));
	}
	public void RodSuccess()
	{
		StartCoroutine_Auto(GOCRAZY(RodPointsBase));
	}
	
	IEnumerator GOCRAZY(float amount)
	{
		int colorHit = 0;
		bool pointsGranted = false;
		if(TaskCompleted != null)
			TaskCompleted();
		// INDSÆT CALL FOR AT SE HVILKEN FARVE ER RAMT - SÆT colorHit til denne
		
		
		switch (colorHit)
		{
			case 0: 
				break;
			case 1:
				amount = amount * YellowZoneModifier;
				break;
			case 2:
				amount = amount * GreenZoneModifier;
				break;
			default:
				break;
		}
		
		if(Feedback.Count == 0)
		{
			Feedback.Add("Perfect!");	
		}
		else
		{
			foreach(string s in Feedback)
			{
				if(pointsGranted)
				{
					amount = 0;	
				}
				GUIList.IncreaseScore(amount, s);
				pointsGranted = true;
				yield return new WaitForSeconds(0.2f);
			}
		}
	}
	#endregion
}
