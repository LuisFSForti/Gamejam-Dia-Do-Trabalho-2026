using UnityEngine;

public class ConfigsEstaticas : MonoBehaviour
{
    public static ConfigsEstaticas s_Configs;

    private float _sens = 0.2f;
    private float _volum = 1;

    public float Sens
    {
        get
        {
            return _sens;
        }
        set
        {
            if (value < 0 || value > 1)
                return;
            else
                _sens = value;
        }
    }

    public float Volum
    {
        get
        {
            return _volum;
        }
        set
        {
            if (value < 0 || value > 1)
                return;
            else
                _volum = value;
        }
    }

    public float VolumDB
    {
        get
        {
            float volume = Mathf.Clamp(_volum, 0.0001f, 1f);
            return Mathf.Log10(volume) * 20;
        }
    }

    void Awake()
    {
        if (s_Configs != null)
        {
            Destroy(this.gameObject);
            return;
        }

        s_Configs = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
