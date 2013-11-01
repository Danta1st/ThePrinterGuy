using UnityEngine;
using System.Collections;

public class InkController : MonoBehaviour
{
    private InkCartridge _redInk;
    private InkCartridge _greenInk;
    private InkCartridge _blueInk;

    void Start()
    {
        _redInk.InitializeInkCartridge(Color.red);
        _greenInk.InitializeInkCartridge(Color.green);
        _blueInk.InitializeInkCartridge(Color.blue);
    }
}
