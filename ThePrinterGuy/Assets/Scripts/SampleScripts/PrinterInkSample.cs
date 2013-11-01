using UnityEngine;
using System.Collections;

public class PrinterInkSample : MonoBehaviour
{
    private InkCartridge _red;
    private InkCartridge _green;
    private InkCartridge _blue;
    private GameObject _go;

	// Use this for initialization
	void Start ()
    {
        _red = GameObject.FindGameObjectWithTag("InkRed").GetComponent<InkCartridge>();
        //_blue = GameObject.FindGameObjectWithTag("InkBlue").AddComponent<InkCartridge>();
        //_green = GameObject.FindGameObjectWithTag("InkGreen").AddComponent<InkCartridge>();
        _red.InitializeInkCartridge(Color.red, 30f);
        //_blue.InitializeInkCartridge();
        //_green.InitializeInkCartridge();
	}
	
	// Update is called once per frame
	void Update ()
    {

	}
}
