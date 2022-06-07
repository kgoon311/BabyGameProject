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
                                                         
    private void Start()
    {
        FadePanel = GameObject.Find("FadePanel");
        FadePanel.SetActive(false);
    }
    public IEnumerator FadeIn(float FadeTime)
    {
        FadePanel.SetActive(true);
        float timer = 0;
        while(timer < 1)
        {
            timer += Time.deltaTime / FadeTime;
            FadePanel.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Lerp(0, 1, timer));
            yield return null;
        }
        yield return null;
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
  
}
