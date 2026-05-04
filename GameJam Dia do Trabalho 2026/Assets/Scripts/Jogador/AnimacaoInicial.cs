using UnityEngine;

public class AnimacaoInicial : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _canvasIntro;
    [SerializeField] private Transform _visualJogador, _posicaoConstrutores;
    [SerializeField] private MovimentacaoJogador _scriptMovimento;
    [SerializeField] private Pause _scriptPause;
    [SerializeField] private float _duracaoZoom, _duracaoTransicao, _fovZoom, _fovNormal;

    private float _tempoPassado;

    void Start()
    {
        _visualJogador.forward = (_posicaoConstrutores.position - _visualJogador.position).normalized;
        _mainCamera.transform.forward = (_posicaoConstrutores.position - _visualJogador.position).normalized;

        if (ControladorAnimacao.s_ControladorAnimacao.JaAnimou)
        {
            _scriptMovimento.enabled = true;
            _scriptPause.enabled = true;
            _mainCamera.GetComponent<CameraJogador>().enabled = true;
            _mainCamera.GetComponent<CameraJogador>().DefinirRotacao(_visualJogador.rotation);
            _mainCamera.fieldOfView = _fovNormal;

            _canvasIntro.SetActive(false);

            Destroy(this);
        }
        else
        {
            _tempoPassado = 0;

            _scriptMovimento.enabled = false;
            _scriptPause.enabled = false;
            _mainCamera.GetComponent<CameraJogador>().enabled = false;
            _mainCamera.fieldOfView = _fovZoom;

            _canvasIntro.SetActive(true);
        }
    }

    void Update()
    {
        _tempoPassado += Time.deltaTime;

        if (_tempoPassado >= _duracaoZoom)
        {
            _mainCamera.fieldOfView = Mathf.Lerp(_fovZoom, _fovNormal, (_tempoPassado - _duracaoZoom)/_duracaoTransicao);

            if (_tempoPassado >= _duracaoZoom + _duracaoTransicao)
            {
                _scriptMovimento.enabled = true;
                _scriptPause.enabled = true;
                _mainCamera.GetComponent<CameraJogador>().enabled = true;
                _mainCamera.GetComponent<CameraJogador>().DefinirRotacao(_visualJogador.rotation);
                _mainCamera.fieldOfView = _fovNormal;
                
                _canvasIntro.SetActive(false);

                ControladorAnimacao.s_ControladorAnimacao.AnimacaoFeita();
                Destroy(this);
            }
        }
    }
}
