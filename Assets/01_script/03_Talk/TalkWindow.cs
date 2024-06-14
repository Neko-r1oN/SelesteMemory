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

    // -----------------------------------------------------------------
    // 会話の開始.
    // -----------------------------------------------------------------
    public async UniTask TalkStart(List<StoryData> talkList, float wordInterval = 0.05f)
    {
        foreach (var talk in talkList)
        {
            //nameText.text = talk.Name;
            nameText.text = data.GetCharacterName(talk.Name);
            talkText.text = "";
            goToNextPage = false;
            currentPageCompleted = false;
            isSkip = false;
            nextArrow.gameObject.SetActive(false);

            await UniTask.Delay((int)(0.5f * 1000f));

            foreach (char word in talk.Talk)
            {
                talkText.text += word;
                await UniTask.Delay((int)(wordInterval * 1000f));

                if (isSkip == true)
                {
                    talkText.text = talk.Talk;
                    break;
                }
            }

            currentPageCompleted = true;
            nextArrow.gameObject.SetActive(true);
            await UniTask.WaitUntil(() => goToNextPage == true);
        }
    }

    // -----------------------------------------------------------------
    // 次へボタンクリックコールバック.
    // -----------------------------------------------------------------
    public void OnNextButtonClicked()
    {
        if (currentPageCompleted == true) goToNextPage = true;
        else isSkip = true;
    }

   
}