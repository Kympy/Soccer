using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            _instance ??= GameObject.FindObjectOfType<GameManager>();
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(_instance);
        }
        _instance ??= this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public float Mouse_X_Speed = 100f;
    public float Mouse_Y_Speed = 100f;
}
