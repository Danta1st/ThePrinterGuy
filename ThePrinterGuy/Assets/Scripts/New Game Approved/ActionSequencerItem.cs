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

    private float _multiplier = 0.0f;
    private Vector3 _startSize;
    private Vector3 _newSize;
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_frequencyAnalyzerObject != null)
        {
            _multiplier = _frequencyAnalyzerScript.GetPitch() / 500f;
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
        _newSize = new Vector3(_startSize.x*_multiplier, _startSize.y, _startSize.z);
        transform.localScale = _newSize;
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
