using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void CambioEscena()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void SalirJuego()
    {
        Application.Quit();
    }
}
