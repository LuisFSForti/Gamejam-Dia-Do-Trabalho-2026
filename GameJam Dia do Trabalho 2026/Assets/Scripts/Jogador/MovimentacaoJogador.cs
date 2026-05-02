using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class MovimentacaoJogador : MonoBehaviour
{
    [SerializeField] private float _velocidadeMovimento, _forcaPulo, _cooldownPulo, _multNoAr;
    [SerializeField] private Transform _corpoVisual;
    [SerializeField] private LayerMask _layersChao, _layersPlataforma;
    [SerializeField] private InputActionReference _acaoAndar, _acaoPular;

    [SerializeField] private float _alturaMorte;

    private Rigidbody _corpo;
    private CapsuleCollider _colisorCorpoVisual;
    private float _margemChao = 0.2f, _multiplicadorForca = 10f;
    bool _cooldownPraPular, _noChao;
    private Vector3 _ultimaVelocidadePlataforma;

    private void Start()
    {
        _corpo = GetComponent<Rigidbody>();
        _colisorCorpoVisual = _corpoVisual.GetComponent<CapsuleCollider>();

        _cooldownPraPular = false;
        _noChao = false;

        _ultimaVelocidadePlataforma = Vector3.zero;
    }

    void Update()
    {
        if (this.transform.position.y < _alturaMorte)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void FixedUpdate()
    {
        Vector2 inputJogador = _acaoAndar.action.ReadValue<Vector2>();
        Vector3 movimento = _corpoVisual.forward * inputJogador.y + _corpoVisual.right * inputJogador.x;

        _noChao = Physics.SphereCast(
            _colisorCorpoVisual.bounds.center,
            _colisorCorpoVisual.bounds.extents.x,
            Vector3.down,
            out RaycastHit hit,
            _colisorCorpoVisual.bounds.extents.y + _margemChao,
            _layersChao
        );

        bool emPlataforma = Physics.SphereCast(
            _colisorCorpoVisual.bounds.center,
            _colisorCorpoVisual.bounds.extents.x,
            Vector3.down,
            out hit,
            _colisorCorpoVisual.bounds.extents.y + _margemChao,
            _layersPlataforma
        );
        if (emPlataforma)
        {
            Rigidbody corpoPlataforma = hit.collider.GetComponent<Rigidbody>();
            Vector3 vel = corpoPlataforma.linearVelocity;

            Vector3 offset = transform.position - corpoPlataforma.worldCenterOfMass;
            Vector3 angular = Vector3.Cross(corpoPlataforma.angularVelocity, offset);

            Vector3 velocidadePlataforma = vel + angular;

            Vector3 delta = velocidadePlataforma - _ultimaVelocidadePlataforma;
            Debug.Log("Delta = " + delta + " - " + velocidadePlataforma + " - " + vel);
            _corpo.linearVelocity += delta;

            _ultimaVelocidadePlataforma = velocidadePlataforma;
        }
        else if (_ultimaVelocidadePlataforma != Vector3.zero)
        {
            Vector3 delta = _ultimaVelocidadePlataforma;
            _corpo.linearVelocity += delta;

            _ultimaVelocidadePlataforma = Vector3.zero;
        }

        if (_noChao)
        {
            _corpo.AddForce(movimento.normalized * _velocidadeMovimento * _multiplicadorForca, ForceMode.Acceleration);
        }
        else
        {
            //Se estiver no ar, tira parte do controle do jogador
            _corpo.AddForce(movimento.normalized * _velocidadeMovimento * _multiplicadorForca * _multNoAr, ForceMode.Acceleration);
        }

        Vector3 velocidadeAtual = new Vector3(_corpo.linearVelocity.x, 0f, _corpo.linearVelocity.z);
        if(velocidadeAtual.magnitude > _velocidadeMovimento)
        {
            Vector3 velocidadeLimitada = velocidadeAtual.normalized * _velocidadeMovimento;
            _corpo.linearVelocity = new Vector3(velocidadeLimitada.x, _corpo.linearVelocity.y, velocidadeLimitada.z);
        }

        if(_acaoPular.action.inProgress && !_cooldownPraPular && _noChao)
        {
            _cooldownPraPular = true;

            _corpo.AddForce(_corpoVisual.up * _forcaPulo, ForceMode.VelocityChange);

            //Em _cooldownPulo segundos chama ResetJump()
            Invoke(nameof(ResetJump), _cooldownPulo);
        }

        //Debug.Log(_corpo.linearVelocity.magnitude);
    }

    private void ResetJump()
    {
        _cooldownPraPular = false;
    }
}
