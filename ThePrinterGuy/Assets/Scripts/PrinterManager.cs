using UnityEngine;
using System.Collections;

public class PrinterManager : MonoBehaviour 
{
	#region Public variables
	[SerializeField]
	private float _timeToPrintPage = 0.2f;
	
	// TODO: To be removed.
	private float blackPrintLifetime = 100;
	private float blackPrintDecayRate = 1;
	private float bluePrintLifetime = 100;
	private float bluePrintDecayRate = 1;
	private float redPrintLifetime = 100;
	private float redPrintDecayRate = 1;
	private float greenPrintLifetime = 100;
	private float greenPrintDecayRate = 1;
	
	[SerializeField]
	private float _stressDecreasePerSecond = 1;
	[SerializeField]
	private float _stressIncreasePerFill = 10;
	[SerializeField]
	private float _stressThresholdPerPenalty = 50;
	#endregion
	
	#region Private variables
	[SerializeField]
	private bool _isBroken = false;
	
	private int _printedPapers = 0;
	private int _printerproblems = 0;
	private TimerUtilities BlackTimer;
	private TimerUtilities RedTimer;
	private TimerUtilities BlueTimer;
	private TimerUtilities GreenTimer;
	private PaperTray paperTray;
	private int PaperTrayPenalties;
	#endregion
	
	#region Delegates & Events
	public delegate void PagePrinted(GameObject go);
	public static event PagePrinted OnPagePrinted;
	#endregion
	
	#region Unity methods
	void Start () 
	{
		BlackTimer = gameObject.AddComponent<TimerUtilities>();
		BlueTimer = gameObject.AddComponent<TimerUtilities>();
		RedTimer = gameObject.AddComponent<TimerUtilities>();
		GreenTimer = gameObject.AddComponent<TimerUtilities>();
		
		StartPrinter();
	}
	
	void Update () 
	{
		
	}
	#endregion
	
	#region Public methods
	public void OnEnable()
	{
		PaperTray.OnEmptyTray += PrinterBroken;
		PaperTray.OnTrayRefilledFromEmpty += PrinterFixed;
		PaperTray.OnPaperTrayPenalty += OnPaperTrayPenalty;
		Popout.OnCylinderHammeredIn += OnPaperTrayPenaltyRemoved;
	}
	public void OnDisable()
	{
		PaperTray.OnEmptyTray -= PrinterBroken;
		PaperTray.OnTrayRefilledFromEmpty -= PrinterFixed;
		PaperTray.OnPaperTrayPenalty -= OnPaperTrayPenalty;
		Popout.OnCylinderHammeredIn -= OnPaperTrayPenaltyRemoved;
	}
	
	public void StartPrinter()
	{
		BlackTimer.StartTimer(blackPrintLifetime, blackPrintDecayRate);
		BlueTimer.StartTimer(bluePrintLifetime, bluePrintDecayRate);
		RedTimer.StartTimer(redPrintLifetime, redPrintDecayRate);
		GreenTimer.StartTimer(greenPrintLifetime, greenPrintDecayRate);
		StartCoroutine_Auto(Print());
	}
	
	public void PrinterBroken(GameObject myGO)
	{
		if(myGO != this.gameObject)
		{
			return;	
		}
		_printerproblems++;
		_isBroken = true;	
	}
	public void PrinterFixed(GameObject myGO)
	{
		if(myGO != this.gameObject)
		{
			return;	
		}
		
		_printerproblems--;
		if(_printerproblems == 0)
		{
			_isBroken = false;
		}
	}
	public void OnPaperTrayPenalty(GameObject myGO)
	{
		if(myGO != this.gameObject)
		{
			return;	
		}
		
		if(PaperTrayPenalties < 3)
		{
			_timeToPrintPage = _timeToPrintPage / (PaperTrayPenalties + 1);
			PaperTrayPenalties++;
			_timeToPrintPage = _timeToPrintPage * (PaperTrayPenalties + 1);
		}
	}
	public void OnPaperTrayPenaltyRemoved(GameObject myGO)
	{
		if(myGO != this.gameObject)
		{
			return;
		}
		_timeToPrintPage = _timeToPrintPage / (PaperTrayPenalties + 1);
		PaperTrayPenalties--;
		if(PaperTrayPenalties != 0)
		{
			_timeToPrintPage = _timeToPrintPage * (PaperTrayPenalties + 1);
		}
	}
	
	/*public void RestockInk(Color inkColor)
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
	}*/
	#endregion
	
	#region Private methods
	private IEnumerator Print()
	{
		while(true)
		{
			if(BlackTimer.GetTimeLeft() <= 0 || RedTimer.GetTimeLeft() <= 0 || GreenTimer.GetTimeLeft() <= 0
				|| BlueTimer.GetTimeLeft() <= 0)
			{
				if(_isBroken)
				{
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
				}
			}
			if(_isBroken && Time.deltaTime != 0)
			{
				BlackTimer.PauseTimer();
				BlueTimer.PauseTimer();
				RedTimer.PauseTimer();
				GreenTimer.PauseTimer();	
			}
			else if(!_isBroken && Time.deltaTime != 0)
			{
				if(OnPagePrinted != null)
					OnPagePrinted(this.gameObject);
				
				_printedPapers++;
			}
			yield return new WaitForSeconds(_timeToPrintPage);
		}
	}
	#endregion
}
