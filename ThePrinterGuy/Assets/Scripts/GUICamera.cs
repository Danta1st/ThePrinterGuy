using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUICamera : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private LayerMask _layerMaskGUI;
    //List of all gui elements.
    [SerializeField]
    private GameObject[] _guiList;
    //List of tool box pages.
    [SerializeField]
    private GameObject[] _toolBoxPages;
    #endregion

    #region Private Variables
    private GameObject _guiAnchorPointObject;
    private Camera _guiCamera;
    private RaycastHit _hit;
    private bool _isGUI = false;
    private bool _canTouch = true;
    private float _timeScale = 0.0f;

    private List<GameObject> _guiSaveList = new List<GameObject>();

    private float _scaleMultiplier;

    //Ingame menu variables.
    private GameObject _ingameMenuObject;
    private Vector3 _ingameMenuStartPos;
    private Vector3 _ingameMenuTargetPos;
    private float _ingameMenuDuration = 0.2f;

    //Tool box variables.
    private GameObject _toolBoxSelectionObject;
    private GameObject _toolBoxObject;
    private Vector3 _toolBoxStartPos;
    private Vector3 _toolBoxTargetPos;
    private int _toolBoxCurrentPageCount = 1;
    private int _toolBoxMaxPageCount;
    private float _toolBoxMoveDuration = 1.0f;
    private bool _isToolBoxOpen = false;
    #endregion

    #region Delegates and Events
    public delegate void InventoryPress(string buttonName);
    public static event InventoryPress OnInventoryPress;
    #endregion

    #region Enable and Disable
    void OnEnable()
    {
        GestureManager.OnTap += CheckCollision;
    }

    void OnDisable()
    {
        GestureManager.OnTap -= CheckCollision;
    }

    public void EnableGUICamera()
    {
        _isGUI = true;
        _guiCamera.gameObject.SetActive(true);
    }

    public void DisableGUICamera()
    {
        _isGUI = false;
        _guiCamera.gameObject.SetActive(false);
    }

    public void EnableGUIElement(string _name)
    {
        foreach(GameObject _gui in _guiList)
        {
            if(_gui.name == _name)
            {
                _gui.SetActive(true);
            }
        }
    }

    public void DisableGUIElement(string _name)
    {
        foreach(GameObject _gui in _guiList)
        {
            if(_gui.name == _name)
            {
                _gui.SetActive(false);
            }
        }
    }

    public void EnableGUIElementAll()
    {
        foreach(GameObject _gui in _guiList)
        {
            _gui.SetActive(true);
        }
    }

    public void DisableGUIElementAll()
    {
        foreach(GameObject _gui in _guiList)
        {
            _gui.SetActive(false);
        }
    }
    #endregion

    #region Start and Update
    // Use this for initialization
    void Start()
    {
        //GUI Camera and rescale of GUI elements.
        //--------------------------------------------------//
        _guiCamera = GameObject.FindGameObjectWithTag("GUICamera").camera;
        transform.position = _guiCamera.transform.position;

//        _scaleMultiplierY = Screen.width / Screen.height;
//        Debug.Log(Screen.height + " " + _scaleMultiplier);
//        _guiCamera.orthographicSize = _guiCamera.orthographicSize * _scaleMultiplierY;
//        GameObject.Find("GUIGame").transform.localScale *= _scaleMultiplierY;
        //--------------------------------------------------//

        //Find specific gui objects in the gui list.
        //--------------------------------------------------//
        foreach(GameObject _guiObject in _guiList)
        {
            if(_guiObject.name == "IngameMenu")
            {
                _ingameMenuObject = _guiObject.transform.FindChild("StatsOverview").gameObject;

                Vector3 _tempIngameMenuPos = new Vector3(_ingameMenuObject.transform.position.x, _ingameMenuObject.transform.position.y, 5);
                _ingameMenuObject.transform.position = _tempIngameMenuPos;

                _ingameMenuStartPos = _ingameMenuObject.transform.position;
                _ingameMenuTargetPos = _guiObject.transform.FindChild("IngameDestinationPosition").position;
                _ingameMenuTargetPos.x = _ingameMenuStartPos.x;
                _ingameMenuTargetPos.z = _ingameMenuStartPos.z;

                _guiObject.transform.FindChild("IngameDestinationPosition").gameObject.SetActive(false);

                _guiObject.SetActive(false);
            }

            if(_guiObject.name == "ToolBox")
            {
                _toolBoxObject = _guiObject;

                Vector3 _tempToolBoxPos = new Vector3(_toolBoxObject.transform.position.x, _toolBoxObject.transform.position.y, 5);
                _toolBoxObject.transform.position = _tempToolBoxPos;
    
                _toolBoxStartPos = _toolBoxObject.transform.position;
                _toolBoxTargetPos = _toolBoxObject.transform.FindChild("ToolBoxDestinationPosition").position;
                _toolBoxTargetPos.y = _toolBoxStartPos.y;
                _toolBoxTargetPos.z = _toolBoxStartPos.z;
    
                _toolBoxObject.transform.FindChild("ToolBoxDestinationPosition").gameObject.SetActive(false);
                _toolBoxMaxPageCount = _toolBoxPages.Length;
            }

            if(_guiObject.name == "ToolBoxSelection")
            {
                _toolBoxSelectionObject = _guiObject;
                _toolBoxSelectionObject.SetActive(false);
            }
        }
        //--------------------------------------------------//

        EnableGUICamera();
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Class Methods
    private void CheckCollision(GameObject _go, Vector2 _screenPosition)
    {
        if(_isGUI && _canTouch)
        {
            Ray _ray = _guiCamera.ScreenPointToRay(_screenPosition);

            if(Physics.Raycast(_ray, out _hit, 100, _layerMaskGUI.value))
            {
                //General GUI layer mask.
                //-----------------------------------------------------------------------//
                if(_hit.collider.gameObject.layer == LayerMask.NameToLayer("GUI"))
                {

                }
                //GUI Menu layer mask.
                //-----------------------------------------------------------------------//
                if(_hit.collider.gameObject.layer == LayerMask.NameToLayer("GUIMenu"))
                {
                    if(_hit.collider.gameObject.name == "Pause")
                    {
                        OpenIngameMenu();
                    }
                    else if(_hit.collider.gameObject.name == "Resume")
                    {
                        CloseIngameMenu();
                    }
                    else if(_hit.collider.gameObject.name == "Restart")
                    {
                        RestartLevel();
                    }
                    else if(_hit.collider.gameObject.name == "Quit")
                    {
                        QuitLevel();
                    }
                    else if(_hit.collider.gameObject.name == "Settings")
                    {

                    }
                }
                //ToolBox layer mask.
                //-----------------------------------------------------------------------//
                if(_hit.collider.gameObject.layer == LayerMask.NameToLayer("GUIToolBox"))
                {
                    if(_hit.collider.gameObject.name == "ToolBoxButton")
                    {
                        if(_isToolBoxOpen)
                        {
                            _isToolBoxOpen = false;
                            MoveGUIElement(_toolBoxObject, _toolBoxStartPos, _toolBoxMoveDuration);
                            _toolBoxSelectionObject.SetActive(false);

                        }
                        else if(!_isToolBoxOpen)
                        {
                            _isToolBoxOpen = true;
                            MoveGUIElement(_toolBoxObject, _toolBoxTargetPos, _toolBoxMoveDuration);
                            _toolBoxSelectionObject.SetActive(false);
                        }
                    }
                    else if(_hit.collider.gameObject.name == "ToolBoxPageUp")
                    {
                        _toolBoxCurrentPageCount -= 1;

                        if(_toolBoxCurrentPageCount < 1)
                        {
                            _toolBoxCurrentPageCount = 1;
                        }
                        UpdateToolBoxPage();
                        _toolBoxSelectionObject.SetActive(false);
                    }
                    else if(_hit.collider.gameObject.name == "ToolBoxPageDown")
                    {
                         _toolBoxCurrentPageCount += 1;

                        if(_toolBoxCurrentPageCount > _toolBoxMaxPageCount)
                        {
                            _toolBoxCurrentPageCount = _toolBoxMaxPageCount;
                        }
                        UpdateToolBoxPage();
                        _toolBoxSelectionObject.SetActive(false);
                    }
                    else
                    {
                        if(OnInventoryPress != null)
                        {
                            OnInventoryPress(_hit.collider.gameObject.name);
                        }

                        _toolBoxSelectionObject.SetActive(true);
                        Vector3 _tempPos = _hit.collider.transform.position;
                        _tempPos.z = 6;
                        _toolBoxSelectionObject.transform.position = _tempPos;
                    }
                }
                //-----------------------------------------------------------------------//
            }
            else
            {
                _toolBoxSelectionObject.SetActive(false);
            }
        }
    }

    private void MoveGUIElement(GameObject _guiElement, Vector3 _position, float _duration)
    {
        iTween.MoveTo(_guiElement, iTween.Hash("position", _position, "duration", _duration));
    }

    #region GUI ToolBox
    private void UpdateToolBoxPage()
    {
        int _index = 0;
        foreach(GameObject _toolBoxPage in _toolBoxPages)
        {
            _index++;

            if(_index == _toolBoxCurrentPageCount)
            {
                _toolBoxPage.SetActive(true);
            }
            else
            {
                _toolBoxPage.SetActive(false);
            }
        }
    }
    #endregion

    #region GUI Ingame Menu
    private void OpenIngameMenu()
    {
        SaveGUIState();
        DisableGUIElement("Pause");
        EnableGUIElement("IngameMenu");

        MoveGUIElement(_ingameMenuObject, _ingameMenuTargetPos, _ingameMenuDuration);
    }

    private void CloseIngameMenu()
    {
        float _start = 0.0f;
        float _end = 3.0f;
        _canTouch = false;
        StartCoroutine(UnpauseTimer(_start ,_end));
    }

    IEnumerator UnpauseTimer(float _start, float _end)
    {
        while(true)
        {
            if(_start < _end)
            {
                _start++;
            }
            else
            {
                _canTouch = true;
                MoveGUIElement(_ingameMenuObject, _ingameMenuStartPos, _ingameMenuDuration);
                yield return new WaitForSeconds(_ingameMenuDuration+0.1f);
                LoadGUIState();
                DisableGUIElement("IngameMenu");
                break;
            }
            yield return new WaitForSeconds(1);
        }
    }
    #endregion

    #region GUI Restart and Quit
    private void RestartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    private void QuitLevel()
    {
        //Application.LoadLevel("SOMETHING SCENE");
    }
    #endregion

    #region GUI Save and Load
    private void SaveGUIState()
    {
        _timeScale = Time.timeScale;

        foreach(GameObject _gui in _guiList)
        {
            if(_gui.activeInHierarchy)
            {
                _guiSaveList.Add(_gui);
                _gui.SetActive(false);
            }
        }
    }

    private void LoadGUIState()
    {
        Time.timeScale = _timeScale;

        foreach(GameObject _gui in _guiSaveList)
        {
            _gui.SetActive(true);
        }
        _guiSaveList.Clear();
    }
    #endregion

    #endregion
}
