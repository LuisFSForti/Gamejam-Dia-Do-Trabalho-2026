using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectarVitoria : MonoBehaviour
{
    [SerializeField] private GameObject _jogador;
    [SerializeField] private float _distanciaVitoria;

    void Update()
    {
        if (Vector3.Distance(_jogador.transform.position, this.transform.position) <= _distanciaVitoria)
        {
            SceneManager.LoadScene(2);
        }
    }
}
