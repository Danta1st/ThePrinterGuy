using UnityEngine;
using System.Collections;

public class PrinterManager : MonoBehaviour 
{
	#region Public variables
	[SerializeField]
	private int _paperlimit = 500;
	[SerializeField]
	private float _timeToPrintPage = 0.2f;
	[SerializeField]
	private float blackPrintLifetime = 100;
	[SerializeField]
	private float blackPrintDecayRate = 1;
	[SerializeField]
	private float bluePrintLifetime = 100;
	[SerializeField]
	private float bluePrintDecayRate = 1;
	[SerializeField]
	private float redPrintLifetime = 100;
	[SerializeField]
	private float redPrintDecayRate = 1;
	[SerializeField]
	private float greenPrintLifetime = 100;
	[SerializeField]
	private float greenPrintDecayRate = 1;
	#endregion
	
	#region Private variables
	[SerializeField]
	private int _papercount = 500;
	[SerializeField]
	private bool _isBroken = false;
	
	private int _printedPapers = 0;
	private TimerUtilities BlackTimer;
	private TimerUtilities RedTimer;
	private TimerUtilities BlueTimer;
	private TimerUtilities GreenTimer;
	#endregion
	
	#region Unity methods
	void Start () 
	{
		BlackTimer = gameObject.AddComponent<TimerUtilities>();
		BlueTimer = gameObject.AddComponent<TimerUtilities>();
		RedTimer = gameObject.AddComponent<TimerUtilities>();
		GreenTimer = gameObject.AddComponent<TimerUtilities>();
		
		BlackTimer.StartTimer(blackPrintLifetime, blackPrintDecayRate);
		BlueTimer.StartTimer(bluePrintLifetime, bluePrintDecayRate);
		RedTimer.StartTimer(redPrintLifetime, redPrintDecayRate);
		GreenTimer.StartTimer(greenPrintLifetime, greenPrintDecayRate);
		StartPrinter(_timeToPrintPage);
	}
	
	void Update () 
	{
		
	}
	#endregion
	
	#region Public methods
	public void StartPrinter(float timeToPrint)
	{
		StartCoroutine_Auto(Print(timeToPrint));
	}
	
	public void RefillPaper(int amountOfPapers)
	{
		if((_papercount + amountOfPapers) <= _paperlimit)
		{
			_papercount = _papercount + amountOfPapers;	
		}
		else
		{
			_papercount = _paperlimit;
			// TODO: Error return? Skal der bare fyldes helt op hver gang?
		}
	}
	
	public void RefillPaper()
	{
		_papercount = _paperlimit;
	}
	
	public void RestockInk(Color inkColor)
	{
		if(inkColor == Color.black)	
		{
			BlackTimer.StartTimer(blackPrintLifetime, blackPrintDecayRate);
		}
		else if(inkColor == Color.blue)
		{
			BlueTimer.StartTimer(bluePrintLifetime, bluePrintDecayRate);
		}
		else if(inkColor == Color.red)
		{
			RedTimer.StartTimer(redPrintLifetime, redPrintDecayRate);
		}
		else if(inkColor == Color.green)
		{
			GreenTimer.StartTimer(greenPrintLifetime, greenPrintDecayRate);
		}
	}
	#endregion
	
	#region Private methods
	private IEnumerator Print(float timeToPrint)
	{
		while(true)
		{
			if(BlackTimer.GetTimeLeft() <= 0 || RedTimer.GetTimeLeft() <= 0 || GreenTimer.GetTimeLeft() <= 0
				|| BlueTimer.GetTimeLeft() <= 0 || _papercount == 0)
			{
				if(!_isBroken)
				{
					_isBroken = true;
					BlackTimer.PauseTimer();
					BlueTimer.PauseTimer();
					RedTimer.PauseTimer();
					GreenTimer.PauseTimer();
				}
			}
			else
			{
				if(_isBroken)
				{
					_isBroken = false;
					BlackTimer.ResumeTimer();
					BlueTimer.ResumeTimer();
					RedTimer.ResumeTimer();
					GreenTimer.ResumeTimer();
				}
			}
			if(!_isBroken && Time.deltaTime != 0)
			{
				_papercount--;
				_printedPapers++;
			}
			yield return new WaitForSeconds(timeToPrint);
		}
	}
	#endregion
}
