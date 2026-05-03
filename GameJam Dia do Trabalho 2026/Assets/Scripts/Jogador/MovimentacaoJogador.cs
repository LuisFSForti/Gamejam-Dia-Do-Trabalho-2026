using JetBrains.Annotations;
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
    private float _margemChaoPorcento = 0.05f, _multiplicadorForca = 10f;
    bool _cooldownPraPular, _noChao;
    
    private GameObject _ultimaPlataforma;
    private Vector3 _ultimaVelocidadePlataforma;

    private void Start()
    {
        _corpo = GetComponent<Rigidbody>();
        _colisorCorpoVisual = _corpoVisual.GetComponent<CapsuleCollider>();

        _cooldownPraPular = false;
        _noChao = false;

        _ultimaPlataforma = null;
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

        //Começa acima do chão porque SphereCast não detecta objetos que já estavam dentro
        _noChao = Physics.BoxCast(
            _colisorCorpoVisual.bounds.center,
            //Diminui levemente o tamanho da caixa para não detectar colisões laterais
            //Diminui muito em Y para fazer a leitura de forma precisa
            Vector3.Scale(_colisorCorpoVisual.bounds.extents, new Vector3(0.95f, 0.01f, 0.95f)),
            -_colisorCorpoVisual.transform.up,
            out RaycastHit hit,
            _colisorCorpoVisual.transform.rotation,
            _colisorCorpoVisual.bounds.extents.y * (1 + _margemChaoPorcento),
            _layersChao
        );

        bool emPlataforma = Physics.BoxCast(
            _colisorCorpoVisual.bounds.center,
            //Diminui levemente o tamanho da caixa para não detectar colisões laterais
            //Diminui muito em Y para fazer a leitura de forma precisa
            Vector3.Scale(_colisorCorpoVisual.bounds.extents, new Vector3(0.9f, 0.01f, 0.9f)),
            -_colisorCorpoVisual.transform.up,
            out hit,
            _colisorCorpoVisual.transform.rotation,
            _colisorCorpoVisual.bounds.extents.y * (1 + _margemChaoPorcento),
            _layersPlataforma
        );

        if (emPlataforma)
        {
            if (_ultimaPlataforma != hit.collider.gameObject)
            {
                _ultimaPlataforma = hit.collider.gameObject;
            }

            Rigidbody corpoPlataforma = hit.collider.GetComponent<Rigidbody>();
            Vector3 vel = corpoPlataforma.linearVelocity;

            Vector3 offset = transform.position - corpoPlataforma.worldCenterOfMass;
            Vector3 angular = Vector3.Cross(corpoPlataforma.angularVelocity, offset);

            Vector3 velocidadePlataforma = vel + angular;

            Vector3 delta = velocidadePlataforma - _ultimaVelocidadePlataforma;
            _corpo.linearVelocity += delta;

            _ultimaVelocidadePlataforma = velocidadePlataforma;
        }
        else if (_noChao)
        {
            _ultimaPlataforma = null;
            _ultimaVelocidadePlataforma = Vector3.zero;
        }

        Vector3 velocidadeRelativa = _corpo.linearVelocity - _ultimaVelocidadePlataforma;
        velocidadeRelativa.y = 0;
        
        if (_noChao)
        {
            if(velocidadeRelativa.magnitude < _velocidadeMovimento)
                _corpo.AddForce(movimento.normalized * _velocidadeMovimento * _multiplicadorForca, ForceMode.Acceleration);
        }
        else
        {
            //Se estiver no ar, tira parte do controle do jogador
            float puloForca = _velocidadeMovimento * _multiplicadorForca * _multNoAr;
            _corpo.AddForce(movimento.normalized * puloForca, ForceMode.Acceleration);

            float relacaoVelocidade = velocidadeRelativa.magnitude / _velocidadeMovimento;
            if (relacaoVelocidade > 0.1)
            {
                float resistencia = puloForca * relacaoVelocidade * relacaoVelocidade;
                _corpo.AddForce(-velocidadeRelativa.normalized * resistencia, ForceMode.Acceleration);
            }
        }

        if(_acaoPular.action.inProgress && !_cooldownPraPular && _noChao)
        {
            _cooldownPraPular = true;

            _corpo.linearVelocity = new Vector3(_corpo.linearVelocity.x, 0, _corpo.linearVelocity.z);
            _corpo.AddForce(_corpoVisual.up * _forcaPulo, ForceMode.VelocityChange);

            //Em _cooldownPulo segundos chama ResetJump()
            Invoke(nameof(ResetJump), _cooldownPulo);
        }
    }

    private void ResetJump()
    {
        _cooldownPraPular = false;
    }
}
