using UnityEngine;
using System.Collections;

public class LevelSelection : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;

    public string GetSceneName()
    {
        return _sceneName;
    }
}
