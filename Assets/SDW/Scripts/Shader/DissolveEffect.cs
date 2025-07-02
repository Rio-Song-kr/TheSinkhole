using System.Collections;
using UnityEngine;

/// <summary>
/// Tile(터렛 등) 설치 시 Dissolve 효과를 제어하기 위한 클래스
/// </summary>
public class DissolveEffect : MonoBehaviour
{
    [SerializeField] private float m_dissolveTime = 5f;
    private Renderer m_renderer;
    private Material m_material;
    private readonly float MinValue = 0.3f;
    private readonly float MaxValue = 1.5f;
    public bool IsDone;

    private void Awake()
    {
        m_renderer = GetComponentInChildren<Renderer>();
        m_material = m_renderer.material;
        m_material.SetFloat("_Cutoff_Height", 0.3f);
    }

    private void Start() => StartCoroutine(SetCutoffValue());

    private IEnumerator SetCutoffValue()
    {
        float time = 0;
        float value = 0;

        do
        {
            time += Time.deltaTime;
            value += (MaxValue - MinValue) / m_dissolveTime * Time.deltaTime;
            m_material.SetFloat("_Cutoff_Height", value);
            yield return null;
        } while (time < m_dissolveTime);

        IsDone = true;
    }
}