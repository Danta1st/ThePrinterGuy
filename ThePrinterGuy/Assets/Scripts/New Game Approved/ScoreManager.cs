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

    public delegate void TaskRed();
    public static event TaskRed OnTaskRed;

    public delegate void TaskYellow();
    public static event TaskYellow OnTaskYellow;

    public delegate void TaskGreen();
    public static event TaskGreen OnTaskGreen;

    public delegate void TaskZone();
    public static event TaskZone OnTaskZone;
	#endregion
	
	#region Unity Methods
	void Start () 
	{
		guiGameCameraScript = GameObject.Find("GUI List").GetComponent<GUIGameCamera>();
	}

    void Update()
    {
        if(OnTaskRed != null)
        {
            OnTaskRed();
        }
    }
	
	void OnEnable()
	{
		Ink.OnCorrectInkInserted += InkSuccess;
		PaperInsertion.OnCorrectPaperInserted += PaperSuccess;
		Barometer.OnBarometerFixed += BarometerSuccess;
		UraniumRods.OnRodHammered += RodSuccess;
	}
	
	void OnDisable()
	{
		Ink.OnCorrectInkInserted -= InkSuccess;
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

		colorHit = guiGameCameraScript.GetZone();

		switch (colorHit)
		{
			case 0:
                Feedback.Clear();
                Feedback.Add("Not in a Zone!");

                if(OnTaskZone != null)
                {
                    OnTaskZone();
                }

				break;

            case 1:

                Feedback.Clear();
                Feedback.Add("Red Zone!");

                if(OnTaskRed != null)
                {
                    OnTaskRed();
                }

                break;

			case 2:
                Feedback.Clear();
                Feedback.Add("Yellow Zone!");
				amount = amount * YellowZoneModifier;

                if(OnTaskYellow != null)
                {
                    OnTaskYellow();
                }

				break;

			case 3:
                Feedback.Clear();
                Feedback.Add("Green Zone!");
                Feedback.Add("Perfect!");
				amount = amount * GreenZoneModifier;

                if(OnTaskGreen != null)
                {
                    OnTaskGreen();
                }

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
