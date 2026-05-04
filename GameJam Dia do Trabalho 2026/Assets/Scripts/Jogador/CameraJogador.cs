using UnityEngine;
using UnityEngine.InputSystem;

public class CameraJogador : MonoBehaviour
{
    [SerializeField] private Transform _corpo;
    [SerializeField] private InputActionReference _acaoOlhar;

    private float _rotacaoEixoX, _rotacaoEixoY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DefinirRotacao(Quaternion rotacao)
    {
        _rotacaoEixoX = rotacao.eulerAngles.x;
        _rotacaoEixoY = rotacao.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Pause.s_Pause.Pausado)
            return;

        Vector2 movimentoMouse = _acaoOlhar.action.ReadValue<Vector2>() * Sensibilidade.s_Sensbilidade.Sens;

        _rotacaoEixoY += movimentoMouse.x;
        _rotacaoEixoX -= movimentoMouse.y;
        _rotacaoEixoX = Mathf.Clamp(_rotacaoEixoX, -90, 90);

        transform.rotation = Quaternion.Euler(_rotacaoEixoX, _rotacaoEixoY, 0);
        _corpo.rotation = Quaternion.Euler(0, _rotacaoEixoY, 0);
    }
}
