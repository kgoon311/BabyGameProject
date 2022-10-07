using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class GMManger : SingletonMono<GMManger>
{
    private GameObject FadePanel;

    public int LastClear =0;
    public bool[,] ClearStar = new bool[4,3];
    public int stage;
    public bool StopTime;

    [SerializeField] GameObject PausePanel;
    protected override void OnAwake()
    {
        //TODO: 인스펙터창에서 받는게 좋을듯
        FadePanel = GameObject.Find("FadePanel");
        FadePanel.SetActive(false);
    }
    public IEnumerator FadeIn(float FadeTime)
    {
        FadePanel.SetActive(true);
        Image Fade = FadePanel.GetComponent<Image>();
        float timer = 0;
        while(timer < 1)
        {
            timer += Time.deltaTime / FadeTime;
            Fade.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, timer));
            yield return null;
        }
    }
    public IEnumerator FadeOut(float FadeTime)
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime / FadeTime;
            FadePanel.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Lerp(1, 0, timer));
            yield return null;
        }
        FadePanel.SetActive(false);
        yield return null;
    }
    #region Button
   
    public void PauseButton()
    {
        StartCoroutine("Pause");
        SoundManager.In.PlaySound("Button", SoundType.SE, 1, 1);
    }
    public IEnumerator Pause()
    {
        FadeIn(0.5f);
        PausePanel.transform.DOLocalMove(Vector3.zero, 1).SetEase(Ease.OutBack);
        yield return null;
    }
    public void CountinueButton()
    {
        StartCoroutine("Countinue");
        SoundManager.In.PlaySound("Button", SoundType.SE, 1, 1);
    }
    public IEnumerator Countinue()
    {
        float timer = 0;
        PausePanel.transform.DOLocalMove(Vector3.up * 1025, 1).SetEase(Ease.OutBack);//정지창 위로 올려 안보이게
        while (timer < 1)
        {
            timer += Time.deltaTime * 2;
            FadePanel.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Lerp(0, 0f, timer));
            yield return null;
        }
        FadePanel.SetActive(false);
        StopTime = false;
        yield return null;
    }
    #endregion
}
