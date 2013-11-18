using UnityEngine;
using System.Collections;

public class ActionSequencerItem : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    private string _moduleName;
    #endregion

    #region Private Variables
    private GameObject _frequencyAnalyzerObject;
    private FrequencyAnalyzer _frequencyAnalyzerScript;
    private GUIGameCamera _guiGameCameraScript;
    private ActionSequencerZone _actionSequencerScript;
    private string _statusZone = "";
    private int _zone = 0;

    private float _refreshStep = 0.0f;
    private float _currentFrequency;
    private float _newFrequency;
    private float _step = 0.0f;
    private float _multiplier = 0.0f;
    private Vector3 _startSize;
    private Vector3 _newSize;
    private bool _isBack = false;
    #endregion

    #region Delegates and Events
    public delegate void FailedAction();
    public static event FailedAction OnFailed;
    #endregion

    // Use this for initialization
    void Start()
    {
        _guiGameCameraScript = GameObject.Find("GUI List").GetComponent<GUIGameCamera>();

        _startSize = transform.localScale;
        _newSize = transform.localScale;

        _frequencyAnalyzerObject = GameObject.Find(_moduleName).gameObject;

        if(_frequencyAnalyzerObject != null)
        {
            _frequencyAnalyzerScript = _frequencyAnalyzerObject.GetComponent<FrequencyAnalyzer>();

            _newFrequency = _frequencyAnalyzerScript.GetPitch() / 900f;
            _currentFrequency = _newFrequency;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _refreshStep += Time.deltaTime;

        if(_frequencyAnalyzerObject != null && _refreshStep > 0.25f)
        {
            _refreshStep = 0.0f;
            //_multiplier = _frequencyAnalyzerScript.GetPitch() / 900f;

//            if(_multiplier < 0.5f)
//            {
//                _multiplier = 0.5f;
//            }
            _newFrequency = _frequencyAnalyzerScript.GetPitch();
        }

        if(_frequencyAnalyzerObject != null)
        {
            //_newSize = new Vector3(_startSize.x*_multiplier, _startSize.y, _startSize.z);
            Debug.Log(_frequencyAnalyzerScript.GetPitch());
            ScaleSize();
        }
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

    private void ScaleSize()
    {
        _step += Time.deltaTime;

        if(_step > 0.25f)
        {
            _step = 0.0f;
            _currentFrequency = _newFrequency;
        }

        //transform.localScale =
    }

//    private void ScaleSize()
//    {
//        _step += Time.deltaTime;
//
//        if(_step > 1.0f)
//        {
//            _step = 0.0f;
//            if(_isBack)
//            {
//                _isBack = false;
//            }
//            else
//            {
//                _isBack = true;
//            }
//        }
//
//        if(_isBack)
//        {
//            transform.localScale = Vector3.Lerp(_newSize, _startSize, _step);
//        }
//        else
//        {
//            transform.localScale = Vector3.Lerp(_startSize, _newSize, _step);
//        }
//    }

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
