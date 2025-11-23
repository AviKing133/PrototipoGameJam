using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void CambioEscena()
    {
        SceneManager.LoadScene("Animacio");
    }
    public void SalirJuego()
    {
        Application.Quit();
    }
}
