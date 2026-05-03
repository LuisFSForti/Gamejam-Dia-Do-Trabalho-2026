using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public static Pause s_Pause;

    [SerializeField] private GameObject _canvas;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _txtSlider;
    [SerializeField] private InputActionReference _acaoPausar;

    public bool Pausado => _canvas.activeSelf;

    void Awake()
    {
        s_Pause = this;
    }

    void Start()
    {
        _slider.value = Sensibilidade.s_Sensbilidade.Sens;
        _txtSlider.text = Sensibilidade.s_Sensbilidade.Sens.ToString("0.00");

        _slider.onValueChanged.AddListener((float val) =>
        {
            Sensibilidade.s_Sensbilidade.Sens = val;
            _txtSlider.text = val.ToString("0.00");
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
