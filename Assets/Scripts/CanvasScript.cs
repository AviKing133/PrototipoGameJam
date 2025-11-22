using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] GameObject panel;
    public void PauseGame()
    {
        // activem el panell de pausa i aturem el temps
        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Quit()
    {
        // sortim de l'aplicacio
        Application.Quit();
    }

    public void ResumeGame()
    {
        // desactivem el panell de pausa i reactivem el temps
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        // reiniciem l'escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        ResumeGame();
    }
}
