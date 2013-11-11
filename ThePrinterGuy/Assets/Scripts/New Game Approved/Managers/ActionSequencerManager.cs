using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionSequencerManager : MonoBehaviour {

    #region SerializeField Variables
    [SerializeField]
    private ActionSequencerList[] _actionSequencerList;
    #endregion

    #region Class Variables
    private string _currentItem = "";
    private string _nextItem = "";

    private float _currentTimeStamp;
    private TimerUtilities _timer;
    #endregion

    #region Delegates and Events
    public delegate void PaperNodeAction();
    public static event PaperNodeAction OnPaperNode;

    public delegate void InkNodeAction();
    public static event InkNodeAction OnInkNode;

    public delegate void UraniumRodNodeAction();
    public static event UraniumRodNodeAction OnUraniumRodNode;

    public delegate void BarometerNodeAction();
    public static event BarometerNodeAction OnBarometerNode;
    #endregion

	// Use this for initialization
	void Start ()
    {
        _timer = gameObject.AddComponent<TimerUtilities>();
        _timer.StartTimer(20, false);
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    #region Subclasses
    [System.Serializable]
    public class ActionSequencerList
    {
        public float timeStamp;
        public ActionItem actionItem = ActionItem.Paper;

        public enum ActionItem
        {
            Paper,
            Ink,
            UraniumRod,
            Barometer
        };
    };
    #endregion
}
