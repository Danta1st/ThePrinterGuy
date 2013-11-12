using UnityEngine;
using System.Collections;

public class ActionSequencerItem : MonoBehaviour {

    private ActionSequencerZone _actionSequencerScript;
    private string _statusZone = "";

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "SequencerZone")
        {
            _actionSequencerScript = other.gameObject.GetComponent<ActionSequencerZone>();
            _statusZone = _actionSequencerScript.GetZone();

            if(_statusZone == "Dead"){
                Destroy(gameObject);
            }
        }
    }
}
