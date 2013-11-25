using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour 
{
	#region Publics
	//Task points
	[SerializeField] private int InkPointsBase = 100;
	[SerializeField] private int PaperPointsBase = 100;
	[SerializeField] private int BarometerPointsBase = 100;
	[SerializeField] private int RodPointsBase = 100;
	//Zone Modifiers
	[SerializeField] private float YellowZoneModifier = 1.2f;
	[SerializeField] private float GreenZoneModifier = 1.5f;
	//StressOMeter Modifiers
	[SerializeField] private List<string> Feedback;
	[SerializeField] private ScoreMultipliers _scoreMultiplier;
	#endregion
	
	#region Privates
	private GUIGameCamera guiGameCameraScript;
	private float _multiplier;
	#endregion
	
	#region Delegates and Events
	public delegate void OnTaskCompletedAction();
	public static event OnTaskCompletedAction OnTaskCompleted;
	#endregion
	
	#region Unity Methods
	void Start () 
	{
		guiGameCameraScript = GameObject.Find("GUI List").GetComponent<GUIGameCamera>();
		_multiplier = _scoreMultiplier.slightlyHappyZoneMultiplier;
	}
	/*void Update()
	{
		if(Input.GetKeyDown(KeyCode.C))
		{
			InkSuccess();
		}
	}*/
	
	void OnEnable()
	{
		//Task Succes Subscriptions
		Ink.OnCorrectInkInserted += InkSuccess;
		PaperInsertion.OnCorrectPaperInserted += PaperSuccess;
		Barometer.OnBarometerFixed += BarometerSuccess;
		UraniumRods.OnRodHammered += RodSuccess;
		
		//StressOMeter Subscriptions
		StressOMeter.OnHappyZone += EnableHappyMultiplier;
		StressOMeter.OnZoneLeft += DisableHappyMultiplier;
	}
	
	void OnDisable()
	{
		//Task Succes Subscriptions
		Ink.OnCorrectInkInserted -= InkSuccess;
		PaperInsertion.OnCorrectPaperInserted -= PaperSuccess;
		Barometer.OnBarometerFixed -= BarometerSuccess;
		UraniumRods.OnRodHammered -= RodSuccess;
	
		//StressOMeter Subscriptions
		StressOMeter.OnAngryZone -= EnableHappyMultiplier;
		StressOMeter.OnZoneLeft -= DisableHappyMultiplier;
	}
	#endregion
	
	#region delegate methods
	private void EnableHappyMultiplier()
	{
		_multiplier = _scoreMultiplier.happyZoneMultiplier;
	}
	
	private void DisableHappyMultiplier()
	{
		_multiplier = _scoreMultiplier.slightlyHappyZoneMultiplier;
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
		int popupSize = 1;
		
		if(OnTaskCompleted != null)
			OnTaskCompleted();

		colorHit = guiGameCameraScript.GetZone();

		switch (colorHit)
		{
			case 0:
                Feedback.Clear();
                Feedback.Add("Not bad!");
				popupSize = 3;
				break;
            case 1:
                Feedback.Clear();
                Feedback.Add("Good!");
				popupSize = 3;
                break;
			case 2:
                Feedback.Clear();
                Feedback.Add("Great!");
				popupSize = 2;
				amount = amount * YellowZoneModifier * _multiplier;
				break;
			case 3:
                Feedback.Clear();
                Feedback.Add("Perfect!");
				popupSize = 1;
				amount = amount * GreenZoneModifier * _multiplier;
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
				
				guiGameCameraScript.IncreaseScore(amount);
				
				if(popupSize == 1)
				{
					guiGameCameraScript.PopupTextBig(s);
				}
				else if(popupSize == 2)
				{
					guiGameCameraScript.PopupTextMedium(s);
				}
				else if(popupSize == 3)
				{
					guiGameCameraScript.PopupTextSmall(s);
				}
				else
				{
					guiGameCameraScript.PopupTextSmall(s);
				}
				
				pointsGranted = true;
				yield return new WaitForSeconds(0.2f);
			}
		}
	}
	#endregion
}

[System.Serializable]
public class ScoreMultipliers
{
	public float happyZoneMultiplier = 1.2f;
	public float slightlyAngryZoneMultiplier = 1f;
	public float slightlyHappyZoneMultiplier = 1f;
	public float angryZoneMultiplier = 0.8f;
}
