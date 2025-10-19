using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public float surviveTime;
    [SerializeField] public int life = 3;
    [SerializeField] public int score;

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
    }
    
    public void OnCompleteFrank()
    {
        score += 10;
    }
}
