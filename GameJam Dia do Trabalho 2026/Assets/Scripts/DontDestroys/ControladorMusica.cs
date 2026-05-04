using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMusica : MonoBehaviour
{
    public static ControladorMusica s_ControladorAnimacao;

    void Awake()
    {
        if (s_ControladorAnimacao != null)
        {
            Destroy(this.gameObject);
            return;
        }

        s_ControladorAnimacao = this;

        DontDestroyOnLoad(this.gameObject);
        SceneManager.activeSceneChanged += CenaMudou;
    }

    private void CenaMudou(Scene cena1, Scene cena2)
    {
        if (cena2.buildIndex != 1)
        {
            SceneManager.activeSceneChanged -= CenaMudou;
            Destroy(this.gameObject);
        }
    }
}
