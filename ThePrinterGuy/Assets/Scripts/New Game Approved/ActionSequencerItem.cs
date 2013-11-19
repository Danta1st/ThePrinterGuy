using UnityEngine;
using System.Collections;

public class ActionSequencerItem : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    private float _ms;
    [SerializeField]
    private float _actionSequencerItemSpeed;
    [SerializeField]
    private iTween.EaseType _easeTypeActionSequencerItem;
    #endregion

    #region Private Variables
    private GUIGameCamera _guiGameCameraScript;
    private ActionSequencerZone _actionSequencerScript;
    private string _statusZone = "";
    private int _zone = 0;
    private float _delay = 0.0f;

    private Vector3 _destinationPosition;
    #endregion

    #region Delegates and Events
    public delegate void FailedAction();
    public static event FailedAction OnFailed;
    #endregion

    void Awake()
    {
        _guiGameCameraScript = GameObject.Find("GUI List").GetComponent<GUIGameCamera>();
        _destinationPosition = GameObject.Find("DeadZone").transform.position;


    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine("OnStartTween");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "SequencerZone")
        {
            _actionSequencerScript = other.gameObject.GetComponent<ActionSequencerZone>();
            _statusZone = _actionSequencerScript.GetZone();

            if(_statusZone == "Dead")
            {
                if(OnFailed != null)
                {
                    OnFailed();
                }
                _guiGameCameraScript.EndZone(gameObject);
            }
        }
    }

    private IEnumerator OnStartTween()
    {
        _delay = _ms - (Time.time % _ms);
        Debug.Log(Time.time);
        Debug.Log("Delay: " + _delay);
        yield return new WaitForSeconds(_delay);
        OnContinueTween();

        Debug.Log(Time.time);
        iTween.PunchScale(gameObject, iTween.Hash("amount", new Vector3(20,0,0), "time", _ms, "looptype", iTween.LoopType.loop));
        iTween.MoveTo(gameObject, iTween.Hash("position", _destinationPosition, "speed", _actionSequencerItemSpeed,
                                                    "easeType", _easeTypeActionSequencerItem));
                                                    Debug.Log(Time.time);
                                                    Debug.Log("Time: " + (float)(Time.time + _delay + _ms));
//        double temp = _delay + Time.time + _ms;
//        Debug.Log("Expected time: " + temp);
//        iTween.PunchScale(gameObject, iTween.Hash("amount", new Vector3(20,0,0), "time", _ms, "delay", _delay,
//                                                    "onComplete", "OnContinueTween", "onCompleteTarget", gameObject));
    }

    private void OnContinueTween()
    {

    }

    public int GetZoneStatus()
    {
        if(_statusZone == "Red")
        {
            _zone = 1;
        }
        else if(_statusZone == "Yellow")
        {
            _zone = 2;
        }
        else if(_statusZone == "Green")
        {
            _zone = 3;
        }

        return _zone;
    }
}
