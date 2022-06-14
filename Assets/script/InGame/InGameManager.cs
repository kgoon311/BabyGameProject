using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using DG.Tweening;
public class InGameManager : MonoBehaviour
{
    public static InGameManager Instence { get; set; }
    [Header("Canvas")]
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject NextPageButton;
    [SerializeField] private GameObject ClearBackGround;
    [SerializeField] private GameObject StarGroup;
    [SerializeField] private GameObject NextStageButton;
    [SerializeField] private GameObject ReGameButton;
    [SerializeField] private GameObject HomeButton;
    [SerializeField] private Text TimerText;
    [Header("Shape")]
    [SerializeField] private GameObject Shape;
    [SerializeField] private GameObject Shadow;
    //[SerializeField] List<RectTransform> ShapeGroup;
    [SerializeField] List<GameObject> ShapeGroup;

    private float cleartimer = 120;
    private float cleartime;
    private bool pushNextbutton;
    private bool[] clearbool = new bool[5];
    private int clearcount = 0;
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
        for (int i = 0; i < 5; i++)
        {
            Shape.transform.GetChild(i).GetComponent<Drag>().ShapeShadow = Shadow.transform.GetChild(i).gameObject;
        }
    }
    private void Start()
    {
        StartCoroutine(StartFadeOut());
        Invoke("GameOver", cleartimer);
    }
    private void Update()
    {
        if (clearcount != 5) { cleartimer -= Time.deltaTime; }
        if (cleartimer > 0) { TimerText.text = $"남은 시간  {(int)(cleartimer / 60)} : {(int)(cleartimer % 60)}"; }
        else { TimerText.text = $"남은 시간  0 : 0"; }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("z");
            StartCoroutine("C_NextPage");
        }
    }
    private void GameOver()
    {
        StartCoroutine("C_NextPage");
    }
    private IEnumerator StartFadeOut()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(GMManger.In.FadeOut(1f));
        yield return null;
    }
    public void NextPage()
    {
        if (pushNextbutton == false)
        {
            StartCoroutine("C_NextPage");
            pushNextbutton = true;
        }
    }
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
        yield return new WaitForSeconds(0.5f);
        Panel.SetActive(true);
        Panel.GetComponent<Image>().DOFade(0.5f, 1);
        yield return new WaitForSeconds(0.5f);
        ClearBackGround.transform.DOLocalMove(Vector3.zero, 1f, false).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(1f);
        StarGroup.transform.DOLocalMove(new Vector3(0, 300, 0), 1, false).SetEase(Ease.OutBack);
        NextStageButton.transform.DOLocalMove(new Vector3(650, -300, 0), 1, false).SetEase(Ease.OutBack);
        ReGameButton.transform.DOLocalMove(new Vector3(480, -300, 0), 1, false).SetEase(Ease.OutBack);
        HomeButton.transform.DOLocalMove(new Vector3(310, -300, 0), 1, false).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1f);
        for (int i = 0; /*i < savestar&&*/i <= clearcount / 2; i++)
        {
            StarGroup.transform.GetChild(i + 3).DOScale(Vector3.one, 0.6f);
            yield return new WaitForSeconds(0.6f);
        }
        yield return null;
    }
    public void Home()
    {
        SceneManager.LoadScene(0);
    }
    public void NextStage()
    {
        SceneManager.LoadScene(GMManger.In.stage += 1);
    }
    public void ReGame()
    {
        SceneManager.LoadScene(GMManger.In.stage);
    }
    public void OrderLayer()
    {
        int count = 0;
        ShapeGroup.Sort((GameObject x, GameObject y) => {
            return x.transform.localPosition.y.CompareTo(y.transform.localPosition.y);//y가 낮은순으로 정렬
        });
        ShapeGroup.Reverse();
        foreach (GameObject OrderLayerSetting in ShapeGroup)
        {
            OrderLayerSetting.GetComponent<SpriteRenderer>().sortingOrder = count++;
        }
    }
}
