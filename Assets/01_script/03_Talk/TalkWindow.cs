using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

// -----------------------------------------------------------------
// 会話ウインドウ.
// -----------------------------------------------------------------
public class TalkWindow : MonoBehaviour
{
    // -----------------------------------------------------------------
    // 会話パラメータ.
    // -----------------------------------------------------------------
    [System.Serializable]
    
    public class StoryData
    {
        // キャラ.
        public string Name = "";
        // 会話内容.
        [Multiline(3)] public string Talk = "";
        // 場所、背景.
        public string Place = "";
        // 左キャラ.
        public string Left = "";
        // 真ん中キャラ.
        public string Center = "";
        // 右キャラ.
        public string Right = "";
    }

    // キャラデータ.
    [SerializeField] CharacterData data = null;

    // -----------------------------------------------------------------
    // テキスト変数
    // -----------------------------------------------------------------
    // 名前のテキスト.
    [SerializeField] Text nameText = null;
    // 会話内容テキスト.
    [SerializeField] Text talkText = null;
    // 会話のトランジション.
    [SerializeField] UITransition talkWindowTransition = null;
    // 次ページへ表示画像.
    [SerializeField] Image nextArrow = null;
    // 会話パラメータリスト.
    [SerializeField] List<StoryData> talks = new List<StoryData>();

    // 次へフラグ.
    bool goToNextPage = false;
    // 次へ行けるフラグ.
    bool currentPageCompleted = false;
    // スキップフラグ.
    bool isSkip = false;


   
    async void Start()
    {
        // テスト用会話開始.(後で消します)
        await Open();
        await TalkStart(talks);
        await Close();
        Debug.Log("テスト終了");
    }

    // -----------------------------------------------------------------
    // ウインドウを開く.
    // -----------------------------------------------------------------
    public async UniTask Open(string initName = "", string initText = "")
    {
        nameText.text = initName;
        talkText.text = initText;
        nextArrow.gameObject.SetActive(false);
        talkWindowTransition.gameObject.SetActive(true);
        await talkWindowTransition.TransitionInWait();
    }

    // -----------------------------------------------------------------
    // ウインドウを閉じる.
    // -----------------------------------------------------------------
    public async UniTask Close()
    {
        await talkWindowTransition.TransitionOutWait();
        talkWindowTransition.gameObject.SetActive(false);
    }

    // 左キャライメージ.
    [SerializeField] Image leftCharacterImage = null;
    // 真ん中キャライメージ.
    [SerializeField] Image centerCharacterImage = null;
    // 右キャライメージ.
    [SerializeField] Image rightCharacterImage = null;
    // 左キャラトランジション.
    [SerializeField] UITransition leftCharacterImageTransition = null;
    // 真ん中キャラトランジション.
    [SerializeField] UITransition centerCharacterImageTransition = null;
    // 右キャラトランジション.
    [SerializeField] UITransition rightCharacterImageTransition = null;

    // 現在の左キャラ情報.
    string currentLeft = "";
    // 現在の真ん中キャラ情報.
    string currentCenter = "";
    // 現在の右キャラ情報.
    string currentRight = "";

    //  選択肢.
    [SerializeField] SelectButtonDialog selectButtonDialog = null;

    // -----------------------------------------------------------------
    // 会話の開始.
    // -----------------------------------------------------------------
    public async UniTask<List<int>> TalkStart(List<StoryData> talkList, float wordInterval = 0.05f)
    {
        currentLeft = "";
        currentCenter = "";
        currentRight = "";

        List<int> responseList = new List<int>();

        foreach (var talk in talkList)
        {
            // 選択肢の場合.
            if (talk.Name == "30")
            {
                goToNextPage = false;
                currentPageCompleted = false;
                isSkip = false;
                nextArrow.gameObject.SetActive(false);
                SetCharacter(talk).Forget();

                string[] arr = talk.Talk.Split(','); // 文字列を「,」で分割

                var res = await selectButtonDialog.CreateButtons(true, arr);

                Debug.Log("Response = " + res);
                responseList.Add(res);

                goToNextPage = true;
            }
            else
            {
                //nameText.text = talk.Name;
                nameText.text = data.GetCharacterName(talk.Name);
                talkText.text = "";
                goToNextPage = false;
                currentPageCompleted = false;
                isSkip = false;
                nextArrow.gameObject.SetActive(false);
                await SetCharacter(talk);

                // await UniTask.Delay((int)(0.5f * 1000f));

                foreach (char word in talk.Talk)
                {
                    talkText.text += word;
                    await UniTask.Delay((int)(wordInterval * 700f));

                    if (isSkip == true)
                    {
                        talkText.text = talk.Talk;
                        break;
                    }
                }
            }
            currentPageCompleted = true;
            nextArrow.gameObject.SetActive(true);
            await UniTask.WaitUntil(() => goToNextPage == true);
        }
        return responseList;
    }

    // -----------------------------------------------------------------
    // 次へボタンクリックコールバック.
    // -----------------------------------------------------------------
    public void OnNextButtonClicked()
    {
        if (currentPageCompleted == true) goToNextPage = true;
        else isSkip = true;
    }

    // -----------------------------------------------------------------
    // キャラ画像の設定.
    // -----------------------------------------------------------------
    async UniTask SetCharacter(StoryData storyData)
    {
        // Nullならすべて消す.
        if (storyData == null)
        {
            leftCharacterImage.gameObject.SetActive(false);
            centerCharacterImage.gameObject.SetActive(false);
            rightCharacterImage.gameObject.SetActive(false);
            return;
        }

        var tasks = new List<UniTask>();
        bool hideLeft = false;
        bool hideCenter = false;
        bool hideRight = false;

        // 左キャラ設定.
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
            Debug.Log("同じなので変化なし.");
        }

        // 真ん中キャラ設定.
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
            Debug.Log("同じなので変化なし.");
        }

        // 右キャラ設定.
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
            Debug.Log("同じなので変化なし.");
        }

        // 待機.
        await UniTask.WhenAll(tasks);

        // 消したいキャラを消す.
        if (hideLeft == true) leftCharacterImage.gameObject.SetActive(false);
        if (hideCenter == true) centerCharacterImage.gameObject.SetActive(false);
        if (hideRight == true) rightCharacterImage.gameObject.SetActive(false);
    }

}