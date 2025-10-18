using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public float surviveTime;
    [SerializeField] public int life = 3;
    [SerializeField] public int score;

    void Start()
    {
        score = 0;
    }

    void Update()
    {
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

    }
    
    public void OnCompleteFrank()
    {
        score += 10;
    }
}
