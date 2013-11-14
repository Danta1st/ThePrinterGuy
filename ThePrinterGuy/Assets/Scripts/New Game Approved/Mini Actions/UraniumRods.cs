using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UraniumRods : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private iTween.EaseType _easeTypeOut = iTween.EaseType.easeOutExpo;
    [SerializeField]
    private iTween.EaseType _easeTypeIn = iTween.EaseType.easeOutBack;
    [SerializeField]
    private GameObject[] _rods;
    #endregion

    #region Privates
    private float _outTime = 0.5f;
    private float _inTime = 0.2f;
    private Dictionary<GameObject, bool> _rodsAndStates = new Dictionary<GameObject, bool>();
    #endregion

    #region Delegates & Events
    public delegate void OnRodHammeredAction();
    public static event OnRodHammeredAction OnRodHammered;
    #endregion

    
    //TODO: Insert Proper connectivity to the Action Sequencer
	//TODO: Handle gesture allowance
    void OnEnable()
    {
		QATestCamera.OnUranRods += TriggerSpring;
//        GestureManager.OnSwipeUp += TriggerSpring;
        GestureManager.OnTap += TriggerHammer;
//        GestureManager.OnSwipeLeft += Reset;
    }

    void OnDisable()
    {
		QATestCamera.OnUranRods -= TriggerSpring;
//        GestureManager.OnSwipeUp -= TriggerSpring;
        GestureManager.OnTap -= TriggerHammer;
//        GestureManager.OnSwipeLeft -= Reset;
    }

    void Start()
    {
        SetStates();
    }


    #region Class Methods
    //Initialise the states for each game object
    private void SetStates()
    {
        foreach(GameObject go in _rods)
        {
            _rodsAndStates.Add(go, false);
        }
    }

    //Trigger a random rod, which is currently not up
    private void TriggerSpring()
    {
        var identifier = Random.Range(0, _rods.Length);

        for(int i = 0; i < _rods.Length; i++)
        {
            var go = _rods[identifier];
            if(_rodsAndStates[go] == false)
            {
                Spring(go);
                _rodsAndStates[go] = true;
                break;
            }
            identifier++;

            if(identifier == _rods.Length)
                identifier = 0;
        }
    }

    private void Spring(GameObject go)
    {
        iTween.MoveTo(go, iTween.Hash("y", go.transform.localPosition.y + 1, "time", _outTime,
                                        "islocal", true, "easetype", _easeTypeOut));
    }

    //Hammer the rod if it is currently up
    private void TriggerHammer(GameObject go, Vector2 screenPos)
    {
        foreach(KeyValuePair<GameObject, bool> pair in _rodsAndStates)
        {
            if(pair.Key == go && pair.Value == true)
            {
                Hammer(go);
                _rodsAndStates[go] = false;

                if(OnRodHammered != null)
                    OnRodHammered();

                break;
            }
        }
    }

    private void Hammer(GameObject go)
    {
        iTween.MoveTo(go, iTween.Hash("y", go.transform.localPosition.y - 1, "time", _inTime,
                                        "islocal", true, "easetype", _easeTypeIn));
    }

    //Reset all the rods
    private void Reset()
    {
        foreach(GameObject go in _rods)
        {
            if(_rodsAndStates[go] == true)
            {
                Hammer(go);
                _rodsAndStates[go] = false;
            }
        }
    }
    #endregion
}
