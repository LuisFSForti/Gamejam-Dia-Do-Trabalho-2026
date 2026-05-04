using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorAnimacao : MonoBehaviour
{
    public static ControladorAnimacao s_ControladorAnimacao;

    private bool _jaAnimou;
    public bool JaAnimou => _jaAnimou;

    void Awake()
    {
        if (s_ControladorAnimacao != null)
            return;

        s_ControladorAnimacao = this;
        _jaAnimou = false;

        DontDestroyOnLoad(this.gameObject);
        SceneManager.activeSceneChanged += CenaMudou;
    }

    public void AnimacaoFeita()
    {
        _jaAnimou = true;
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
