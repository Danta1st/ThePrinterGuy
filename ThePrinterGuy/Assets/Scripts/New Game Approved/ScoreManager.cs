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
	private float _multiplier;
	//Script references
	private GUIGameCamera guiGameCameraScript;
	private StressOMeter _stressOmeterReference;
	#endregion
	
	#region Delegates and Events
	public delegate void OnTaskCompletedAction();
	public static event OnTaskCompletedAction OnTaskCompleted;
	#endregion
	
	#region Unity Methods
	void Awake()
	{
		if(GameObject.FindGameObjectWithTag("StressOmeter") != null)
			_stressOmeterReference = GameObject.FindGameObjectWithTag("StressOmeter").GetComponent<StressOMeter>();
	}
	
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
    public ScoreMultipliers GetScoreMultipliers()
    {
        return _scoreMultiplier;
    }

    public float GetGreenZoneModifier()
    {
        return GreenZoneModifier;
    }

    public float GetPaperPointsBase()
    {
        return PaperPointsBase;
    }

    public float GetBarometerPointsBase()
    {
        return BarometerPointsBase;
    }

    public float GetInkPointsBase()
    {
        return InkPointsBase;
    }

    public float GetRodPointsBase()
    {
        return RodPointsBase;
    }

	public void InkSuccess()
	{
		StartCoroutine_Auto(AwardPoints(InkPointsBase));
	}
	public void PaperSuccess()
	{
		StartCoroutine_Auto(AwardPoints(PaperPointsBase));
	}
	public void BarometerSuccess()
	{
		StartCoroutine_Auto(AwardPoints(BarometerPointsBase));
	}
	public void RodSuccess()
	{
		StartCoroutine_Auto(AwardPoints(RodPointsBase));
	}
	
	IEnumerator AwardPoints(float taskValue)
	{
		float calculatedValue = 0;
		int colorHit = 0;
		bool pointsGranted = false;
		int popupSize = 1;
        int maxAmount = 0;
        ScoreMultipliers sm = new ScoreMultipliers();
		
		if(OnTaskCompleted != null)
			OnTaskCompleted();

		colorHit = guiGameCameraScript.GetZone();

		switch (colorHit)
		{
			case 0:
                Feedback.Clear();
                Feedback.Add("TO EARLY!");
				popupSize = 3;
				_stressOmeterReference.ReductPointsFailed();
				break;
            case 1:
                Feedback.Clear();
                Feedback.Add("GOOD!");
				popupSize = 3;
                break;
			case 2:
                Feedback.Clear();
                Feedback.Add("GREAT!");
				popupSize = 2;
				calculatedValue = taskValue * YellowZoneModifier * _multiplier;
				break;
			case 3:
                Feedback.Clear();
                Feedback.Add("PERFECT!");
				popupSize = 1;
				calculatedValue = taskValue * GreenZoneModifier * _multiplier;
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
					taskValue = 0;	
				}
				
				guiGameCameraScript.IncreaseScore(calculatedValue);
				
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
