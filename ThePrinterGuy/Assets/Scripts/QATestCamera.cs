using UnityEngine;
using System.Collections;

public class QATestCamera : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private iTween.EaseType easeType = iTween.EaseType.easeOutBack;
    [SerializeField]
    private float moveTime = 0.5f;
    #endregion
	

    #region Privates
    private bool MoveInProcess = false;
	private GUIGameCamera GUIList;
	[SerializeField]
	private int CurrLocation = 1;
	
	public delegate void OnBarometerBreakAction();
	public static event OnBarometerBreakAction OnBarometerBreak;
	
	public delegate void OnPaperAction();
	public static event OnPaperAction OnPaper;
	
	public delegate void OnUranRodsAction();
	public static event OnUranRodsAction OnUranRods;
	
	public delegate void OnInkAction();
	public static event OnInkAction OnInk;
	
	public delegate void OnInkOffAction();
	public static event OnInkOffAction OnInkOff;
    #endregion

    #region OnEnableOnDisable
    void OnEnable()
    {
		GUIList = GameObject.Find("GUI List").GetComponent<GUIGameCamera>();
		InkController.OnInkInsertedSuccess += ScoreIncreaseWithSwipeDisabled;
		PaperInsertion.OnCorrectPaperInserted += ScoreIncrease;
		Barometer.OnBarometerFixed += ScoreIncrease;
		UraniumRods.OnRodHammered += ScoreIncrease;
        GestureManager.OnSwipeRight += CameraRotationLeft;
        GestureManager.OnSwipeLeft += CameraRotationRight;
		StartCoroutine_Auto(StartFirstTask());
    }

    void OnDisable()
    {
		InkController.OnInkInsertedSuccess -= ScoreIncreaseWithSwipeDisabled;
		PaperInsertion.OnCorrectPaperInserted -= ScoreIncrease;
		Barometer.OnBarometerFixed -= ScoreIncrease;
		UraniumRods.OnRodHammered -= ScoreIncrease;
        GestureManager.OnSwipeRight -= CameraRotationLeft;
        GestureManager.OnSwipeLeft -= CameraRotationRight;
    }
    #endregion

    #region Methods
	public void ScoreIncrease()
	{
		StartCoroutine_Auto(GOCRAZY());
	}
	
	public void ScoreIncreaseWithSwipeDisabled()
	{
		StartCoroutine_Auto(GOCRAZY());
		GestureManager.OnSwipeRight += CameraRotationLeft;
        GestureManager.OnSwipeLeft += CameraRotationRight;
		
	}
	
	IEnumerator GOCRAZY()
	{
		GUIList.IncreaseScore(100, "SUPER!");
		yield return new WaitForSeconds(0.2f);
		GUIList.IncreaseScore(0, "MEGA!");
		yield return new WaitForSeconds(0.2f);
		GUIList.IncreaseScore(0, "AWESOME!");
	}
	
	IEnumerator StartFirstTask()
	{
		yield return new WaitForSeconds(0.2f);
		if(OnBarometerBreak != null)
			OnBarometerBreak();	
	}
	
    public void CameraRotationLeft(GameObject go)
    {
        if(MoveInProcess == false)
        {
			if(CurrLocation == 1)
			{
				CurrLocation = 0;
				if(OnPaper != null)
					OnPaper();
            	MoveCamera(-13, 0, 0);
			}
			else if(CurrLocation == 2)
			{
				CurrLocation = 1;
				if(OnBarometerBreak != null)
					OnBarometerBreak();
            	MoveCamera(-13, 0, 0);
			}
			/*else if(CurrLocation == 3)
			{
				CurrLocation = 2;
				if(OnInkOff != null)
					OnInkOff();
				if(OnUranRods != null)
					OnUranRods();
            	MoveCamera(-23, 0, 0);	
			}*/
        }
    }

    public void CameraRotationRight(GameObject go)
    {
		if(MoveInProcess == false)
        {
        	if(CurrLocation == 0)
			{
				CurrLocation = 1;
				if(OnBarometerBreak != null)
					OnBarometerBreak();
            	MoveCamera(13, 0, 0);
			}
			else if(CurrLocation == 1)
			{
				CurrLocation = 2;
				if(OnUranRods != null)
					OnUranRods();
            	MoveCamera(13, 0, 0);
			}
			/*else if(CurrLocation == 2)
			{
				CurrLocation = 3;
				if(OnInk != null)
				{
					OnInk();
					GestureManager.OnSwipeRight -= CameraRotationLeft;
        			GestureManager.OnSwipeLeft -= CameraRotationRight;
				}
				MoveCamera(23, 0, 0);
			}*/
		}
    }

    public void MoveCamera(float moveX, float moveY, float moveZ)
    {
            MoveInProcess = true;
            iTween.MoveAdd(Camera.main.gameObject, iTween.Hash("x", moveX, "y", moveY, "z", moveZ, "time", moveTime, "easeType", easeType, "onComplete", "CameraRotationStopped", "onCompleteTarget", gameObject));
    }

    public void CameraRotationStopped()
    {
        MoveInProcess = false;
    }
    #endregion
}