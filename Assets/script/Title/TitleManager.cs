using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{
    [Header("타이틀")]
    [SerializeField] private GameObject Title;
    [SerializeField] private GameObject ClickText;
    public bool ScreenTouch;

    [Header("스테이지")]
    private int LastClearStage;
    [SerializeField] private GameObject StagePanel;
    [SerializeField] private GameObject StageGroup;
    void Start()
    {
        Title.transform.DOScale(Vector3.one, 1).SetEase(Ease.InQuint);
        LastClearStage = GMManger.In.LastClear;
        for (int i = 0; i <= LastClearStage;i++)
        {
            StageGroup.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                for (int j = 0;j<3;j++)
                {
                    if (GMManger.In.ClearStar[i, j] == true)
                        StageGroup.transform.GetChild(i).GetChild(1).GetChild(j).GetComponent<Image>().color = new Color(255,255,255);
                }
        }
        StartCoroutine(TextMove());
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (ScreenTouch == false)
            {
                ScreenTouch = true;
                StagePanel.transform.DOLocalMove(Vector3.zero, 1f, false).SetEase(Ease.OutBounce);
            }
        }
    }
    
    private IEnumerator TextMove()
    {
        ClickText.transform.DOLocalMove(new Vector3(0, -360, 0),2f,false);
        yield return new WaitForSeconds(2f);
        ClickText.transform.DOLocalMove(new Vector3(0, -320, 0),2f,false);
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(TextMove());
    }
    public void Click(int stage)
    {
        if(stage <= LastClearStage+1)
            StartCoroutine(ClickShow(stage));
    }
    public IEnumerator ClickShow(int stage)
    {
        StartCoroutine(GMManger.In.FadeIn(1));
        yield return new WaitForSeconds(1f);
        GMManger.In.stage = stage;
        SceneManager.LoadScene(stage);
        yield return null;
    }
}
