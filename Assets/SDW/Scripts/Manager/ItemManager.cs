using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 내 아이템들을 관리하는 매니저 클래스
/// CSV 데이터를 로드하여 아이템 풀을 생성하고 관리
/// </summary>
public class ItemManager : MonoBehaviour
{
    [SerializeField] private ItemPrefabDatabaseSO m_itemPrefabDatabaseSO;
    [SerializeField] private GameObject m_sceneItemPrefab;

    private Dictionary<ItemEnName, ItemPool<SceneItem>> m_itemPools;

    /// <summary>
    /// 아이템별 오브젝트 풀 딕셔너리를 반환
    /// </summary>
    public Dictionary<ItemEnName, ItemPool<SceneItem>> ItemPools => m_itemPools;

    private Dictionary<ItemEnName, ItemDataSO> m_itemEnDataSO;

    /// <summary>
    /// 아이템 영어 이름별 ItemDataSO를 반환
    /// </summary>
    public Dictionary<ItemEnName, ItemDataSO> ItemEnDataSO => m_itemEnDataSO;

    /// <summary>
    /// CSV 데이터를 로드하여 아이템 데이터베이스와 오브젝트 풀을 초기화
    /// </summary>
    private void OnEnable()
    {
        if (m_itemPrefabDatabaseSO.Equals(null))
        {
            Debug.Log("Item Database SO가 연결되지 않았습니다.");
            return;
        }

        //# 확장자없이 파일 이름 문자열만 사용
        string[] itemLines = LoadCSV.LoadFromCsv("Item");
        var itemList = ReadDataFromLines(itemLines);

        m_itemPools = new Dictionary<ItemEnName, ItemPool<SceneItem>>();
        m_itemEnDataSO = new Dictionary<ItemEnName, ItemDataSO>();

        var parentObject = new GameObject();
        parentObject.name = "Items";

        foreach (var item in itemList)
        {
            //# Scriptable Object 생성
            var newItemDataSO = ScriptableObject.CreateInstance<ItemDataSO>();
            newItemDataSO.name = item.ItemId.ToString();


            //# string을 이용하여 enum을 읽어옴
            if (!Enum.TryParse<ItemType>(item.ItemType, true, out var itemType))
                itemType = ItemType.None; // 기본값 설정

            if (!Enum.TryParse<ItemEnName>(item.ItemEnName, true, out var itemEnName))
                itemEnName = ItemEnName.None; // 기본값 설정

            m_itemPools.Add(itemEnName, new ItemPool<SceneItem>());

            //# CSV에서 읽은 Data 연결
            newItemDataSO.ItemData.ItemId = item.ItemId;
            newItemDataSO.ItemData.ItemName = item.ItemName;
            newItemDataSO.ItemEnName = itemEnName;
            newItemDataSO.ItemType = itemType;
            newItemDataSO.ItemMaxOwn = item.ItemMaxOwn;
            newItemDataSO.ItemText = item.ItemText;

            m_itemPrefabDatabaseSO.OnSetPrefab(ref newItemDataSO);

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

            m_itemPools[newItemDataSO.ItemEnName].SetPool(itemObject);
            m_itemEnDataSO[newItemDataSO.ItemEnName] = newItemDataSO;
        }
    }

    /// <summary>
    /// CSV에서 읽어온 데이터를 ItemFileData 구조체에 맞게 변환
    /// </summary>
    /// <param name="lines">CSV 파일에서 읽어온 각 줄 데이터</param>
    /// <returns>각 열별로 구분된 ItemFileData</returns>
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