using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public float surviveTime;
    [SerializeField] public int life = 3;
    [SerializeField] public int score;
    [SerializeField] GameplayMenu menu;

    bool hasLost;

    void Start()
    {
        score = 0;
        hasLost = false;
    }

    void Update()
    {
        if (hasLost)
        {
            return;
        }

        surviveTime += Time.deltaTime;
    }

    public void Miss()
    {
        life--;

        if (life <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        hasLost = true;

        menu.SetGameOverScreen();
    }

    public void OnCompleteFrank()
    {
        score += 10;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("StartScreen", LoadSceneMode.Single);
    }
    
    public void TryAgain()
    {
        SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
    }
}
