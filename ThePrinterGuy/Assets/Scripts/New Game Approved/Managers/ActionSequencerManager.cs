﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionSequencerManager : MonoBehaviour {

    #region SerializeField Variables
    [SerializeField]
    private ActionSequencerList[] _actionSequencerList;
    #endregion

    #region Class Variables
    private string _newItem = "";
    private string _focusItem = "";
    private int _spawnIndex = 0;
    private int _focusIndex = 0;
    private float _startTimeStamp;
    private float _currentTimeStamp;
    private TimerUtilities _timer;
    #endregion

    #region Delegates and Events
    public delegate void CreateNewNodeAction(string itemName);
    public static event CreateNewNodeAction OnCreateNewNode;

    public delegate void PaperNodeAction();
    public static event PaperNodeAction OnPaperNode;

    public delegate void InkNodeAction();
    public static event InkNodeAction OnInkNode;

    public delegate void UraniumRodNodeAction();
    public static event UraniumRodNodeAction OnUraniumRodNode;

    public delegate void BarometerNodeAction();
    public static event BarometerNodeAction OnBarometerNode;
    #endregion

    #region OnEnable and OnDisable
    void OnEnable()
    {

    }

    void OnDisable()
    {

    }
    #endregion

    #region Start and Update
	// Use this for initialization
	void Start ()
    {
        _timer = gameObject.AddComponent<TimerUtilities>();
        _startTimeStamp = 40;
        _timer.StartTimer(_startTimeStamp, true);

        if(_actionSequencerList.Length > 0)
        {
            UpdateItemInFocus();
        }

        _timer.ResumeTimer();
	}
	
	// Update is called once per frame
	void Update ()
    {
        _currentTimeStamp =  _startTimeStamp - _timer.GetTimeLeft();

        if(_spawnIndex < _actionSequencerList.Length){
            if(_currentTimeStamp >= _actionSequencerList[_spawnIndex].timeStamp)
            {
                SpawnItem();
            }
        }
	}
    #endregion

    #region Class Methods
    private void SpawnItem()
    {
        if(_spawnIndex < _actionSequencerList.Length)
        {
            _newItem = _actionSequencerList[_spawnIndex].actionItem.ToString();

            if(OnCreateNewNode != null && _newItem != "None")
            {
                OnCreateNewNode(_newItem);
            }
        }

        _spawnIndex++;
        if(_spawnIndex >= _actionSequencerList.Length)
        {
            _spawnIndex = _actionSequencerList.Length;
        }
    }

    private void UpdateItemInFocus()
    {
        _focusItem = _actionSequencerList[_focusIndex].actionItem.ToString();
        if(_focusItem == "Paper")
        {
            if(OnPaperNode != null)
            {
                OnPaperNode();
            }
        }
        else if(_focusItem == "Ink")
        {
            if(OnInkNode != null)
            {
                OnInkNode();
            }
        }
        else if(_focusItem == "UraniumRod")
        {
            if(OnUraniumRodNode != null)
            {
                OnUraniumRodNode();
            }
        }
        else if(_focusItem == "Barometer")
        {
            if(OnBarometerNode != null)
            {
                OnBarometerNode();
            }
        }

        _focusIndex++;
        if(_focusIndex >= _actionSequencerList.Length)
        {
            _focusIndex = _actionSequencerList.Length;
        }
    }
    #endregion

    #region Subclasses
    [System.Serializable]
    public class ActionSequencerList
    {
        public float timeStamp;
        public ActionItem actionItem = ActionItem.None;

        public enum ActionItem
        {
            None,
            Paper,
            Ink,
            UraniumRod,
            Barometer
        };
    };
    #endregion
}
