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
    public bool ScreenTouch;

    [Header("스테이지")]
    [SerializeField] private GameObject StagePanel;
    [SerializeField] private GameObject StageGroup;
    private int LastClearStage;

    [Header("스테이지")]
    [SerializeField] private Button SettingButton;
    void Start()
    {
        SoundManager.In.PlaySound("title", SoundType.BGM, 1, 1);
        SettingButton.onClick.AddListener(GMManger.In.PauseButton);
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
    }
    public void StageDown()
    {
        if (ScreenTouch == false)
        {
            SoundManager.In.PlaySound("Button", SoundType.SE, 1, 1);
            ScreenTouch = true;
            StagePanel.transform.DOLocalMove(Vector3.zero, 1f, false).SetEase(Ease.OutBounce);
        }
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
