using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StartScreenUIScript : MonoBehaviour
{
    [SerializeField] Canvas optionCanvas;
    [SerializeField] Canvas CreditosCanvas;

    public void StartGame()
    {
        SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
    }

    public void ShowCanvas(Canvas obj, bool value)
    {
        obj.gameObject.SetActive(value);
    }
}
