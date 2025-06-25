using UnityEngine;
using UnityEngine.UI;

public class CropBtn : MonoBehaviour
{
    public CropDataSO cropDataSO;
    public Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnUIDetail);
    }

    private void OnUIDetail()
    {
        // FarmUI.Instance.DisplayCropDetail(cropDataSO);
        FarmUI.Instance.SelectCrop(cropDataSO);
    }
}
