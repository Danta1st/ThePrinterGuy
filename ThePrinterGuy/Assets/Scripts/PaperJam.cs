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
        Instantiate(_smokePrefabHolder);

        _smokePrefabHolder.transform.position = transform.position;
    }

    void Start()
    {
        _startRotation = gameObject.transform.rotation;
        _shakeJam = new Vector3(_shakeJamRotate, _shakeJamLeftRight, _shakeJamInOut);
        _shakePrint = new Vector3(_shakeRotate, _shakeLeftRight, _shakeInOut);
    }

    void Update()
    {
        //Should be connected to the paper print event
        OnPaperPrint();
    }

    void OnEnable()
    {
        OnJam += Shake;
        OnJam += Smoke;
        OnUnjammed += JamStopped;
        //OnPrintPaper += OnPaperPrint;
    }

    void OnDisable()
    {
        OnJam -= Shake;
        OnJam -= Smoke;
        OnUnjammed -= JamStopped;
        //OnPrintPaper += OnPaperPrint;
    }

    void Shake()
    {
        iTween.PunchRotation(gameObject, iTween.Hash("amount", _shakeJam, "time", _shakeJamTime, "oncomplete", "Shake"));
    }

    void JamStopped()
    {
        iTween.Stop(gameObject);
        transform.rotation = _startRotation;
        _smokePrefabHolder.particleSystem.Stop();
    }

    void Smoke()
    {
        _smokePrefabHolder.particleSystem.Play();
    }

    void OnPaperPrint()
    {
        iTween.PunchRotation(gameObject, iTween.Hash("amount", _shakePrint, "time", _shakeTime, "oncomplete", "Shake"));

        if(Random.Range(0, _paperJamMaxRate) <= _paperJamChance)
        {
            if(OnJam != null)
                OnJam();
        }
    }
}
