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
	private GUIGameCamera guiGameCameraScript;
	#endregion
	
	#region Delegates and Events
	public delegate void OnTaskCompleted();
	public static event OnTaskCompleted TaskCompleted;
	#endregion
	
	#region Unity Methods
	void Start () 
	{
		guiGameCameraScript = GameObject.Find("GUI List").GetComponent<GUIGameCamera>();
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

		colorHit = guiGameCameraScript.GetZone(); // INDSÆT CALL FOR AT SE HVILKEN FARVE ER RAMT - SÆT colorHit til denne

		switch (colorHit)
		{
			case 0:
                Feedback.Clear();
                Feedback.Add("Not in a Zone!");
				break;
            case 1:
                Feedback.Clear();
                Feedback.Add("Red Zone!");
                break;
			case 2:
                Feedback.Clear();
                Feedback.Add("Yellow Zone!");
				amount = amount * YellowZoneModifier;
				break;
			case 3:
                Feedback.Clear();
                Feedback.Add("Green Zone!");
                Feedback.Add("Fucking perfect!");
				amount = amount * GreenZoneModifier;
				break;
			default:
				break;
		}
		
		if(Feedback.Count == 0)
		{
			Feedback.Add("Something wrong with feedback!");
		}
		else
		{
			foreach(string s in Feedback.ToArray())
			{
				if(pointsGranted)
				{
					amount = 0;	
				}
				guiGameCameraScript.IncreaseScore(amount, s);
				pointsGranted = true;
				yield return new WaitForSeconds(0.2f);
			}
		}
	}
	#endregion
}
