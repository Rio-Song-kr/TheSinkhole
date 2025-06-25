using UnityEngine;

public class Crop : MonoBehaviour
{
    public float GrowTime = 3f;
    public GameObject GrownCropPrefab;
    private float m_timer = 0f;
    private bool m_isGrowing = false;
    void Update()
    {
        if (!m_isGrowing) return;

        m_timer += Time.deltaTime;
        if (m_timer >= GrowTime)
        {
            Grow();
        }
    }

    public void StartGrow()
    {
        m_isGrowing = true;
        m_timer = 0f;
    }
    void Grow()
    {
        //씨앗 -> 작물로 변경
        Instantiate(GrownCropPrefab, transform.position, Quaternion.identity);
        //씨앗은 파괴.
        Destroy(gameObject);
    }

}