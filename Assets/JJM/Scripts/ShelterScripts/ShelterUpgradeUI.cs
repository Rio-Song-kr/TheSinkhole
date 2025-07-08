using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShelterUpgradeUI : MonoBehaviour
{
    [Header("UI 연결")]
    public GameObject shelterCanvas; // Shelter UI 캔버스
    public TextMeshProUGUI timeText; // 업그레이드 시간 텍스트
    public TextMeshProUGUI descriptionText; // 업그레이드 설명 텍스트

    [Header("재료 슬롯 UI")]
    public List<IngreSlot> slots; //ReqIngre0~3연결;

    [Header("업그레이드 버튼")]
    public Button enforceButton; // 업그레이드 버튼
    public TextMeshProUGUI enforceButtonText; // 업그레이드 버튼 텍스트

    private Dictionary<ItemEnName, List<int>> matDic; //재료 정보 저장
    private ShelterUpgrade shelterUpgrade; // ShelterUpgrade 컴포넌트
    private static bool m_isUICloseButtonPressed;

    private void Start()
    {
        enforceButton.onClick.AddListener(OnClickUpgrade);
        Hide(); //시작시 숨김
    }

    private void Update()
    {
        if (!m_isUICloseButtonPressed) return;
        m_isUICloseButtonPressed = false;

        Hide();
    }

    //UI열기
    public void Show(Dictionary<ItemEnName, List<int>> mat, ShelterUpgrade upgrade)
    {
        matDic = mat;
        shelterUpgrade = upgrade;

        shelterCanvas.SetActive(true); // Shelter UI 캔버스 활성화
        RefreshSlots(); //재료 표시
        SetTexts(); //상단 텍스트 세팅
    }

    //UI 닫기
    public void Hide()
    {
        shelterCanvas.SetActive(false); // Shelter UI 캔버스 비활성화
        timeText.text = "5 seconds"; //시간 초기화
    }

    //상단 텍스트 설정
    private void SetTexts()
    {
        int level = shelterUpgrade.GetCurrentLevel();

        descriptionText.text = $"현재 레벨 {level}\n"
                               + $"현재 내구도: {shelterUpgrade.GetCurrentDurability()} / {shelterUpgrade.GetMaxDurability()}\n\n\n";

        int nextLevelId = GetNextLevelId(level);
        if (nextLevelId == -1) return;

        int nextMaxDurability = GetNextLevelMaxDurability(nextLevelId);
        int afterDurability = nextMaxDurability + shelterUpgrade.GetCurrentDurability() - shelterUpgrade.GetMaxDurability();
        descriptionText.text += $"다음 레벨 {level + 1}\n"
                                + $"다음 내구도: {afterDurability} / {nextMaxDurability}\n";
        // 업그레이드 설명 텍스트 설정
    }

    private int GetNextLevelId(int levelId)
    {
        int nextLevel = shelterUpgrade.GetCurrentLevel() + 1;
        if (GameManager.Instance.Shelter.ShelterLevelToId.ContainsKey(nextLevel))
            return GameManager.Instance.Shelter.ShelterLevelToId[nextLevel];
        return -1;
    }

    private int GetNextLevelMaxDurability(int levelId) =>
        GameManager.Instance.Shelter.ShelterLevelData[levelId].ShelterDurability;

    private void RefreshSlots()
    {
        bool canUpgrade = true; // 업그레이드 가능 여부 초기화
        int i = 0;

        foreach (var keyValue in matDic)
        {
            if (i >= slots.Count)
            {
                break;
            }
            var enName = keyValue.Key;
            int have = keyValue.Value[0]; // 현재 재료 보유량
            int need = keyValue.Value[1]; // 업그레이드에 필요한 재료량

            var itemData = GameManager.Instance.Item.ItemEnDataSO[enName];

            slots[i].Set(itemData.Icon, itemData.ItemData.ItemName, have, need); // 슬롯에 재료 정보 설정
            slots[i].gameObject.SetActive(true);
            if (have < need) // 보유량이 필요한 양보다 적으면 업그레이드 불가능
            {
                canUpgrade = false;
            }
            i++;
        }
        enforceButton.interactable = canUpgrade; // 업그레이드 버튼 활성화/비활성화
    }

    //버튼 클릭
    private void OnClickUpgrade()
    {
        enforceButton.interactable = false; // 버튼 비활성화
        enforceButtonText.text = "업그레이드 중..."; // 버튼 텍스트 변경
        StartCoroutine(UpgradeCoroutine()); // 업그레이드 코루틴 시작
    }
    //5초후 강화 완료
    private IEnumerator UpgradeCoroutine()
    {
        float wait = 5f; // 대기 시간 5초
        while (wait > 0f)
        {
            timeText.text = $"{wait:F0} seconds"; // 남은 시간 표시
            yield return new WaitForSeconds(1f); // 1초 대기
            wait -= 1f; // 남은 시간 감소
        }

        shelterUpgrade.ProgressUpgrade(); // 업그레이드 진행
        enforceButtonText.text = "업그레이드 완료"; // 버튼 텍스트 변경
        yield return new WaitForSeconds(1f); // 1초 대기
        enforceButtonText.text = "업그레이드"; // 버튼 텍스트 초기화

        GameManager.Instance.SetCursorLock();
        ;
        Hide(); // UI 닫기
    }

    public static void OnUICloseKeyPressed() => m_isUICloseButtonPressed = true;
    public static void OnUICloseKeyReleased() => m_isUICloseButtonPressed = false;
}