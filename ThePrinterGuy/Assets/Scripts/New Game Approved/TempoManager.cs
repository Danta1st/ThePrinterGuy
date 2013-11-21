using UnityEngine;
using System.Collections;

public class TempoManager : MonoBehaviour {
	
	#region Delegates and Events
	public delegate void InkTempo();
	public static event InkTempo OnInkTempo;
	
	public delegate void PaperTempo();
	public static event PaperTempo OnPaperTempo;
	
	public delegate void UraniumRodTempo();
	public static event UraniumRodTempo OnUraniumRodTempo;
	
	public delegate void BarometerTempo();
	public static event BarometerTempo OnBarometerTempo;
	#endregion
	
	#region SerializeFields
	[SerializeField]
	private float _inkMs;
	[SerializeField]
	private float _inkTime;
	[SerializeField]
	private float _paperMs;
	[SerializeField]
	private float _paperTime;
	[SerializeField]
	private float _uraniumRodMs;
	[SerializeField]
	private float _uraniumRodTime;
	[SerializeField]
	private float _barometerMs;
	[SerializeField]
	private float _barometerTime;
	#endregion
	
	#region Private Variables
	private TimerUtilities _inkTimer;
	private TimerUtilities _paperTimer;
	private TimerUtilities _uraniumRodTimer;
	private TimerUtilities _barometerTimer;
	
	#endregion
	
	// Use this for initialization
	void Start () {
	
		_inkTimer = gameObject.AddComponent<TimerUtilities>();
		_paperTimer = gameObject.AddComponent<TimerUtilities>();
		_uraniumRodTimer = gameObject.AddComponent<TimerUtilities>();
		_barometerTimer = gameObject.AddComponent<TimerUtilities>();
		
		StartInkTimer();
		StartPaperTimer();
		StartUraniumRodTimer();
		StartBarometerTimer();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(_inkTimer.GetTimeLeft() == 0)
		{
			StartInkTimer();
			if(OnInkTempo != null)
			{
				OnInkTempo();
			}
		}
		else if(_paperTimer.GetTimeLeft() == 0)
		{
			StartPaperTimer();
			if(OnPaperTempo != null)
			{
				OnPaperTempo();
			}
		}
		else if(_uraniumRodTimer.GetTimeLeft() == 0)
		{
			StartUraniumRodTimer();
			if(OnUraniumRodTempo != null)
			{
				OnUraniumRodTempo();
			}
		}
		else if(_barometerTimer.GetTimeLeft() == 0)
		{
			StartBarometerTimer();
			if(OnBarometerTempo != null)
			{
				OnBarometerTempo();
			}
		}
	}
	
	private void StartInkTimer()
	{
		_inkTimer.StartTimer(_inkMs, false);
	}
	
	private void StartPaperTimer()
	{
		_paperTimer.StartTimer(_paperMs, false);
	}
	
	private void StartUraniumRodTimer()
	{
		_uraniumRodTimer.StartTimer(_uraniumRodMs, false);
	}
	
	private void StartBarometerTimer()
	{
		_barometerTimer.StartTimer(_uraniumRodMs, false);
	}
	
	public float GetInkMs()
	{
		return _inkMs;
	}
	
	public float GetPaperMs()
	{
		return _paperMs;
	}
	
	public float GetUraniumRodMs()
	{
		return _uraniumRodMs;
	}
	
	public float GetBarometerMs()
	{
		return _barometerMs;
	}
	
	public float GetInkTime()
	{
		return _inkTime;
	}
	
	public float GetPaperTime()
	{
		return _paperTime;
	}
	
	public float GetUraniumRodTime()
	{
		return _uraniumRodTime;
	}
	
	public float GetBarometerTime()
	{
		return _barometerTime;
	}
}












