using UnityEngine;
using System.Collections;

public class TempoManager : MonoBehaviour {
	
	#region Delegates and Events
	public delegate void InkTempo(float ms);
	public static event InkTempo OnInkTempo;
	
	public delegate void PaperTempo(float ms);
	public static event PaperTempo OnPaperTempo;
	
	public delegate void UraniumRodTempo(float ms);
	public static event UraniumRodTempo OnUraniumRodTempo;
	
	public delegate void BarometerTempo(float ms);
	public static event BarometerTempo OnBarometerTempo;
	#endregion
	
	#region SerializeFields
	[SerializeField]
	private float _inkMs;
	[SerializeField]
	private float _paperMs;
	[SerializeField]
	private float _uraniumRodMs;
	[SerializeField]
	private float _barometerMs;
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
		
//		if(_inkTimer.)
//		{
//			
//		}
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
}












