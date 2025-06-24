using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogBox
{
    #region ������ �ɼ�
    /// <summary>
    /// ���Ŀɼ�
    /// </summary>
    
    public enum Align
    {
        /// <summary>
        /// ����
        /// </summary>
        LEFT,
        /// <summary>
        /// ������
        /// </summary>
        RIGHT,
        /// <summary>
        /// ���
        /// </summary>
        CENTER,
        /// <summary>
        /// ��� Ȯ�� (�¿�� �ִ�ġ)
        /// </summary>
        EXPAND,
    }

    /// <summary>
    /// �Է��ʵ� �̺�Ʈ
    /// </summary>
     
    public enum InputFieldEvent
    {
        /// <summary>
        /// �Է°��� ����� ��
        /// </summary>

        OnValueChanged,

        /// <summary>
        /// ������ ������� (��Ŀ�� ����)
        /// </summary>

        OnEndEdit,

        /// <summary>
        /// ���õ� ��
        /// </summary>

        OnSelect,

        /// <summary>
        /// ������ ���� ��
        /// </summary>

        OnDeselect,
    }

    #endregion
    public class DialogBoxController : MonoBehaviour
    {
        private const int BORDER_GAP = 20; //������ ���� ������ �������� �����ϴ� ũ��

        public const string RESERVED_EVENT_CLOSE = "RESERVED_EVENT_CLOSE"; //�ݱ� �̺�Ʈ �����
        public const string RESERVED_EVENT_ON_DESTROY = "RESERVED_EVENT_ON_DESTROY"; //�ı� �̺�Ʈ �����

        
        [Space(50)]
        [Header("���̾�α� �ڽ� ��Ʈ(ĵ���� child)")]
        [SerializeField] private GameObject mDialogBoxRoot;

        [Space(50)]
        [Header("Ÿ��Ʋ ��")]
        [SerializeField] private TextMeshProUGUI mTitleLabel;

        [Space(50)]
        [Header("�߰��� �ؽ�Ʈ �� ������")]
        [SerializeField] private GameObject mTextLabelPrefab;

        [Header("�߰��� ��ư ������")]
        [SerializeField] private GameObject mButtonPrefab;

        [Header("�߰��� ����(�̹���) ������")]
        [SerializeField] private GameObject mBorderPrefab;

        [Header("�߰��� �Է��ʵ� ������")]
        [SerializeField] private GameObject mInputFieldPrefab;

        [Space(50)]
        [Header("���(Ÿ��Ʋ) �� �߰��� ������Ʈ���� �θ� Ʈ������")]
        [SerializeField] private RectTransform mTopContentsParent;

        [Header("������ ������ �߰��� ������Ʈ���� �θ� Ʈ������")]
        [SerializeField] private RectTransform mCenterContentsParent;

        [Header("�ϴ� ��ȣ�ۿ� ������ �߰��� ������Ʈ���� �θ� Ʈ������")]
        [SerializeField] private RectTransform mBottomContentsParent;

        [Space(50)]
        [Header("������ ������ �ֻ��� �θ� Ʈ������")]
        [SerializeField] private RectTransform mContentsScrollViewRoot;

        private Vector2Int mDialogBoxRootSize; //Ȱ��ȭ �� ���̾�α׹ڽ��� ������
        private Action<DialogBoxController, string> mEventAction;//�� ���̾�α׹ڽ��� ȣ���� �̺�Ʈ
        private Dictionary<string, DialogBoxController> mDestroyCallEvents = new Dictionary<string, DialogBoxController>();//���̾�α׹ڽ��� �ı��ɶ� ȣ���� �̺�Ʈ
        private Dictionary<string, DialogBoxController> mReferenceDialogBoxes = new Dictionary<string, DialogBoxController>(); //������ ���̾�α� �ڽ�

        #region �ν��Ͻ��� ������Ʈ
        private Dictionary<string, GameObject> mInstantiatedObjects = new Dictionary<string, GameObject>(); //�ν��Ͻ��� �Է��ʵ��
        #endregion

        /// <summary>
        /// ���̾�α׹ڽ��� �ʱ�ȭ�մϴ�.
        /// </summary>
        /// <param name="boxWidth">����ũ�� (0 ~ 800)</param>
        /// <param name="boxHeight">����ũ�� (0 ~ 600)</param>
        /// <param name="eventAction">���̾�α׹ڽ� ��ҵ��� �̺�Ʈ�� ȣ���Ұ�� ���޵� �븮��</param>
        
        public void InitDialogBox(int boxWidth, int boxHeight, Action<DialogBoxController, string> eventAction = null)
        {
            //ũ�� ����
            RectTransform rootRectTransform = mDialogBoxRoot.GetComponent<RectTransform>();
            rootRectTransform.sizeDelta = mDialogBoxRootSize = new Vector2Int(boxWidth, boxHeight);
            RefreshBoxSize();
            ToggleDialogBox(true);

            //�̺�Ʈ ��ư ���� (�ִ°��)
            if (eventAction != null)
            {
                mEventAction += eventAction; //�̺�Ʈ ���
            }
        }
        #region ��� �߰�
        /// <summary>
        /// �ؽ�Ʈ ���̺� �߰�
        /// </summary>
        /// <param name="key">���۷����� ���� Ű</param>
        /// <param name="isEnableWhenStart">���̾�α׹ڽ��� Ȱ��ȭ�ɶ� ���� Ȱ��ȭ�Ǵ°�?</param>
        /// <param name="text">�Է��� �ؽ�Ʈ</param>
        /// <param name="fontSize">��Ʈ ũ��</param>
        /// <param name="alignOption">�ؽ�Ʈ ���� �ɼ�</param>
        /// <returns></returns>
        public TextMeshProUGUI AddText(string key, bool isEnableWhenStart, string text, float fontSize, TextAlignmentOptions alignOption = TextAlignmentOptions.Left)
        {
            GameObject newText = Instantiate(mTextLabelPrefab, Vector3.zero, Quaternion.identity, mCenterContentsParent);
            TextMeshProUGUI currentText = newText.GetComponent<TextMeshProUGUI>();

            //�ؽ�Ʈ ����
            currentText.text = text;
            currentText.fontSize = fontSize / 1.45f; //1.45�� ������� �ؽ�Ʈ�� ũ�⺸��
            currentText.alignment = alignOption;

            //�ؽ�Ʈ�� ũ�⸦ ���� ���������� ũ�⿡ ����
            newText.GetComponent<RectTransform>().sizeDelta = new Vector2Int(mDialogBoxRootSize.x - BORDER_GAP, 0);

            //��ųʸ��� ����
            AddInstantiatedObject(key, newText);

            //�ؽ�Ʈ Ȱ��ȭ
            newText.SetActive(isEnableWhenStart);
            return currentText;
        }

        /// <summary>
        /// ��ư�� �߰�
        /// </summary>
        /// <param name="key">���۷����� ���� Ű</param>
        /// <param name="isEnableWhenStart">���̾�α׹ڽ��� Ȱ��ȭ�ɶ� ���� Ȱ��ȭ�Ǵ°�?</param>
        /// <param name="text">��ư�� ������ �ؽ�Ʈ</param>
        /// <param name="eventID">��ư�� ������ ȣ��� �̺�Ʈ ID</param>
        /// <param name="targetTransform">� Ʈ�������� �ڽ����� ��ġ�Ұ��ΰ�? (null, ����������)</param>
        /// <returns></returns>
        public Button AddButton(string key, bool isEnableWhenStart, string text, string eventID, Transform targetTransform = null)
        {
            GameObject newButton = Instantiate(mButtonPrefab, Vector3.zero, Quaternion.identity, targetTransform == null ? mBottomContentsParent : targetTransform);

            //������Ʈ ȹ��
            Button currentButton = newButton.GetComponent<Button>();

            newButton.GetComponentInChildren<TextMeshProUGUI>(true).text = text;
            currentButton.onClick.AddListener(() => EventTrigger(eventID));

            //��ųʸ��� ����
            AddInstantiatedObject(key, newButton);

            //Ȱ��ȭ
            newButton.SetActive(isEnableWhenStart);
            return currentButton;
        }

        /// <summary>
        /// ��ҵ� �� ������ �߰�
        /// </summary>
        /// <param name="key">���۷����� ���� Ű</param>
        /// <param name="isEnableWhenStart">���̾�α׹ڽ��� Ȱ��ȭ�ɶ� ���� Ȱ��ȭ�Ǵ°�?</param>
        /// <param name="height">����</param>
        /// <param name="isCenter">������ ������ �߰��ϴ°�? (false �ϴ� ����)</param>
        /// <returns></returns>
        public GameObject AddBorder(string key, bool isEnableWhenStart, int height, bool isCenter)
        {
            GameObject newBorder = Instantiate(mBorderPrefab, Vector3.zero, Quaternion.identity, isCenter ? mCenterContentsParent : mBottomContentsParent);

            Image borderImage = newBorder.GetComponent<Image>();
            borderImage.sprite = Sprite.Create(new Texture2D(height, height, TextureFormat.RGB24, false), new Rect(0, 0, height, height), new Vector2(0f, 0f), 100.0f);
            borderImage.type = Image.Type.Simple;
            newBorder.SetActive(isEnableWhenStart);

            //��ųʸ��� ����
            AddInstantiatedObject(key, newBorder);

            return newBorder;
        }

        /// <summary>
        /// �̹����� �߰�
        /// </summary>
        /// <param name="key">���۷����� ���� Ű</param>
        /// <param name="isEnableWhenStart">���̾�α׹ڽ��� Ȱ��ȭ�ɶ� ���� Ȱ��ȭ�Ǵ°�?</param>
        /// <param name="spriteImage">�ν��Ͻ� �� ��������Ʈ �̹���</param>
        /// <param name="imageHeight">�̹����� ����</param>
        /// <param name="alignOption">�̹����� ��ġ ���� �ɼ�</param>
        /// <returns></returns>
        public Image AddImage(string key, bool isEnableWhenStart, Sprite spriteImage, int imageHeight, Align alignOption)
        {
            GameObject newBorder = AddBorder(null, isEnableWhenStart, imageHeight, true); //�̹����� ����ϱ����� ���� ����

            //�̹��� ���� ����
            GameObject newImage = Instantiate(mBorderPrefab, Vector3.zero, Quaternion.identity, mCenterContentsParent);
            newImage.transform.SetParent(newBorder.transform);

            //������Ʈ ȹ��
            Image currentImage = newImage.GetComponent<Image>();
            RectTransform currentTransform = newImage.GetComponent<RectTransform>();

            //������ �̹����� ����� ������� �������� ũ�� ����
            float sizeCorrectionDelta = spriteImage.rect.size.y / imageHeight;
            currentTransform.sizeDelta = spriteImage.rect.size / sizeCorrectionDelta;
            currentImage.sprite = spriteImage;

            //�̹��� ��ġ ����
            switch (alignOption)
            {
                case Align.LEFT:
                    {
                        currentTransform.pivot = new Vector2(0, 0.5f);
                        currentTransform.localPosition = Vector3.zero;
                        break;
                    }
                case Align.RIGHT:
                    {
                        currentTransform.pivot = new Vector2(1, 0.5f);
                        currentTransform.localPosition = new Vector2(mDialogBoxRootSize.x - BORDER_GAP, 0f);
                        break;
                    }
                case Align.CENTER:
                    {
                        currentTransform.pivot = new Vector2(0.5f, 0.5f);
                        currentTransform.localPosition = new Vector2((mDialogBoxRootSize.x - BORDER_GAP) * 0.5f, 0f);
                        break;
                    }
                case Align.EXPAND:
                    {
                        currentTransform.pivot = new Vector2(0.5f, 0.5f);
                        currentTransform.localPosition = new Vector2((mDialogBoxRootSize.x - BORDER_GAP) * 0.5f, 0f);
                        currentTransform.sizeDelta = new Vector2Int(mDialogBoxRootSize.x - BORDER_GAP, imageHeight);
                        break;
                    }
            }

            //��ųʸ��� ����
            AddInstantiatedObject(key, newImage);

            //�̹��� Ȱ��ȭ
            currentImage.color = Color.white;
            newImage.SetActive(true);
            return currentImage;
        }

        /// <summary>
        /// �Է��ʵ� �߰�
        /// </summary>
        /// <param name="key">���۷����� ���� Ű</param>
        /// <param name="isEnableWhenStart">���̾�α׹ڽ��� Ȱ��ȭ�ɶ� ���� Ȱ��ȭ�Ǵ°�?</param>
        /// <param name="widthPercentile">������ �������� ���� ������ ���� (0 ~ 1f)</param>
        /// <param name="height">����</param>
        /// <param name="targetTransform">� Ʈ�������� �ڽ����� ��ġ�Ұ��ΰ�? (null, ����������)</param>
        /// <param name="contentType">�Է��ʵ��� ������ Ÿ��</param>
        /// <param name="alignOption">�Է��ʵ� ��ġ ���� �ɼ�</param>
        /// <returns></returns>
        public TMP_InputField AddInputField(string key, bool isEnableWhenStart, float widthPercentile, int height, Transform targetTransform = null, TMP_InputField.ContentType contentType = TMP_InputField.ContentType.Standard, Align alignOption = Align.LEFT)
        {
            GameObject newInputField = Instantiate(mInputFieldPrefab, Vector3.zero, Quaternion.identity, targetTransform == null ? mCenterContentsParent : targetTransform);

            //������Ʈ ȹ��
            RectTransform currentTransform = newInputField.GetComponent<RectTransform>();
            TMP_InputField currentInputField = newInputField.GetComponent<TMP_InputField>();

            //������ ����
            widthPercentile = Mathf.Clamp(widthPercentile, 0f, 1f);
            currentTransform.sizeDelta = new Vector2Int((int)((mDialogBoxRootSize.x - BORDER_GAP) * widthPercentile), height);

            //�Է��ʵ� ��ġ ����
            switch (alignOption)
            {
                case Align.LEFT:
                    {
                        currentTransform.pivot = new Vector2(0, 0.5f);
                        currentTransform.localPosition = Vector3.zero;
                        break;
                    }
                case Align.RIGHT:
                    {
                        GameObject newBorder = AddBorder(null, true, height, true);
                        currentTransform.SetParent(newBorder.transform);

                        currentTransform.pivot = new Vector2(1, 0.5f);
                        currentTransform.localPosition = new Vector2(mDialogBoxRootSize.x - BORDER_GAP, 0f);
                        break;
                    }
                case Align.CENTER:
                    {
                        GameObject newBorder = AddBorder(null, true, height, true);
                        currentTransform.SetParent(newBorder.transform);

                        currentTransform.pivot = new Vector2(0.5f, 0.5f);
                        currentTransform.localPosition = new Vector2((mDialogBoxRootSize.x - BORDER_GAP) * 0.5f, 0f);
                        break;
                    }
                case Align.EXPAND:
                    {
                        currentTransform.pivot = new Vector2(0.5f, 0.5f);
                        currentTransform.localPosition = new Vector2((mDialogBoxRootSize.x - BORDER_GAP) * 0.5f, 0f);
                        currentTransform.sizeDelta = new Vector2Int(mDialogBoxRootSize.x - BORDER_GAP, height);
                        break;
                    }
            }

            //�Է��ʵ� �ɼ� ����
            currentInputField.contentType = contentType;

            //��ųʸ��� ����
            AddInstantiatedObject(key, newInputField);

            //�Է��ʵ� Ȱ��ȭ
            newInputField.SetActive(isEnableWhenStart);
            return currentInputField;
        }

        /// <summary>
        /// ������ ������ ���η��̾ƿ��� �߰�
        /// </summary>
        /// <param name="key">���۷����� ���� Ű</param>
        /// <param name="isEnableWhenStart">���̾�α׹ڽ��� Ȱ��ȭ�ɶ� ���� Ȱ��ȭ�Ǵ°�?</param>
        /// <param name="height">����</param>
        /// <param name="spacing">���̾ƿ� ���ο��� ��ҵ� �� ����</param>
        /// <param name="isAutoAlign">�ڵ����� ũ�⸦ �����ϴ°�?</param>
        /// <returns></returns>
        public Transform AddHorizontalLayout(string key, bool isEnableWhenStart, int height, int spacing, bool isAutoAlign)
        {
            GameObject newHorizontalLayoutBorder = AddBorder(null, isEnableWhenStart, height, true);

            //������Ʈ ȹ��
            HorizontalLayoutGroup currentLayout = newHorizontalLayoutBorder.AddComponent<HorizontalLayoutGroup>();

            //���̾ƿ� ����
            currentLayout.childControlWidth = isAutoAlign; //���� ��ҵ��� �ڵ����� �����ϴ°�?
            currentLayout.spacing = spacing; //���� ����
            newHorizontalLayoutBorder.GetComponent<RectTransform>().sizeDelta = new Vector2Int(mDialogBoxRootSize.x - BORDER_GAP, height); //ũ�� ����

            //��ųʸ��� ����
            AddInstantiatedObject(key, newHorizontalLayoutBorder);

            return newHorizontalLayoutBorder.transform; //Ʈ�������� ����
        }

        /// <summary>
        /// ���������� �̸� �����ص� UI��Ҹ� ���̾�α׹ڽ��� �������� �߰�
        /// </summary>
        /// <param name="key">���۷����� ���� Ű</param>
        /// <param name="isEnableWhenStart">���̾�α׹ڽ��� Ȱ��ȭ�ɶ� ���� Ȱ��ȭ�Ǵ°�?</param>
        /// <param name="prefab">�ν��Ͻ� �� ������</param>
        /// <param name="isDeliverEvent">�����տ��� ȣ���ϴ� �̺�Ʈ�� �������ΰ�?</param>
        /// <returns></returns>
        public DialogBoxPrefabDeliver AddExistsPrefab(string key, bool isEnableWhenStart, DialogBoxPrefabDeliver prefab, bool isDeliverEvent)
        {
            //�̺�Ʈ ���ù� ����
            if (isDeliverEvent)
            {
                prefab.EventReciever = this;
            }

            //������Ʈ ȹ��
            RectTransform currentRectTransform = prefab.GetComponent<RectTransform>();

            //ũ�� ������ġ ���ϱ�
            float scaleCorrection = mDialogBoxRootSize.x / currentRectTransform.sizeDelta.x;

            //�������� ũ�⸦ ������� ���� ����
            GameObject newBorder = AddBorder(null, isEnableWhenStart, (int)(currentRectTransform.sizeDelta.y * scaleCorrection), true);

            //������ ��ġ �� ũ�� ���� 
            RectTransform newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity, newBorder.transform).GetComponent<RectTransform>();
            newPrefab.localScale = Vector3.one * scaleCorrection;
            newPrefab.localPosition = new Vector3((mDialogBoxRootSize.x * 0.5f) - BORDER_GAP * 0.5f, 0f);

            //������ Ȱ��ȭ
            prefab.gameObject.SetActive(true);

            //Ȱ��ȭ ���ο� ���� �θ������Ʈ�� ���� Ȱ��ȭ
            newBorder.SetActive(isEnableWhenStart);

            //��ųʸ��� ����
            AddInstantiatedObject(key, newPrefab.gameObject);

            return prefab;
        }
        #endregion

        #region ��ƿ��Ƽ

        /// <summary>
        /// �ڽ��� Ȱ��ȭ/��Ȱ��ȭ
        /// </summary>
        /// <param name="isEnable">Ȱ��ȭ �Ұ��ΰ�?</param>
        public void ToggleDialogBox(bool isEnable)
        {
            gameObject.SetActive(isEnable);
        }

        /// <summary>
        /// �ڽ��� �ı�
        /// </summary>
        public void DestroyBox()
        {
            foreach (KeyValuePair<string, DialogBoxController> caller in mDestroyCallEvents)
            {
                caller.Value.EventTrigger(caller.Key);
            }

            Destroy(gameObject);
        }

        /// <summary>
        /// ���̾�α׹ڽ����� ���۷����� UI��Ҹ� ȹ��
        /// </summary>
        /// <param name="key">���۷����� Ű</param>
        /// <typeparam name="T">� Ÿ���� ȹ���Ұ��ΰ�?</typeparam>
        /// <returns></returns>
        public T GetInstantiatedObject<T>(string key)
        {
            return mInstantiatedObjects[key].GetComponent<T>();
        }

        /// <summary>
        /// �Է��ʵ忡�� �߻��ϴ� �̺�Ʈ�� �߰�
        /// </summary>
        /// <param name="inputField">�̺�Ʈ�� ȣ���� �Է��ʵ�</param>
        /// <param name="eventID">ȣ���� �̺�Ʈ ID</param>
        /// <param name="eventType">�̺�Ʈ Ÿ��</param>
        public void AddInputFieldEvent(TMP_InputField inputField, string eventID, InputFieldEvent eventType)
        {
            //string argument = null;

            switch (eventType)
            {
                case InputFieldEvent.OnDeselect:
                    inputField.onDeselect.AddListener((argument) => EventTrigger(eventID));
                    break;
                case InputFieldEvent.OnEndEdit:
                    inputField.onEndEdit.AddListener((argument) => EventTrigger(eventID));
                    break;
                case InputFieldEvent.OnSelect:
                    inputField.onSelect.AddListener((argument) => EventTrigger(eventID));
                    break;
                case InputFieldEvent.OnValueChanged:
                    inputField.onValueChanged.AddListener((argument) => EventTrigger(eventID));
                    break;
            }
        }

        /// <summary>
        /// ���̾�α׹ڽ����� �ٸ� ���̾�α׹ڽ��� ���۷��� �� ��� ��ųʸ��� �߰�
        /// </summary>
        /// <param name="dialogBoxKey">�߰��� ���̾�α׹ڽ��� Ű</param>
        /// <param name="dialogBox">�߰��� ���̾�α׹ڽ�</param>
        public void AddReferenceDialogBox(string dialogBoxKey, DialogBoxController dialogBox)
        {
            mReferenceDialogBoxes.Add(dialogBoxKey, dialogBox);
        }

        /// <summary>
        /// ���̾�α׹ڽ����� �ٸ� ���̾�α׹ڽ��� ȹ��
        /// </summary>
        /// <param name="dialogBoxKey">������ ���̾�α׹ڽ��� Ű</param>
        /// <returns></returns>
        public DialogBoxController GetReferenceDialogBox(string dialogBoxKey)
        {
            return mReferenceDialogBoxes[dialogBoxKey];
        }

        /// <summary>
        /// �ı��� ȣ���� �̺�Ʈ �����ʸ� ��� 'RESERVED_EVENT_ON_DESTROY'�� ȣ��
        /// </summary>
        /// <param name="eventReciever">���� �̺�Ʈ�� ȣ���Ұ��ΰ�? (null: �ڱ��ڽ�)</param>
        public void AddDestroyListener(DialogBoxController eventReciever = null)
        {
            mDestroyCallEvents.Add(RESERVED_EVENT_ON_DESTROY, eventReciever == null ? this : eventReciever);
        }

        /// <summary>
        /// ��� ������ ���̸� ����
        /// </summary>
        /// <param name="height">������ ����</param>
        public void SetTopBoxHeight(int height)
        {
            //ũ�� ����
            mTopContentsParent.sizeDelta = new Vector2(0, height);

            //Height�� 0���� ũ��(�����ϴ°��) Ȱ��ȭ
            mTopContentsParent.gameObject.SetActive(height > 0);

            //���� ũ��� ������ 75%
            mTitleLabel.fontSizeMax = height * 0.75f;

            //�ڽ� ũ�� ����
            RefreshBoxSize();
        }

        /// <summary>
        /// ��� ������ �ؽ�Ʈ�� ����
        /// </summary>
        /// <param name="text">�ؽ�Ʈ</param>
        /// <param name="alignOption">���� �ɼ�</param>
        public void SetTitleBox(string text, TextAlignmentOptions alignOption = TextAlignmentOptions.Center)
        {
            TextMeshProUGUI titleLabel = mTitleLabel.GetComponent<TextMeshProUGUI>();

            titleLabel.text = text;
            titleLabel.alignment = alignOption;
        }

        /// <summary>
        /// �ϴ� ������ ���̸� ����
        /// </summary>
        /// <param name="height">������ ����</param>
        public void SetBottomBoxHeight(int height)
        {
            mBottomContentsParent.sizeDelta = new Vector2(0, height);

            mBottomContentsParent.gameObject.SetActive(height > 0);
            RefreshBoxSize();
        }
        #endregion

        #region ���� Ŭ����
        /// <summary>
        /// �������� ������ �ν��Ͻ��� ��ųʸ��� ����
        /// </summary>
        private void AddInstantiatedObject(string key, GameObject obj)
        {
            if (key == null) { return; }
            mInstantiatedObjects.Add(key, obj);
        }

        /// <summary>
        /// ������ �ڽ� ����� ����
        /// </summary>
        private void RefreshBoxSize()
        {
            mContentsScrollViewRoot.sizeDelta = new Vector2(0, mDialogBoxRootSize.y - mTopContentsParent.sizeDelta.y - mBottomContentsParent.sizeDelta.y);
            mContentsScrollViewRoot.anchorMax = new Vector2(1.0f, 0.5f);

            mContentsScrollViewRoot.localPosition = new Vector3(-mDialogBoxRootSize.x * 0.5f, (mBottomContentsParent.sizeDelta.y - mTopContentsParent.sizeDelta.y) * 0.5f, 0f);
        }
        #endregion

        #region UI �̺�Ʈ Ʈ����

        /// <summary>
        /// �������� ������ UI��ҵ鿡�� �̺�Ʈ�� ����
        /// </summary>
        /// <param name="eventID"></param>
        public void EventTrigger(string eventID)
        {
            switch (eventID) //ȣ���� �̺�Ʈ�� ����Ǿ��ִ� �̺�Ʈ�ΰ�?
            {
                case RESERVED_EVENT_CLOSE: //�ܼ��� �ݴ� ���̾�α׹ڽ��ΰ��
                    {
                        DestroyBox();

                        break;
                    }
            }

            mEventAction.Invoke(this, eventID); //�̺�Ʈ ȣ��
        }
        #endregion
    }  
}
