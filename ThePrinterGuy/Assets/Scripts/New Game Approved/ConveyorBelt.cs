using UnityEngine;
using System.Collections;

public class ConveyorBelt : MonoBehaviour
{
    private GameObject _conveyorBelt;

    void Start()
    {
        _conveyorBelt = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        RollConvoreBelt();
    }

    public void RollConvoreBelt()
    {
        Vector2 thisOffset = _conveyorBelt.renderer.material.mainTextureOffset;
        _conveyorBelt.renderer.material.mainTextureOffset = (thisOffset + new Vector2(0.0f, -0.001f));
    }
}
