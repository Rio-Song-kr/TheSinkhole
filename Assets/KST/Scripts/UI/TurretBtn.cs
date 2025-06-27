using UnityEngine;
using UnityEngine.UI;

public class TurretBtn : MonoBehaviour
{
    private TurretSo m_turretSO;
    [SerializeField]private Image m_image;
    public Button Btn;

    void Awake()
    {
        if (Btn == null)
            Btn = GetComponent<Button>();
        Btn.onClick.AddListener(OnUIDetail);

    }
    public void Init(TurretSo so)
    {
        m_turretSO = so;
        m_image.sprite = so.TurretImg;
    }

    private void OnUIDetail()
    {
        TurretUI.Instance.SelectTurret(m_turretSO);
    }
}
