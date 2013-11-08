using UnityEngine;
using System.Collections;

public class InkCartridge : MonoBehaviour
{
	#region Variables Editable in Editor
	#endregion
	
	#region Private variables
    private float _lifeTime = 60;
    private TimerUtilities _inkTimer;
    private bool _isInstantiated = false;
    private Vector3 _defaultScale;
    private Vector3 _defaultPosition;
	private Color _inkColor;
    private enum InkStates { Working, ErrorWait };
    private InkStates _inkState = InkStates.Working;
	private GameObject _inkColorObj;
	#endregion
	
	#region delegate declarations
    public delegate void InkCartridgeError(GameObject go);
    public static event InkCartridgeError OnInkCartridgeError;

    public delegate void InkCartridgeRefilled(GameObject go);
    public static event InkCartridgeRefilled OnInkCartridgeRefilled;
	
	public delegate void InkCartridgeRefilledFromEmpty(GameObject go);
    public static event InkCartridgeRefilledFromEmpty OnInkCartridgeRefilledFromEmpty;
	#endregion
	
    void Update()
    {
        if (_isInstantiated) {
            if (_inkTimer.GetTimeLeft() > 0 && _inkState == InkStates.Working)
            {
                UseInk();
            }
            else if (_inkTimer.GetTimeLeft() <= 0 && _inkState != InkStates.ErrorWait)
            {
                if (OnInkCartridgeError != null)
                    OnInkCartridgeError(this.gameObject.transform.root.gameObject);
				
                _inkState = InkStates.ErrorWait;
            }
        }
    }
	
	
	#region Initialization methods
    public void InitializeInkCartridge(Color color)
    {
		foreach(Transform t in gameObject.transform)
		{
			if(t.gameObject.tag == "InkColor")
			{
				_inkColorObj = t.gameObject;
			}
		}
        
		_defaultScale = _inkColorObj.transform.localScale;
        _defaultPosition = _inkColorObj.transform.localPosition;
		
		_inkColorObj.renderer.material.color = color;
		
        _inkColor = color;
        _inkTimer = gameObject.AddComponent<TimerUtilities>();
    }
	#endregion
	
	#region public methods
    public void RefillInk()
    {
        if(OnInkCartridgeRefilled != null)
            OnInkCartridgeRefilled(this.gameObject.transform.root.gameObject);
		
        _inkState = InkStates.Working;
        RescaleCartridge();
    }
	
	public void PauseTimer(GameObject go)
	{
		if(go.Equals(gameObject.transform.root.gameObject))
		{
			_inkTimer.PauseTimer();
		}
	}

	public void ResumeTimer(GameObject go)
	{
		if(go.Equals(gameObject.transform.root.gameObject))
		{
			_inkTimer.ResumeTimer();
		}
	}
	#endregion
	
	#region private methods
    private void UseInk()
    {
        float deltaScale = 0.0f;
        Vector3 scale = _inkColorObj.transform.localScale;
        scale.y = _defaultScale.y * _inkTimer.GetTimeLeftInPctDecimal();
        deltaScale = _inkColorObj.transform.localScale.y - scale.y;
        _inkColorObj.transform.localScale = scale;

        Vector3 pos = _inkColorObj.transform.localPosition;
        pos.y = pos.y - (deltaScale);
        _inkColorObj.transform.localPosition = pos;
    }

    private void RescaleCartridge()
    {
        _inkColorObj.transform.localScale = _defaultScale;
        _inkColorObj.transform.localPosition = _defaultPosition;
    }
	#endregion

}
