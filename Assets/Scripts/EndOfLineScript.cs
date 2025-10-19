using UnityEngine;
using System.Collections.Generic;

public class EndOfLineScript : MonoBehaviour
{
    public List<FrankensteinMonster> completeMonsters = new List<FrankensteinMonster>();
    public List<FrankensteinMonster> incompleteMonsters = new List<FrankensteinMonster>();

    [SerializeField] GameManager gameManager;

    void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"TriggerEnter detectado com: {other.gameObject.name}");
        FrankensteinMonster currentMonster = other.gameObject.GetComponent<FrankensteinMonster>();

        if (currentMonster != null)
        {
            ProcessMonster(currentMonster);
        }
    }
    
    void ProcessMonster(FrankensteinMonster monster)
    {
        if (monster.IsComplete())
        {
            Debug.Log("Frankeinstein chegou ao final completo!");
            completeMonsters.Add(monster);
            gameManager.OnCompleteFrank();
        }
        else
        {
            Debug.Log($"Frankeinstein chegou incompleto");
            incompleteMonsters.Add(monster);

            gameManager.Miss();
        }

        Destroy(monster.gameObject);
    }
}