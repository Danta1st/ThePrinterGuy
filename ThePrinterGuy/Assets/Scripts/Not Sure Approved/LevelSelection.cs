using UnityEngine;
using System.Collections;

public class LevelSelection : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;

    public void GoToLevel()
    {
        Application.LoadLevel(_sceneName);
    }
}
