using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

// -----------------------------------------------------------------
// ��b�E�C���h�E.
// -----------------------------------------------------------------
public class TalkWindow : MonoBehaviour
{

    // ���O�̃e�L�X�g.
    [SerializeField] Text nameText = null;
    // ��b���e�e�L�X�g.
    [SerializeField] Text talkText = null;

    // ���y�[�W�֕\���摜.
    [SerializeField] Image nextArrow = null;
    // ��b�p�����[�^���X�g.
    //[SerializeField] List<StoryData> talks = new List<StoryData>();
    [SerializeField] public List<StoryData> Talks = new List<StoryData>();

    // ��b�̃g�����W�V����.
    [SerializeField] UITransition talkWindowTransition = null;

    // ���փt���O.
    bool goToNextPage = false;
    // ���֍s����t���O.
    bool currentPageCompleted = false;
    // �X�L�b�v�t���O.
    bool isSkip = false;

    // �L�����f�[�^.
    [SerializeField] CharacterData data = null;

    // �w�i�f�[�^.
    [SerializeField] BackgroundData bgData = null;

    // ���L�����C���[�W.
    [SerializeField] Image leftCharacterImage = null;
    // �^�񒆃L�����C���[�W.
    [SerializeField] Image centerCharacterImage = null;
    // �E�L�����C���[�W.
    [SerializeField] Image rightCharacterImage = null;

    // �w�i�C���[�W.
    [SerializeField] Image bgImage = null;
    // �w�i�g�����W�V����.
    [SerializeField] UITransition bgTransition = null;

    // ���L�����g�����W�V����.
    [SerializeField] UITransition leftCharacterImageTransition = null;
    // �^�񒆃L�����g�����W�V����.
    [SerializeField] UITransition centerCharacterImageTransition = null;
    // �E�L�����g�����W�V����.
    [SerializeField] UITransition rightCharacterImageTransition = null;

    // ���݂̍��L�������.
    string currentLeft = "";
    // ���݂̐^�񒆃L�������.
    string currentCenter = "";
    // ���݂̉E�L�������.
    string currentRight = "";

    //  �I����.
    [SerializeField] SelectButtonDialog selectButtonDialog = null;

    void Awake()
    {
        SetCharacter(null).Forget();
        talkWindowTransition.gameObject.SetActive(false);
    }

    async void Start()
    {
        // �e�X�g�p��b�J�n.(��ŏ����܂�)
        //await Open();
        //await TalkStart(talks);
        //await Close();
        //Debug.Log("�e�X�g�I��");
    }

    // -----------------------------------------------------------------
    // �E�C���h�E���J��.
    // -----------------------------------------------------------------
    public async UniTask Open(string initName = "", string initText = "")
    {
        SetCharacter(null).Forget();
        nameText.text = initName;
        talkText.text = initText;
        nextArrow.gameObject.SetActive(false);
        talkWindowTransition.gameObject.SetActive(true);
        await talkWindowTransition.TransitionInWait();
    }

    // -----------------------------------------------------------------
    // �E�C���h�E�����.
    // -----------------------------------------------------------------
    public async UniTask Close()
    {
        await talkWindowTransition.TransitionOutWait();
        talkWindowTransition.gameObject.SetActive(false);
    }

    // -----------------------------------------------------------------
    // ��b�̊J�n.
    // -----------------------------------------------------------------
    public async UniTask<List<int>> TalkStart(List<StoryData> talkList, float wordInterval = 0.03f)
    {
        try
        {
            currentLeft = "";
            currentCenter = "";
            currentRight = "";

            List<int> responseList = new List<int>();

            foreach (var talk in talkList)
            {
                if (string.IsNullOrEmpty(talk.Place) == false)
                {
                    await SetBg(talk.Place, false);
                }
                // �I�����̏ꍇ.
                if (talk.Name == "30")
                {
                    goToNextPage = false;
                    currentPageCompleted = false;
                    isSkip = false;
                    nextArrow.gameObject.SetActive(false);
                    SetCharacter(talk).Forget();

                    string[] arr = talk.Talk.Split(','); // ��������u,�v�ŕ���

                    var res = await selectButtonDialog.CreateButtons(true, arr);

                    Debug.Log("Response = " + res);
                    responseList.Add(res);

                    goToNextPage = true;
                }
                else
                {
                    nameText.text = data.GetCharacterName(talk.Name);
                    talkText.text = "";
                    goToNextPage = false;
                    currentPageCompleted = false;
                    isSkip = false;
                    nextArrow.gameObject.SetActive(false);
                    await SetCharacter(talk);

                    await UniTask.Delay((int)(0.5f * 1000f), cancellationToken: this.gameObject.GetCancellationTokenOnDestroy());

                    foreach (char word in talk.Talk)
                    {
                        talkText.text += word;
                        await UniTask.Delay((int)(wordInterval * 1000f), cancellationToken: this.gameObject.GetCancellationTokenOnDestroy());

                        if (isSkip == true)
                        {
                            talkText.text = talk.Talk;
                            break;
                        }
                    }
                }

                currentPageCompleted = true;
                nextArrow.gameObject.SetActive(true);
                await UniTask.WaitUntil(() => goToNextPage == true, cancellationToken: this.gameObject.GetCancellationTokenOnDestroy());
            }

            return responseList;
        }
        catch (System.OperationCanceledException e)
        {
            Debug.Log("TalkStart �L�����Z���B");
            throw e;
        }
    }

    // -----------------------------------------------------------------
    // ���փ{�^���N���b�N�R�[���o�b�N.
    // -----------------------------------------------------------------
    public void OnNextButtonClicked()
    {
        if (currentPageCompleted == true) goToNextPage = true;
        else isSkip = true;
    }

    // -----------------------------------------------------------------
    // �L�����摜�̐ݒ�.
    // -----------------------------------------------------------------
    async UniTask SetCharacter(StoryData storyData)
    {
        // Null�Ȃ炷�ׂď���.
        if (storyData == null)
        {
            leftCharacterImage.gameObject.SetActive(false);
            centerCharacterImage.gameObject.SetActive(false);
            rightCharacterImage.gameObject.SetActive(false);
            return;
        }

        try
        {
            var tasks = new List<UniTask>();
            bool hideLeft = false;
            bool hideCenter = false;
            bool hideRight = false;

            // ���L�����ݒ�.
            if (string.IsNullOrEmpty(storyData.Left) == true)
            {
                tasks.Add(leftCharacterImageTransition.TransitionOutWait());
                hideLeft = true;
            }
            else if (currentLeft != storyData.Left)
            {
                var img = data.GetCharacterSprite(storyData.Left);
                leftCharacterImage.sprite = img;
                leftCharacterImage.gameObject.SetActive(true);
                tasks.Add(leftCharacterImageTransition.TransitionInWait());

                currentLeft = storyData.Left;
            }
            else
            {
                Debug.Log("�����Ȃ̂ŕω��Ȃ�.");
            }

            // �^�񒆃L�����ݒ�.
            if (string.IsNullOrEmpty(storyData.Center) == true)
            {
                tasks.Add(centerCharacterImageTransition.TransitionOutWait());
                hideCenter = true;
            }
            else if (currentCenter != storyData.Center)
            {
                var img = data.GetCharacterSprite(storyData.Center);
                centerCharacterImage.sprite = img;
                centerCharacterImage.gameObject.SetActive(true);
                tasks.Add(centerCharacterImageTransition.TransitionInWait());

                currentCenter = storyData.Center;
            }
            else
            {
                Debug.Log("�����Ȃ̂ŕω��Ȃ�.");
            }

            // �E�L�����ݒ�.
            if (string.IsNullOrEmpty(storyData.Right) == true)
            {
                tasks.Add(rightCharacterImageTransition.TransitionOutWait());
                hideRight = true;
            }
            else if (currentRight != storyData.Right)
            {
                var img = data.GetCharacterSprite(storyData.Right);
                rightCharacterImage.sprite = img;
                rightCharacterImage.gameObject.SetActive(true);
                tasks.Add(rightCharacterImageTransition.TransitionInWait());

                currentRight = storyData.Right;
            }
            else
            {
                Debug.Log("�����Ȃ̂ŕω��Ȃ�.");
            }

            // �ҋ@.
            await UniTask.WhenAll(tasks);

            // ���������L����������.
            if (hideLeft == true) leftCharacterImage.gameObject.SetActive(false);
            if (hideCenter == true) centerCharacterImage.gameObject.SetActive(false);
            if (hideRight == true) rightCharacterImage.gameObject.SetActive(false);

        }
        catch (System.OperationCanceledException e)
        {
            Debug.Log("SetCharactor �L�����Z���B");
            throw e;
        }
    }

    // -----------------------------------------------------------------
    // �w�i�̐ݒ�.
    // -----------------------------------------------------------------
    public async UniTask SetBg(string place, bool isImmediate)
    {
        var sp = bgData.GetSprite(place);
        bgTransition.gameObject.SetActive(true);

        var currentBg = bgImage.sprite.name;
        if (currentBg == sp.name)
        {
            Debug.Log("�����w�i�Ȃ̂ŕύX���X�L�b�v���܂��B");
            return;
        }

        if (isImmediate == false)
        {
            try
            {
                await bgTransition.TransitionOutWait();
                bgImage.sprite = sp;
                await bgTransition.TransitionInWait();
            }
            catch (System.OperationCanceledException e)
            {
                Debug.Log("SetBg �L�����Z���B");
                throw e;
            }
        }
        else
        {
            bgImage.sprite = sp;
        }
    }
}