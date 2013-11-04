using UnityEngine;
using System.Collections;

public class PaperTray : MonoBehaviour 
{
	#region Public variables
	[SerializeField]
	private int _paperlimit = 500;
	[SerializeField]
	private int _currentPapercount = 500;
	[SerializeField]
	private float _stressDecreasePerSecond = 1;
	[SerializeField]
	private float _stressIncreasePerFill = 10;
	[SerializeField]
	private float _stressThresholdPerPenalty = 50;
	[SerializeField]
	private int _paperStackSize = 8;
	#endregion
	
	
	#region Private variables
	private float _printerStress = 0;
	#endregion
	
	
	#region Delegates & Events
	public delegate void EmptyTrayAction(GameObject go);
	public static event EmptyTrayAction OnEmptyTray;
	
	public delegate void TrayRefilledAction(GameObject go);
	public static event TrayRefilledAction OnTrayRefilledFromEmpty;
	
	public delegate void PaperTrayPenalty(GameObject go);
	public static event PaperTrayPenalty OnPaperTrayPenalty;
	#endregion
	
	
	#region Monobehaviour Functions
	public void OnEnable()
	{
		PrinterManager.OnPagePrinted += PagePrinted;
		ZoomHandler.OnTray += TrayFocus;
		ZoomHandler.OnFreeroam += FreeRoamMode;
	}
	public void OnDisable()
	{
		PrinterManager.OnPagePrinted -= PagePrinted;
		ZoomHandler.OnTray -= TrayFocus;
		ZoomHandler.OnFreeroam -= FreeRoamMode;
		GestureManager.OnTap -= TrayClicked;
	}
	
	void Start () 
	{
		StartCoroutine_Auto(DecreasePrinterStress());
	}
	#endregion
	
	
	#region Class methods
	public void TrayFocus()
	{
		GestureManager.OnTap += TrayClicked;
	}
	public void FreeRoamMode()
	{
		GestureManager.OnTap -= TrayClicked;
	}
	
	public void TrayClicked(GameObject myGO, Vector2 pos)
	{
		if(myGO != this.gameObject)
		{
			return;
		}
		
		RefillPaper(_paperStackSize);
	}
	
	public void PagePrinted(GameObject myPrinterGO)
	{
		if(myPrinterGO != gameObject.transform.root.gameObject)
			return;
		if(_currentPapercount > 0)
		{
			_currentPapercount--;
		}
		else
		{
			if(OnEmptyTray != null)
				OnEmptyTray(gameObject.transform.root.gameObject);	
		}
	}
	
	public void RefillPaper(int amountOfPapers)
	{
		if(_currentPapercount == 0)
		{
			_currentPapercount += amountOfPapers;
			if(OnTrayRefilledFromEmpty != null)
				OnTrayRefilledFromEmpty(gameObject.transform.root.gameObject);
		}
		else if((_currentPapercount + amountOfPapers) <= _paperlimit)
		{
			_currentPapercount += amountOfPapers;	
		}
		else
		{
			_currentPapercount = _paperlimit;
			// TODO: Error return? Skal der bare fyldes helt op hver gang?
		}
		_printerStress += _stressIncreasePerFill;
		if(_printerStress >= _stressThresholdPerPenalty)
		{
			OnPaperTrayPenalty(this.gameObject.transform.root.gameObject);
			_printerStress -= _stressThresholdPerPenalty;
		}
	}
	
	public int GetCurrentPapercount()
	{
		return _currentPapercount;	
	}
	
	IEnumerator DecreasePrinterStress()
	{
		while(true)
		{
			if(_printerStress != 0)
			{
				_printerStress -= _stressDecreasePerSecond;
			}
			yield return new WaitForSeconds(1);
		}
	}
	#endregion
}
