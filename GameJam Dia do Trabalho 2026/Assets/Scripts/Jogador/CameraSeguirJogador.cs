using UnityEngine;

public class CameraSeguirJogador : MonoBehaviour
{
    [SerializeField] private Transform _olhos;

    void Update()
    {
        this.transform.position = _olhos.position;
    }
}
