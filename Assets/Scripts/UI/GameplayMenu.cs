using UnityEngine;
using System;

public class GameplayMenu : MonoBehaviour
{
    public static event Action<bool> OnPauseChanged;
    private float originalFixedDeltaTime;
    
    void Start()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;
    }
    
    public void Pause(bool value)
    {
        Time.timeScale = value ? 0f : 1f;
        OnPauseChanged?.Invoke(value);
    }
}
