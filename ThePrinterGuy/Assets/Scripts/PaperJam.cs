using UnityEngine;
using System.Collections;

public class PaperJam : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private float _shakeJamInOut = 10.0f;
    [SerializeField]
    private float _shakeJamLeftRight = 3.0f;
    [SerializeField]
    private float _shakeJamRotate = 0.5f;
    [SerializeField]
    private float _shakeJamTime = 1.5f;
    [SerializeField]
    private float _shakeInOut = 2.0f;
    [SerializeField]
    private float _shakeLeftRight = 0.5f;
    [SerializeField]
    private float _shakeRotate = 0.0f;
    [SerializeField]
    private float _shakeTime = 0.5f;
    [SerializeField]
    private int _paperJamMaxRate = 100;
    [SerializeField]
    private int _paperJamChance = 1;
    #endregion

    #region Privates
    private Vector3 _shakeJam;
    private Vector3 _shakePrint;
    private Quaternion _startRotation;
    private GameObject _smokePrefabHolder;
    private GameObject _paperJam;
    private GameObject _particleHolder;
    private GameObject _paperJamHolder;
    private bool _isJammed;
    #endregion

    #region Delegates
    public delegate void OnJamAction();
    public static event OnJamAction OnJam;
    public delegate void OnUnjammedAction();
    public static event OnUnjammedAction OnUnjammed;
    #endregion

    void Awake()
    {
        _smokePrefabHolder = Resources.Load("Effects/jamSmoke", typeof(GameObject)) as GameObject;

        _paperJam = Resources.Load("Effects/paperJam", typeof(GameObject)) as GameObject;

        _smokePrefabHolder.transform.position = transform.position;
    }

    void Start()
    {
        _startRotation = gameObject.transform.rotation;
        _shakeJam = new Vector3(_shakeJamRotate, _shakeJamLeftRight, _shakeJamInOut);
        _shakePrint = new Vector3(_shakeRotate, _shakeLeftRight, _shakeInOut);

        _smokePrefabHolder.particleSystem.playOnAwake = true;
        _smokePrefabHolder.particleSystem.enableEmission = false;
        _particleHolder = (GameObject)Instantiate(_smokePrefabHolder);
    }

    #region Enable/Disable
    void OnEnable()
    {
        OnJam += Shake;
        OnJam += Smoke;
        OnUnjammed += JamStopped;
        PrinterManager.OnPagePrinted += OnPaperPrint;

        GestureManager.OnTap += ResolvePaperJam;
    }

    void OnDisable()
    {
        OnJam -= Shake;
        OnJam -= Smoke;
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

    void Smoke()
    {
        _particleHolder.particleSystem.enableEmission = true;
    }

    void JamStopped()
    {
        iTween.Stop(gameObject);
        transform.rotation = _startRotation;
    }
    #endregion

    #region PaperJam Occurs is Resolved
    void OnPaperPrint(GameObject printObject)
    {
        iTween.PunchRotation(gameObject, iTween.Hash("amount", _shakePrint, "time", _shakeTime));

        if(Random.Range(0, _paperJamMaxRate) <= _paperJamChance)
        {
            if(OnJam != null)
                _isJammed = true;
                OnJam();
                _paperJamHolder = (GameObject)Instantiate(_paperJam);
        }
    }

    void ResolvePaperJam(GameObject thisPaperJam, Vector2 screenPos)
    {
        if(thisPaperJam != null && _paperJamHolder != null)
        {
            if(thisPaperJam.tag == _paperJamHolder.tag)
            {
                _isJammed = false;
                OnUnjammed();
                Destroy(thisPaperJam);
                _particleHolder.particleSystem.enableEmission = false;
            }
        }
    }
    #endregion
}
