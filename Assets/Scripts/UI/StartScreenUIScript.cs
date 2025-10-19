using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

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

    public void StartTutorial()
    {
        SceneManager.LoadScene("Controles", LoadSceneMode.Single);
    }

    public void QUIT_GAME()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
