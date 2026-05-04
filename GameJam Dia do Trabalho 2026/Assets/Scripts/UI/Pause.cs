using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public static Pause s_Pause;

    [SerializeField] private GameObject _canvas;
    [SerializeField] private Slider _sliderSens, _sliderVol;
    [SerializeField] private TMP_Text _txtSliderSens, _txtSliderVol;
    [SerializeField] private InputActionReference _acaoPausar;

    [SerializeField] private AudioSource _fonteMusica;

    public bool Pausado => _canvas.activeSelf;

    void Awake()
    {
        s_Pause = this;
    }

    void Start()
    {
        _sliderSens.value = ConfigsEstaticas.s_Configs.Sens;
        _txtSliderSens.text = ConfigsEstaticas.s_Configs.Sens.ToString("0.00");

        _sliderVol.value = ConfigsEstaticas.s_Configs.Volum;
        _txtSliderVol.text = ConfigsEstaticas.s_Configs.Volum.ToString("0.00");
        _fonteMusica.volume = 1;
        _fonteMusica.outputAudioMixerGroup.audioMixer.SetFloat("MasterVolume", ConfigsEstaticas.s_Configs.VolumDB);

        _sliderSens.onValueChanged.AddListener((float val) =>
        {
            ConfigsEstaticas.s_Configs.Sens = val;
            _txtSliderSens.text = ConfigsEstaticas.s_Configs.Sens.ToString("0.00");
        });
        _sliderVol.onValueChanged.AddListener((float val) =>
        {
            ConfigsEstaticas.s_Configs.Volum = val;
            _txtSliderVol.text = ConfigsEstaticas.s_Configs.Volum.ToString("0.00");
            _fonteMusica.outputAudioMixerGroup.audioMixer.SetFloat("MasterVolume", ConfigsEstaticas.s_Configs.VolumDB);
        });

        _canvas.SetActive(false);
    }

    void Update()
    {
        if (_acaoPausar.action.WasPerformedThisFrame())
        {
            if (_canvas.activeSelf)
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            _canvas.SetActive(!_canvas.activeSelf);
        }
    }

    public void Continuar()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _canvas.SetActive(false);
    }

    public void Sair()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }
}
