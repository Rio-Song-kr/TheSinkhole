using UnityEngine;
using UnityEngine.UI;

public class WheelUI : MonoBehaviour
{
    [SerializeField] [Range(1, 360)] private float m_spinSpeed;
    private Image m_fillImage;

    private void Awake()
    {
        var images = GetComponentsInChildren<Image>();
        m_fillImage = images.Length > 1 ? images[1] : images[0];
    }

    private void Update()
    {
        m_fillImage.transform.Rotate(0, 0, m_spinSpeed * Time.deltaTime);
    }
}