using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Jogar()
    {
        SceneManager.LoadScene(1);
    }

    public void Sair()
    {
        Application.Quit();
    }
}
