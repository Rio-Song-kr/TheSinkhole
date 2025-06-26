using UnityEngine;
using UnityEngine.UI;

public class CropBtn : MonoBehaviour
{
    private CropDataSO m_cropDataSO;
    [SerializeField]private Image m_image;
    private Button m_button;

    void Awake()
    {
        if (m_button == null)
            m_button = GetComponent<Button>();
        m_button.onClick.AddListener(OnUIDetail);

    }
    public void Init(CropDataSO cropDataSO)
    {
        m_cropDataSO = cropDataSO;
        m_image.sprite = cropDataSO.cropImg;
    }

    private void OnUIDetail()
    {
        FarmUI.Instance.SelectCrop(m_cropDataSO);
    }
}
