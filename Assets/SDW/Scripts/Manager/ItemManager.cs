using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 내 아이템들을 관리하는 매니저 클래스
/// CSV 데이터를 로드하여 아이템 풀을 생성하고 관리
/// </summary>
public class ItemManager : MonoBehaviour
{
    [SerializeField] private ItemDatabaseSO m_itemDatabaseSO;
    [SerializeField] private GameObject m_sceneItemPrefab;

    //todo 테스트를 위한 prefab 추후 변경 예정
    // [SerializeField] private GameObject _redPrefab;
    // [SerializeField] private GameObject _greenPrefab;

    private Dictionary<int, ItemPool<SceneItem>> m_itemPools;

    /// <summary>
    /// 아이템별 오브젝트 풀 딕셔너리를 반환
    /// </summary>
    public Dictionary<int, ItemPool<SceneItem>> ItemPools => m_itemPools;

    /// <summary>
    /// CSV 데이터를 로드하여 아이템 데이터베이스와 오브젝트 풀을 초기화합니다
    /// </summary>
    private void OnEnable()
    {
        if (m_itemDatabaseSO.Equals(null))
        {
            Debug.Log("Item Database SO가 연결되지 않았습니다.");
            return;
        }

        //# 확장자 없이 파일 이름 문자열만 사용
        string[] itemLines = LoadCSV.LoadFromCsv("Item");
        var itemList = ReadDataFromLines(itemLines);

        m_itemPools = new Dictionary<int, ItemPool<SceneItem>>();

        var parentObject = new GameObject();
        parentObject.name = "Items";

        for (int i = 0; i < itemList.Count; i++)
        {
            //# Scriptable Object 생성
            var newItemDataSO = ScriptableObject.CreateInstance<ItemDataSO>();
            newItemDataSO.name = itemList[i].ItemId.ToString();

            m_itemPools.Add(itemList[i].ItemId, new ItemPool<SceneItem>());

            //# string을 이용하여 enum을 읽어옴
            if (!Enum.TryParse<ItemType>(itemList[i].ItemType, true, out var itemType))
                itemType = ItemType.None; // 기본값 설정

            if (!Enum.TryParse<ItemEnName>(itemList[i].ItemEnName, true, out var itemEnName))
                itemEnName = ItemEnName.None; // 기본값 설정

            //# CSV에서 읽은 Data 연결
            newItemDataSO.ItemData.ItemId = itemList[i].ItemId;
            newItemDataSO.ItemData.ItemName = itemList[i].ItemName;
            newItemDataSO.ItemEnName = itemEnName;
            newItemDataSO.ItemType = itemType;
            newItemDataSO.ItemMaxOwn = itemList[i].ItemMaxOwn;
            newItemDataSO.ItemText = itemList[i].ItemText;

            //todo 테스트를 위한 prefab 추후 변경 예정
            // newItemDataSO.ModelPrefab = i % 2 == 0 ? _redPrefab : _greenPrefab;

            m_itemDatabaseSO.OnSetPrefab(ref newItemDataSO);

            //todo 데이터 초기화 및 모델, Icon 등 연결

            var itemObject = Instantiate(m_sceneItemPrefab);
            itemObject.transform.parent = parentObject.transform;
            itemObject.tag = "Item";

            var itemModel = Instantiate(newItemDataSO.ModelPrefab);
            itemModel.transform.parent = itemObject.transform;

            //# sceneItem으로 캐스팅
            var sceneItem = itemObject.GetComponent<SceneItem>();
            sceneItem.ItemDataSO = newItemDataSO;
            sceneItem.SetupColliderSize();

            itemObject.SetActive(false);

            m_itemPools[itemList[i].ItemId].SetPool(itemObject);
        }
    }

    private List<ItemFileData> ReadDataFromLines(string[] lines)
    {
        var dataList = new List<ItemFileData>();

        // for (int i = 0; i < lines.Length; i++)
        foreach (string line in lines)
        {
            string[] fields = line.Split(',');

            if (fields.Length >= 7)
            {
                var data = new ItemFileData(fields);

                dataList.Add(data);
            }
        }

        return dataList;
    }
}