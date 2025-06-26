using UnityEngine;
using UnityEngine.UI;

public class CropBtn : MonoBehaviour
{
    private CropDataSO m_cropDataSO;
    [SerializeField]private Image m_image;
    public Button Btn;

    void Awake()
    {
        if (Btn == null)
            Btn = GetComponent<Button>();
        Btn.onClick.AddListener(OnUIDetail);

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
