using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DialogBox;
using TMPro;
public class DialogBoxGenerator : Singleton<DialogBoxGenerator>
{
    [Header("���̾�α� �ڽ� �������� �ֻ��� �θ�")]
    [SerializeField] private GameObject mDialogBoxPrefab;

    /// <summary>
    /// ����ִ� ���̾�α׹ڽ��� ����
    /// </summary>
    /// <returns></returns>
    public DialogBoxController CreateEmptyDialogBox()
    {
        DialogBoxController controller = Instantiate(mDialogBoxPrefab, Vector3.zero, Quaternion.identity).GetComponent<DialogBoxController>();
        return controller;
    }

    /// <summary>
    /// �ܼ��� ���̾�α׹ڽ��� ����
    /// </summary>
    /// <param name="title">Ÿ��Ʋ (��� ����)</param>
    /// <param name="context">������ (������ �� ����)</param>
    /// <param name="buttonText">��ư (�ϴ� ��ư ����)</param>
    /// <param name="eventAction">�̺�Ʈ �븮��</param>
    /// <param name="buttonEventID">�⺻ �����Ǵ� ��ư�� ȣ���� �̺�Ʈ (null: �ܼ� ����)</param>
    /// <param name="width">���α���</param>
    /// <param name="height">���α���</param>
    /// <param name="titleHeight">��� ������ ����</param>
    /// <param name="buttonHeight">�ϴ� ������ ����</param>
    /// <returns></returns>
    public DialogBoxController CreateSimpleDialogBox(string title, string context, string buttonText, System.Action<DialogBoxController, string> eventAction = null, string buttonEventID = DialogBoxController.RESERVED_EVENT_CLOSE, int width = 200, int height = 150, int titleHeight = 30, int buttonHeight = 30)
    {
        //���̾�α׹ڽ� ����
        DialogBoxController controller = CreateEmptyDialogBox();

        //�̺�Ʈ ȣ�� ȯ�濡�� �븮�ڰ� ���°��?
        if (buttonEventID != DialogBoxController.RESERVED_EVENT_CLOSE && eventAction == null)
        {
            Debug.LogWarningFormat("{0}has no reciever!", gameObject.name);
        }

        //���̾�α׹ڽ� ũ�� ����
        controller.InitDialogBox(width, height, eventAction);

        //������ ����
        controller.SetTopBoxHeight(titleHeight);
        controller.SetBottomBoxHeight(buttonHeight);

        //Ÿ��Ʋ
        controller.SetTitleBox(title);

        //���ؽ�Ʈ �ؽ�Ʈ ����
        controller.AddText(null, true, context, 20, TextAlignmentOptions.Center);

        //������ ��ư ����
        controller.AddButton(null, true, buttonText, buttonEventID);

        //������ ��Ʈ�ѷ� ����
        return controller;
    }
    
}
