using UnityEngine;

public class Sensibilidade : MonoBehaviour
{
    public static Sensibilidade s_Sensbilidade;

    private float _sens = 0.2f;
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

    void Awake()
    {
        if (s_Sensbilidade != null)
        {
            Destroy(this.gameObject);
            return;
        }

        s_Sensbilidade = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
