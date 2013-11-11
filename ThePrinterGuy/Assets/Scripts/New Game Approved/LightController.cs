using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    private Material _lightOn;
    [SerializeField]
    private Material _lightOff;
    [SerializeField]
    private float _blinkRate = 0.5f;
    #endregion

    #region Private variables
    private bool _isEnabled = false;
    private GameObject _go;
    private enum States { On, Off };
    private States _state = States.Off;
    #endregion

    #region Public methods
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
    #endregion

    #region Private methods
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
    #endregion
}
