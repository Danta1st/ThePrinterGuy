using UnityEngine;
using System.Collections;

public class SequencerTest : MonoBehaviour {
	
	#region Editor Publics
	[SerializeField] private TaskSequence[] _TaskSequences;
	#endregion
	
	
	#region Privates
	#endregion
	
	#region Delegates & Events
    public delegate void CreateNewNodeAction(string itemName);
    public static event CreateNewNodeAction OnCreateNewNode;

    public delegate void LastNodeAction();
    public static event LastNodeAction OnLastNode;
	
	public delegate void DefaultCamPosAction();
    public static event DefaultCamPosAction OnDefaultCamPos;
	
    public delegate void OnTaskAction(int taskIdentifier);

    public static event OnTaskAction OnPaperNode;
    public static event OnTaskAction OnInkNode;
    public static event OnTaskAction OnUraniumRodNode;
    public static event OnTaskAction OnBarometerNode;
	#endregion
	
	void Start () {
	
	}
	void Update () {
	
	}
	
	#region Class Methods
	private void EnableSequencer()
	{
		
	}
	private void StopSequencer()
	{
		
	}
	#endregion
	
    #region SubClasses
    [System.Serializable]
    public class TaskSequence
    {
		public enum Tasks
		{
			Paper, Ink, Uranium
		}
        public enum Beats
		{
			OnBeat3rd1,
			OnBeat3rd2,
			OnBeat3rd3,
			//4 tact beats
			OnBeat4th1,
			OnBeat4th2,
			OnBeat4th3,
			OnBeat4th4,
			//6 tact Beats
			OnBeat6th1,
			OnBeat6th2,
			OnBeat6th3,
			OnBeat6th4,
			OnBeat6th5,
			OnBeat6th6,
			//8 tact Beats
			OnBeat8th1,
			OnBeat8th2,
			OnBeat8th3,
			OnBeat8th4,
			OnBeat8th5,
			OnBeat8th6,
			OnBeat8th7,
			OnBeat8th8
		}
		public Tasks task;
		public Beats beat; 
        public int[] amounts;
		public int SkippingEvery;
		public int beatsUntillNextSequence;
    };
    #endregion
}
