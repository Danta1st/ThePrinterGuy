using UnityEngine;
using System.Collections;

public class PrinterManager : MonoBehaviour 
{
	#region Public variables
	[SerializeField]
	private float _timeToPrintPage = 0.2f;
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
	private PaperTray paperTray;
	private int PaperTrayPenalties;
	#endregion
	
	#region Delegates & Events
	public delegate void PagePrinted(GameObject go);
	public static event PagePrinted OnPagePrinted;
	
	public delegate void PrinterBrokenAction(GameObject go);
	public static event PrinterBrokenAction OnPrinterBroken;
	
	public delegate void PrinterFixedAction(GameObject go);
	public static event PrinterFixedAction OnPrinterFixed;
	#endregion
	
	#region Unity methods
	void Start () 
	{
		StartPrinter();
	}
	
	void Update () 
	{
		
	}
	#endregion
	
	#region Public methods
	public void OnEnable()
	{
		PaperJam.OnJam += PrinterBroken;
		PaperJam.OnUnjammed += PrinterFixed;
		PaperTray.OnEmptyTray += PrinterBroken;
		PaperTray.OnTrayRefilledFromEmpty += PrinterFixed;
		InkCartridge.OnInkCartridgeError += PrinterBroken;
		InkCartridge.OnInkCartridgeRefilledFromEmpty += PrinterFixed;
		PaperTray.OnPaperTrayPenalty += OnPaperTrayPenalty;
		Popout.OnCylinderHammeredIn += OnPaperTrayPenaltyRemoved;
	}
	public void OnDisable()
	{
		PaperJam.OnJam -= PrinterBroken;
		PaperJam.OnUnjammed -= PrinterFixed;
		PaperTray.OnEmptyTray -= PrinterBroken;
		PaperTray.OnTrayRefilledFromEmpty -= PrinterFixed;
		PaperTray.OnPaperTrayPenalty -= OnPaperTrayPenalty;
		Popout.OnCylinderHammeredIn -= OnPaperTrayPenaltyRemoved;
		InkCartridge.OnInkCartridgeError -= PrinterBroken;
		InkCartridge.OnInkCartridgeRefilledFromEmpty -= PrinterFixed;
	}
	
	public void StartPrinter()
	{
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
		if(OnPrinterBroken != null)
		{	
			OnPrinterBroken(gameObject);
		}
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
			if(OnPrinterFixed != null)
				OnPrinterFixed(gameObject);
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
	#endregion
	
	#region Private methods
	private IEnumerator Print()
	{
		while(true)
		{
			if(!_isBroken && Time.deltaTime != 0)
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
