using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class GameplayMenu : MonoBehaviour
{
    public static event Action<bool> OnPauseChanged;
    private float originalFixedDeltaTime;

    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI score;

    [SerializeField] GameManager gameManager;

    [SerializeField] Canvas GameOverCanvas;
    [SerializeField] TextMeshProUGUI timerInGameOverScreen;
    [SerializeField] TextMeshProUGUI finalScoreInGameOverScreen;

    [SerializeField] Image[] healthImages;
    [SerializeField] Sprite healthIcon;

    
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
        int minutes = Mathf.FloorToInt(gameManager.surviveTime / 60f);
        int seconds = Mathf.FloorToInt(gameManager.surviveTime % 60f);
        timer.text = $"{minutes:00}:{seconds:00}";

        score.text = gameManager.score.ToString();

        UpdateLife();
    }

    public void SetGameOverScreen()
    {
        GameOverCanvas.gameObject.SetActive(true);
        timerInGameOverScreen.text = gameManager.surviveTime.ToString("0:00");
        finalScoreInGameOverScreen.text = gameManager.score.ToString();
    }
    
    public void UpdateLife()
    {
        for(int i = 0; i < 3; i++)
        {
            if(i < gameManager.life)
            {
                healthImages[i].sprite = healthIcon;
            }
            else
            {
                healthImages[i].sprite = null;
            }
        }
    }
}
