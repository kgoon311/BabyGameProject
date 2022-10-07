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
    [SerializeField] private GameObject Credit;
    public bool ScreenTouch;

    [Header("스테이지")]
    [SerializeField] private GameObject StagePanel;
    [SerializeField] private GameObject StageGroup;
    [SerializeField] private int FinalStage;
    [SerializeField] private int LastClearStage;

    [Header("스테이지")]
    [SerializeField] private Button SettingButton;
    void Start()
    {
        SoundManager.In.PlaySound("title", SoundType.BGM, 1, 1);
        SettingButton.onClick.AddListener(GMManger.In.PauseButton);
        Title.transform.DOScale(Vector3.one * 0.7f, 1).SetEase(Ease.InQuint);
        LastClearStage = GMManger.In.LastClear;
        for (int i = 0; i <= LastClearStage && LastClearStage <= FinalStage;i++)
        {
            StageGroup.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                for (int j = 0;j<3;j++)
                {
                    if (GMManger.In.ClearStar[i, j] == true)
                        StageGroup.transform.GetChild(i).GetChild(1).GetChild(j).GetComponent<Image>().color = new Color(255,255,255);
                }
        }
    }
    public void StageDown()
    {
        if (ScreenTouch == false)
        {
            SoundManager.In.PlaySound("Button", SoundType.SE, 1, 1);
            ScreenTouch = true;
            StagePanel.transform.DOLocalMove(Vector3.down * 50, 1f, false).SetEase(Ease.OutBounce);//스테이지창 중앙으로
        }
    }
    public void Click(int stage)
    {
        if(stage <= LastClearStage+1)
            StartCoroutine(ClickShow(stage));
    }
    public void CreditOpen()
    {
        Credit.SetActive(true);
        SoundManager.In.PlaySound("Button", SoundType.SE, 1, 1);
    }
    public void CreditClose()
    {
        Credit.SetActive(false);
        SoundManager.In.PlaySound("Button", SoundType.SE, 1, 1);
    }
    public IEnumerator ClickShow(int stage)
    {
        StartCoroutine(GMManger.In.FadeIn(1));
        SoundManager.In.PlaySound("Button", SoundType.SE, 1, 1);
        yield return new WaitForSeconds(1f);
        GMManger.In.stage = stage;
        SceneManager.LoadScene(stage);
        yield return null;
    }
}
