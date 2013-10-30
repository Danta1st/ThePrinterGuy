using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{
    [SerializeField]
    private float _blinkRate = 0.5f;

    private Material _lightOn = (Material)Resources.Load("Materials/lightOnMat", typeof(Material));
    private Material _lightOff = (Material)Resources.Load("Materials/lightOffMat", typeof(Material));

    private bool _isEnabled = false;
    private GameObject _go;
    private enum States { On, Off };
    private States _state = States.Off;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void InitializeLight(GameObject go, bool enabled)
    {
        _isEnabled = enabled;
        _go = go;
        UpdateTexture();

        if(_isEnabled)
        {
            StartCoroutine("Blink");
        }
    }

    public void InitializeLight(GameObject go, bool enabled, float blinkRate)
    {
        _isEnabled = enabled;
        _go = go;
        _blinkRate = blinkRate;
        UpdateTexture();

        if(_isEnabled)
        {
            StartCoroutine("Blink");
        }
    }

    public void EnableLight()
    {
        _isEnabled = true;
        StartCoroutine("Blink");
        UpdateTexture();
    }

    public void DisableLight()
    {
        _isEnabled = false;
        StopCoroutine("Blink");
        UpdateTexture();
    }

    private void UpdateTexture()
    {
        if(_isEnabled)
        {
            _go.renderer.material = _lightOn;
        }
        else
        {
            _go.renderer.material = _lightOff;
        }
    }

    private void SwitchState()
    {
        switch(_state)
        {
        case States.On:
            _go.renderer.material = _lightOn;
            break;
        case States.Off:
            _go.renderer.material = _lightOff;
            break;
        }
    }

    private void SwitchTexture()
    {
        switch(_state)
        {
        case States.On:
            _state = States.Off;
            break;
        case States.Off:
            _state = States.On;
            break;
        }
    }

    IEnumerator Blink()
    {
        while(true)
        {
            SwitchState();
            SwitchTexture();

            yield return new WaitForSeconds(_blinkRate);
        }
    }
}
