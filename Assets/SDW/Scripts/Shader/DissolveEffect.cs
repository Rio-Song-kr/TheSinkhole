using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    private Renderer m_renderer;
    private Material m_material;
    public readonly float MinValue = 0.3f;
    public readonly float MaxValue = 1.5f;
    private bool m_isMaximum;
    private float m_valueRate = 0f;

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
        m_material = m_renderer.material;
        m_material.SetFloat("_Cutoff_Height", 0.3f);
    }

    public void Update()
    {
        if (m_isMaximum)
            m_valueRate -= 0.1f * Time.deltaTime * 10;
        else
            m_valueRate += 0.1f * Time.deltaTime * 10;
        SetCutoffValue(m_valueRate);

        if (m_valueRate <= 0f) m_isMaximum = false;
        else if (m_valueRate >= 1f) m_isMaximum = true;
    }

    public void SetCutoffValue(float valueRate)
    {
        float value = (MaxValue - MinValue) * valueRate + MinValue;
        m_material.SetFloat("_Cutoff_Height", value);
    }
}