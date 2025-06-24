using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogBox
{
    [System.Serializable]
    public struct UiElement
    {
        public string key;
        public GameObject go;
    }

    public class DialogBoxPrefabDeliver : MonoBehaviour
    {
        [Header("������ ������Ʈ��")]
        [SerializeField] private UiElement[] mElements; //��Ÿ�� ���� ����� ������Ʈ

        private Dictionary<string, GameObject> mElementsDictionary = new Dictionary<string, GameObject>(); //��Ÿ�� ���� ã�� ��Ҹ� ������ ��ųʸ�

        /// <summary>
        /// �����ڰ� �̺�Ʈ�� ������ ���ù�, null�ϰ�� �̺�Ʈ�� �������� ����
        /// </summary>
        [HideInInspector] public DialogBoxController EventReciever = null; //Deliver �����տ��� ȣ��Ǵ� �̺�Ʈ�� ȣ���� ���̾�α׹ڽ� ��Ʈ�ѷ�

        private void Awake()
        {
            //�ν����Ϳ��� ������ ��ҵ��� ����ִ´�.
            foreach (UiElement element in mElements) { mElementsDictionary.Add(element.key, element.go); }
        }

        public T GetElement<T>(string key) { return mElementsDictionary[key].GetComponent<T>(); }

        public void Event_Deliver(string eventID)
        {
            if (EventReciever != null) { EventReciever.EventTrigger(eventID); }
        }
    }
}
