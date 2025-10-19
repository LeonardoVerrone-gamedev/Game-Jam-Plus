using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class GameplayMenu : MonoBehaviour
{
    public static event Action<bool> OnPauseChanged;
    private float originalFixedDeltaTime;

    [SerializeField] TextMeshProUGUI timer;

    [SerializeField] GameManager gameManager;

    
    void Start()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void Pause(bool value)
    {
        Time.timeScale = value ? 0f : 1f;
        OnPauseChanged?.Invoke(value);
    }
    
    void Update()
    {
        timer.text = gameManager.surviveTime.ToString("0:00");
    }
}
