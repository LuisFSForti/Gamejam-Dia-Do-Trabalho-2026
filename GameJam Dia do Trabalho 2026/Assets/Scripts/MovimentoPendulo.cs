using Unity.VisualScripting;
using UnityEngine;

public class MovimentoPendulo : MonoBehaviour
{
    [SerializeField] private float _forcaX, _offsetTemporal;
    [SerializeField] private Rigidbody _corpo;
    private float _energiaInicial, _tempoAteColetar = 2;
    private bool _lancado;

    void Start()
    {
        _lancado = false;
        Invoke(nameof(Lancar), _offsetTemporal);
    }

    private void Lancar()
    {
        _corpo.AddForce(new Vector3(_forcaX, 0, 0), ForceMode.VelocityChange);
        _lancado = true;
    }

    void FixedUpdate()
    {
        if (!_lancado)
            return;

        float altura = _corpo.position.y;
        float velocidade = _corpo.linearVelocity.magnitude;

        float energiaAtual = altura + (velocidade * velocidade) / (2f * Physics.gravity.magnitude);
        //Força o código a esperar alguns frames antes de salvar a "energia inicial"
        //Assim dá tempo o suficiente pra física calcular adequadamente
        if (_tempoAteColetar > 0)
        {
            _energiaInicial = energiaAtual;
            _tempoAteColetar--;
        }

        float diferenca = _energiaInicial - energiaAtual;

        if (Mathf.Abs(diferenca) > 0.1f)
        {
            Vector3 direcao = _corpo.linearVelocity.normalized;
            _corpo.AddForce(direcao * diferenca * 10f, ForceMode.Acceleration);
        }
    }
}
