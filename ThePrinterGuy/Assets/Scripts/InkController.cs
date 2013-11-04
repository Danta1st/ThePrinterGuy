using UnityEngine;
using System.Collections;

public class InkController : MonoBehaviour
{
    private InkCartridge _redInk;
    private InkCartridge _greenInk;
    private InkCartridge _blueInk;
	private bool _tasksEnabled = true;
	[SerializeField]
	private iTween.EaseType _easeType;
	[SerializeField]
	private float _animationSpeed = 0.5f;
	[SerializeField]
	private float _rotationAmount = 75f;
	private bool _inkSelected = true;
	
	void OnEnable()
	{
		ZoomHandler.OnInk += EnableInkTask;
		GestureManager.OnSwipeLeft += RotateLeft;
		GestureManager.OnSwipeRight += RotateRight;
		GestureManager.OnTap += InsertInk;
	}
	
	void OnDisable()
	{
		ZoomHandler.OnInk -= EnableInkTask;
		GestureManager.OnSwipeLeft -= RotateLeft;
		GestureManager.OnSwipeRight -= RotateRight;
		GestureManager.OnTap -= InsertInk;
	}
	
    void Start()
    {
        _redInk = GameObject.FindGameObjectWithTag("InkRed").GetComponent<InkCartridge>();
		_greenInk = GameObject.FindGameObjectWithTag("InkGreen").GetComponent<InkCartridge>();
		_blueInk = GameObject.FindGameObjectWithTag("InkBlue").GetComponent<InkCartridge>();
		_redInk.InitializeInkCartridge(Color.red);
        _greenInk.InitializeInkCartridge(Color.green);
        _blueInk.InitializeInkCartridge(Color.blue);
		
    }
	
	private void EnableInkTask()
	{
		_tasksEnabled = true;		
	}
	
	private void RotateLeft()
	{
		if (_tasksEnabled)
		{
			iTween.RotateAdd(this.gameObject, iTween.Hash("amount", new Vector3(0, _rotationAmount, 0),
					"time", _animationSpeed, "easetype", _easeType));
		}
	}
	
	private void RotateRight()
	{
		if (_tasksEnabled)
		{
			iTween.RotateAdd(this.gameObject, iTween.Hash("amount", new Vector3(0, -_rotationAmount, 0),
					"time", _animationSpeed, "easetype", _easeType));
		}
	}
	
	private void InsertInk(GameObject go, Vector2 screenPos)
	{
		if(_tasksEnabled)
		{
			if (_inkSelected)
			{
				switch(go.tag)
				{
				case "InkRed":
					_redInk.RefillInk();
					break;
				case "InkGreen":
					_greenInk.RefillInk();
					break;
				case "InkBlue":
					_blueInk.RefillInk();
					break;
				default:
					_inkSelected = false;
					break;
				}
			}
		}
	}
}
