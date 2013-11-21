using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    private static PaperSounds _paperSounds;

    void Awake()
    {
        _paperSounds = transform.FindChild("PaperTray").GetComponent<PaperSounds>();
    }

    public static void thisTest(GameObject go, Vector2 thisPos)
    {
        Debug.Log("Successfull subscription to static ");
    }

    public static void Effect_PaperTray_MoveUp()
    {
        _paperSounds.Effect_PaperTray_MoveUp();
    }

    public static void Effect_PaperTray_MoveDown()
    {
        _paperSounds.Effect_PaperTray_MoveDown();
    }
}
