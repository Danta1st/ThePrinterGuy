using UnityEngine;
using System.Collections;

public class ActionSequencerZone : MonoBehaviour
{
    [SerializeField]
    private string _zone;

    // Use this for initialization
    void Start()
    {
 
    }
 
    // Update is called once per frame
    void Update()
    {
 
    }

    public string GetZone()
    {
        return _zone;
    }
}
