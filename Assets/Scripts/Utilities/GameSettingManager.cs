using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingManager : MonoBehaviour
{
    private static GameSettingManager _instance;
    public static GameSettingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameSettingManager");
                go.AddComponent<GameSettingManager>();
                _instance = go.GetComponent<GameSettingManager>();

            }
            return _instance;
        }
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }


    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public float GetTimeScale()
    {
        return Time.timeScale;
    }
    public void SetGlobalTime(float TimeModifier)
    {
        Time.timeScale = TimeModifier;
        // Time.fixedDeltaTime = TimeModifier * 0.02f;
    }
}
