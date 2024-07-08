using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : ViewBase
{
    // �g�[�N�E�C���h�E.
    [SerializeField] TalkWindow talkWindow = null;

    void Start()
    {
    }

    // -------------------------------------------------------
    // �r���[�I�[�v�����R�[��.
    // -------------------------------------------------------
    public override async void OnViewOpened()
    {
        base.OnViewOpened();

        var data = talkWindow.Talks;
        try
        {
            await talkWindow.SetBg(data[0].Place, true);

            Debug.Log("��b�J�n");
            await talkWindow.Open();
            await talkWindow.TalkStart(data);
            await talkWindow.Close();
            Debug.Log("�e�X�g�I��");
        }
        catch (System.OperationCanceledException e)
        {
            Debug.Log("�e�X�g��b���L�����Z������܂����B" + e);
        }
    }

    // -------------------------------------------------------
    // �r���[�N���[�Y���R�[��.
    // -------------------------------------------------------
    public override void OnViewClosed()
    {
        base.OnViewClosed();
    }

    // -------------------------------------------------------
    // �z�[���ɖ߂�.
    // -------------------------------------------------------
    public void OnBackToHomeButtonClicked()
    {
        Scene.ChangeScene("01_Home").Forget();
    }
}