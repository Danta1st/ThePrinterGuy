using UnityEngine;
using System.Collections;

public class PaperJam : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private int _jamChanceInPercent = 1;
    [SerializeField]
    private float _openAndCloseTime = 1.0f;
	[SerializeField]
	private iTween.EaseType _openEaseType;
	[SerializeField]
	private iTween.EaseType _closeEaseType;
	[SerializeField]
	private GameObject _jammedItem;
    #endregion

    #region Privates
    private float _shakeJamInOut = 10.0f;
    private float _shakeJamLeftRight = 3.0f;
    private float _shakeJamRotate = 0.5f;
    private float _shakeJamTime = 1.5f;
	
    private float _shakeInOut = 2.0f;
    private float _shakeLeftRight = 0.5f;
    private float _shakeRotate = 0.0f;
    private float _shakeTime = 0.5f;
	
    private int _paperJamMaxRate = 100;
    private float _litRotation = -90.0f;
	
    private Vector3 _shakeJam;
    private Vector3 _shakePrint;
    private Quaternion _startRotation;
    private GameObject _paperJamHolder;
    private bool _isJammed;
    private float _lidStartRotation;
    private bool _litOpen = false;
	private ParticleSystem[] _smokes;
    #endregion

    #region Delegates
    public delegate void OnJamAction();
    public static event OnJamAction OnJam;
	
    public delegate void OnUnjammedAction();
    public static event OnUnjammedAction OnUnjammed;
    #endregion
	
	#region Monobehaviour Functions
    void Start()
    {
        _startRotation = gameObject.transform.rotation;
        _shakeJam = new Vector3(_shakeJamRotate, _shakeJamLeftRight, _shakeJamInOut);
        _shakePrint = new Vector3(_shakeRotate, _shakeLeftRight, _shakeInOut);
    }
	
//    void Update()
//    {
//		if(Input.anyKey)
//        	OnPaperPrint(gameObject);
//    }
	#endregion

    #region Enable/Disable
    void OnEnable()
    {
        OnJam += Shake;
        OnUnjammed += JamStopped;
        PrinterManager.OnPagePrinted += OnPaperPrint;
        GestureManager.OnTap += ResolvePaperJam;
    }

    void OnDisable()
    {
        OnJam -= Shake;
        OnUnjammed -= JamStopped;
        PrinterManager.OnPagePrinted -= OnPaperPrint;
        GestureManager.OnTap -= ResolvePaperJam;
    }
    #endregion

    #region PaperJam actions
    void Shake()
    {
        iTween.PunchRotation(gameObject, iTween.Hash("amount", _shakeJam, "time", _shakeJamTime, "oncomplete", "Shake"));
    }

    void JamStopped()
    {
        iTween.Stop(gameObject);
    }
    #endregion

    #region PaperJam Occurs is Resolved
    void OnPaperPrint(GameObject printObject)
    {		
        iTween.PunchRotation(gameObject, iTween.Hash("amount", _shakePrint, "time", _shakeTime));

        if(Random.Range(0, _paperJamMaxRate) <= _jamChanceInPercent)
        {
            if(OnJam != null)
			{
                _isJammed = true;
                OnJam();
                Vector3 tmpPos = new Vector3(transform.position.x, transform.position.y+2, transform.position.z+1);
			
                _jammedItem.transform.position = tmpPos;
                _jammedItem.transform.localScale = new Vector3(1.5f, 0.15f, 1.5f);

                _paperJamHolder = (GameObject)Instantiate(_jammedItem);
			}
        }
    }

    void ResolvePaperJam(GameObject thisGameObj, Vector2 screenPos)
    {
        if(thisGameObj != null)
        {
			//Open or Close the lid
            if(thisGameObj.tag == "JamTask" && !_litOpen && _isJammed)
            {
                iTween.Stop(gameObject);
                transform.rotation = _startRotation;

                _lidStartRotation = thisGameObj.transform.rotation.x;
                iTween.RotateAdd(thisGameObj, iTween.Hash("amount", new Vector3(_litRotation, 0, 0), "time", _openAndCloseTime, "easetype", _openEaseType));
                _litOpen = true;
            }
            else if(thisGameObj.tag == "JamTask" && _litOpen && !_isJammed)
            {
                iTween.Stop(gameObject);
    
                iTween.RotateAdd(thisGameObj, new Vector3(-_litRotation, 0, 0), _openAndCloseTime);
                _litOpen = false;
            }
    		
			//Delete Jammed Item
            if(thisGameObj != null && _paperJamHolder != null)
            {
                if(thisGameObj.name == _paperJamHolder.name)
                {
                    _isJammed = false;
                    OnUnjammed();
                    Destroy(thisGameObj);
                }
            }
        }
    }
    #endregion
}
