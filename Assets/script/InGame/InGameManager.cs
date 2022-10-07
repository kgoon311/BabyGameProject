using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class InGameManager : MonoBehaviour
{
    public static InGameManager Instence { get; set; }
    [Header("Canvas")]
    [SerializeField] private Text TimerText;
    [SerializeField] private GameObject NextPageButton;
    [SerializeField] GameObject ClearPage;
    [SerializeField] GameObject PausePanel;

    //0.BC / 1.Star / 2.Next / 3.Home/ 4.ReGame
    [Header("Chat")]
    [SerializeField] private GameObject[] ChatBalloon;
    [SerializeField] private List<GameObject> Chatsurve = new List<GameObject>();
    [SerializeField] private List<GameObject> ChatContents = new List<GameObject>();
    [SerializeField] private List<GameObject> ChatContentsGroup = new List<GameObject>();

    [Header("Shape")]
    [SerializeField] private GameObject Shape;
    [SerializeField] private GameObject Shadow;
    private List<GameObject> ShapeGroup = new List<GameObject>();
    private List<GameObject> ShadowGroup = new List<GameObject>();

    [SerializeField] float[] ChatPos = new float[2];
    [SerializeField] float[] ChatContPos = new float[2];

    private float cleartimer = 120;
    private float cleartime;
    private bool pushNextbutton;
    private bool[] clearbool = new bool[5];
    private int clearcount = 0;
    private bool Over;
    public int ClearCount
    {
        get { return clearcount; }
        set
        {
            clearcount++;
            clearbool[value] = true;
            if (clearcount == 5)
            {
                NextPageButton.transform.DOLocalMove(new Vector3(800, -130, 0), 0.5f, false).SetEase(Ease.OutBounce);
            }
        }
    }
    void Awake()
    {
        Instence = this;
        SoundManager.In.PlaySound($"{GMManger.In.stage}Stage",SoundType.BGM,1,1);
        for (int i = 0; i < 5; i++)
        {
            ShapeGroup.Add(Shape.transform.GetChild(i).gameObject);
            ShadowGroup.Add(Shadow.transform.GetChild(i).gameObject);
            ShapeGroup[i].GetComponent<Drag>().ShapeShadow = ShadowGroup[i];
        }
    }
    private void Start()
    {
       
        StartCoroutine(StartFadeOut());
    }
    private void Update()
    {
        if (clearcount != 5 && GMManger.In.StopTime == false) { cleartimer -= Time.deltaTime; }
        if (cleartimer > 0) { TimerText.text = $"남은 시간  {(int)(cleartimer / 60)} : {(int)(cleartimer % 60)}"; }
        else { TimerText.text = $"남은 시간  0 : 0"; StartCoroutine("C_NextPage"); GameOver(); }
    }
    public IEnumerator Interaction(GameObject myObject, GameObject targetObject)
    {
        float timer = 0;
        GMManger.In.StopTime = true;

        Vector3 firstposition1 = myObject.transform.position;
        Vector3 firstposition2 = targetObject.transform.position;
        float sidepos = (firstposition1.x - targetObject.transform.position.x > 0 ? 1f : -1f);
        myObject.transform.rotation = Quaternion.Euler(0, 90 + sidepos * 90, 0);
        targetObject.transform.rotation = Quaternion.Euler(0, 90 + sidepos * -90, 0);
        while (timer < 1)
        {
            myObject.transform.position = Vector3.Lerp(firstposition1, new Vector3(2*sidepos,0,0), timer);
            targetObject.transform.position = Vector3.Lerp(firstposition2,  new Vector3(2f * -sidepos, 0, 0), timer);
            myObject.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, timer);
            targetObject.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, timer);
            timer += Time.deltaTime * 2;
            yield return null;
        }
        int[] saveoderlayer = {myObject.GetComponent<SpriteRenderer>().sortingOrder , targetObject.GetComponent<SpriteRenderer>().sortingOrder};
        myObject.GetComponent<SpriteRenderer>().sortingOrder = 10;
        targetObject.GetComponent<SpriteRenderer>().sortingOrder = 10;
        switch (Random.Range(0,4))
        {
            case 0:
                #region 음식 생각
                int ChatCont = Random.Range(0, ChatContents.Count);
                GameObject ChatBox = Instantiate(ChatBalloon[1], myObject.transform.position + new Vector3(ChatPos[0] * sidepos, ChatPos[1], 0), myObject.transform.rotation);
                GameObject Chatting = Instantiate(ChatContents[ChatCont], myObject.transform.position + new Vector3(ChatContPos[0] * sidepos, ChatContPos[1], 0),myObject.transform.rotation);
                yield return new WaitForSeconds(2.5f);
                Destroy(ChatBox.gameObject);
                Destroy(Chatting.gameObject);
                yield return new WaitForSeconds(1f);
                GameObject fruit = Instantiate(ChatContents[ChatCont], myObject.transform.position + new Vector3(1.5f * -sidepos, 10f, 0), transform.rotation);
                GameObject fruit2 = Instantiate(ChatContents[ChatCont], myObject.transform.position + new Vector3(2.5f * -sidepos, 11f, 0), transform.rotation);
                fruit.transform.DOLocalMove((myObject.transform.position + new Vector3(1.5f * -sidepos, -1, 0)), 1f, false).SetEase(Ease.OutBounce);
                fruit2.transform.DOLocalMove((myObject.transform.position + new Vector3(2.5f * -sidepos, -1, 0)), 1.1f, false).SetEase(Ease.OutBounce);
                yield return new WaitForSeconds(1f);
                ChatBox = Instantiate(ChatBalloon[0], myObject.transform.position + new Vector3(ChatPos[0] * sidepos,ChatPos[1], 0), myObject.transform.rotation);
                Chatting = Instantiate(Chatsurve[2], myObject.transform.position + new Vector3(ChatContPos[0] * sidepos, ChatContPos[1], 0), transform.rotation);
                yield return new WaitForSeconds(1f);
                Destroy(ChatBox.gameObject);
                Destroy(Chatting.gameObject);
                yield return new WaitForSeconds(0.5f);
                ChatBox = Instantiate(ChatBalloon[0], myObject.transform.position + new Vector3(ChatPos[0] * sidepos, ChatPos[1], 0), myObject.transform.rotation);
                Chatting = Instantiate(ChatContentsGroup[0], myObject.transform.position + new Vector3(ChatContPos[0] * sidepos, ChatContPos[1], 0), myObject.transform.rotation);
                yield return new WaitForSeconds(1.5f);
                GameObject ChatBox2;
                GameObject Chatting2;
                if (Random.Range(0, 2) == 0)
                {
                    ChatBox2 = Instantiate(ChatBalloon[0], targetObject.transform.position+ new Vector3(ChatPos[0] * -sidepos, ChatPos[1], 0), targetObject.transform.rotation);
                    Chatting2 = Instantiate(Chatsurve[0], targetObject.transform.position + new Vector3(ChatContPos[0] * -sidepos, ChatContPos[1], 0), targetObject.transform.rotation);
                    yield return new WaitForSeconds(2f);
                    Destroy(ChatBox.gameObject);
                    Destroy(Chatting.gameObject);
                    Destroy(ChatBox2.gameObject);
                    Destroy(Chatting2.gameObject);
                    fruit.transform.DOLocalMove(myObject.transform.position, 1f, false).SetEase(Ease.OutCubic);
                    fruit2.transform.DOLocalMove(targetObject.transform.position, 1f, false).SetEase(Ease.OutCubic);
                    timer = 0;
                    while (timer < 1)
                    {
                        fruit.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, timer);
                        fruit2.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, timer);
                        timer += Time.deltaTime/2;
                        yield return null;
                    }
                    yield return new WaitForSeconds(0.5f);
                    Destroy(fruit);
                    Destroy(fruit2);
                }//수락
                else
                {
                    ChatBox2 = Instantiate(ChatBalloon[0], targetObject.transform.position + new Vector3(ChatPos[0] * -sidepos, ChatPos[1], 0), targetObject.transform.rotation);
                    Chatting2 = Instantiate(Chatsurve[1], targetObject.transform.position + new Vector3(ChatContPos[0] * -sidepos, ChatContPos[1], 0),targetObject.transform.rotation);
                    yield return new WaitForSeconds(1f);
                    GameObject Sad = Instantiate(Chatsurve[3], myObject.transform.position + Vector3.up * 0.5f + Vector3.left*0.5f* sidepos, transform.rotation);
                    yield return new WaitForSeconds(1f);
                    Destroy(ChatBox.gameObject);
                    Destroy(Chatting.gameObject);
                    Destroy(Sad.gameObject);
                    Destroy(ChatBox2.gameObject);
                    Destroy(Chatting2.gameObject);
                    timer = 0;
                    while (timer < 1)
                    {
                        fruit.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, timer * 2);
                        fruit2.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, timer * 2);
                        timer += Time.deltaTime * 2;
                        yield return null;
                    }
                    yield return new WaitForSeconds(0.5f);
                    Destroy(fruit);
                    Destroy(fruit2);
                }//거절
                #endregion
                break;
            case 1:
                #region 점프
                targetObject.transform.DOMove(targetObject.transform.position + Vector3.up, 0.5f);
                yield return new WaitForSeconds(0.5f);
                myObject.transform.DOMove(myObject.transform.position + Vector3.up, 0.5f);
                targetObject.transform.DOMove(targetObject.transform.position + Vector3.down, 0.5f).SetEase(Ease.OutBounce);
                yield return new WaitForSeconds(0.5f);
                targetObject.transform.DOMove(targetObject.transform.position + Vector3.up, 0.5f);
                myObject.transform.DOMove(myObject.transform.position + Vector3.down, 0.5f).SetEase(Ease.OutBounce);
                yield return new WaitForSeconds(0.5f);
                myObject.transform.DOMove(myObject.transform.position + Vector3.up, 0.5f);
                targetObject.transform.DOMove(targetObject.transform.position + Vector3.down, 0.5f).SetEase(Ease.OutBounce);
                yield return new WaitForSeconds(0.5f);
                targetObject.transform.DOMove(targetObject.transform.position + Vector3.up, 0.5f);
                myObject.transform.DOMove(myObject.transform.position + Vector3.down, 0.5f).SetEase(Ease.OutBounce);
                yield return new WaitForSeconds(0.5f);
                targetObject.transform.DOMove(targetObject.transform.position + Vector3.down, 0.5f).SetEase(Ease.OutBounce);
                yield return new WaitForSeconds(0.5f);
                yield return null;
                #endregion
                break;
            case 2:
                #region 춤1
                timer = 0;
                for (int i = 0; i < 7; i++)
                {
                    if (i < 2)
                    {
                        myObject.transform.DOScaleY(0.8f, 0.1f);
                        yield return new WaitForSeconds(0.1f);
                        myObject.transform.DOScaleY(1.2f, 0.2f);
                        yield return new WaitForSeconds(0.2f);
                        myObject.transform.DOScaleY(1f, 0.1f);
                        yield return new WaitForSeconds(0.1f);
                    }
                    else
                    {
                        myObject.transform.DOScaleY(0.8f, 0.1f);
                        targetObject.transform.DOScaleY(1.2f, 0.1f);
                        yield return new WaitForSeconds(0.1f);
                        myObject.transform.DOScaleY(1.2f, 0.2f);
                        targetObject.transform.DOScaleY(0.8f, 0.2f);
                        yield return new WaitForSeconds(0.2f);
                        myObject.transform.DOScaleY(1f, 0.1f);
                        targetObject.transform.DOScaleY(1f, 0.1f);
                        yield return new WaitForSeconds(0.1f);
                    }

                }
                #endregion
                break;
            case 3:
                #region 춤 2
                myObject.transform.DORotate(new Vector3(0, 0, 20), 0.25f);
                targetObject.transform.DORotate(new Vector3(0, 0, 20), 0.25f);
                yield return new WaitForSeconds(0.25f);
                for (int i = 0; i < 3; i++)
                {
                    myObject.transform.DORotate(new Vector3(0, 0, -20), 0.5f);
                    targetObject.transform.DORotate(new Vector3(0, 0, -20), 0.5f);
                    yield return new WaitForSeconds(0.5f);
                    myObject.transform.DORotate(new Vector3(0, 0, 20), 0.5f);
                    targetObject.transform.DORotate(new Vector3(0, 0, 20), 0.5f);
                    yield return new WaitForSeconds(0.5f);
                }
                myObject.transform.DORotate(new Vector3(0, 0, 0), 0.25f);
                targetObject.transform.DORotate(new Vector3(0, 0, 0), 0.25f);
                yield return new WaitForSeconds(0.25f);
                #endregion
                break;
        }//행동 랜덤 뽑기
        timer = 0;
        while (timer < 1)
        {
            myObject.transform.position = Vector3.Lerp(new Vector3(2f * sidepos, 0, 0), firstposition1,  timer);
            targetObject.transform.position = Vector3.Lerp(new Vector3(2f * -sidepos, 0, 0), firstposition2, timer);
            myObject.transform.localScale = Vector3.Lerp(Vector3.one*1.2f, Vector3.one*0.9f, timer);
            targetObject.transform.localScale = Vector3.Lerp(Vector3.one*1.2f, Vector3.one*0.9f, timer);
            timer += Time.deltaTime * 2;
            yield return null;
        }
        myObject.GetComponent<SpriteRenderer>().sortingOrder = saveoderlayer[0];
        targetObject.GetComponent<SpriteRenderer>().sortingOrder = saveoderlayer[1];
        GMManger.In.StopTime = false;
        yield return null;
    }//상호작용
    public void OrderLayer()
    {
        int count = 0;
        ShapeGroup.Sort((GameObject x, GameObject y) =>
        {
            return x.transform.localPosition.y.CompareTo(y.transform.localPosition.y);//y가 낮은순으로 정렬
        });
        ShapeGroup.Reverse();
        foreach (GameObject OrderLayerSetting in ShapeGroup)
        {
            OrderLayerSetting.GetComponent<SpriteRenderer>().sortingOrder = count++;
        }
    }//Y값에 따라 layer변경
    private IEnumerator StartFadeOut()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(GMManger.In.FadeOut(1f));
        yield return null;
    }
    #region End
    public void NextPage()
    {
        if (pushNextbutton == false)
        {
            SoundManager.In.PlaySound("Button", SoundType.SE, 1, 1);
            StartCoroutine("C_NextPage");
            pushNextbutton = true;
            GMManger.In.StopTime = false;
        }
    }//클리어 후 버튼
    private void GameOver()
    {
        if (Over != true)
            StartCoroutine("C_NextPage");
        Over = true;
    }//시간초과
    private IEnumerator C_NextPage()
    {
        /*     int savestar = (int)(60 / cleartime);*/
        if (GMManger.In.stage > GMManger.In.LastClear)
        {
            GMManger.In.LastClear = GMManger.In.stage;
        }
        for (int i = 0; /*i < savestar && i < 3*/ i <= clearcount / 2; i++)
        {
            GMManger.In.ClearStar[GMManger.In.stage - 1, i] = true;
        }
        ClearPage.transform.GetChild(4).GetComponent<Text>().text = $"{(int)(cleartimer / 60)} : {(int)(cleartimer % 60)}";
        yield return new WaitForSeconds(0.5f);
        GMManger.In.FadeIn(0.5f);       
        yield return new WaitForSeconds(0.5f);
        ClearPage.transform.DOLocalMove(Vector3.down*1050, 1);//클리어 페이지 중간으로 이동
        yield return new WaitForSeconds(1f);
        for (int i = 0; /*i < savestar&&*/i <= clearcount / 2; i++)
        {
            ClearPage.transform.GetChild(1).transform.GetChild(i + 3).DOScale(Vector3.one, 0.6f);
            yield return new WaitForSeconds(0.6f);
        }
        yield return null;
    }//클리어창 애니메이션
    #endregion
    #region Button
    public void Home()
    {
        SceneManager.LoadScene(0);
        SoundManager.In.PlaySound("Button", SoundType.SE, 1, 1);
        GMManger.In.StopTime = false;
    }
    public void ReGame()
    {
        SceneManager.LoadScene(GMManger.In.stage);
        SoundManager.In.PlaySound("Button", SoundType.SE, 1, 1);
        GMManger.In.StopTime = false;
    }
    public void PauseButton()
    {
        GMManger.In.StartCoroutine("Pause");
        SoundManager.In.PlaySound("Button", SoundType.SE, 1, 1);
    }
    public void NextGame()
    {
        SceneManager.LoadScene(GMManger.In.LastClear +1);
        SoundManager.In.PlaySound("Button", SoundType.SE, 1, 1);
        GMManger.In.stage++;
    }
    #endregion
}
