using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{
    [Header("타이틀")]
    [SerializeField] GameObject Title;
    [SerializeField] GameObject ClickText;
    public bool ScreenTouch;

    [Header("스테이지")]
    public int LastClearStage;
    [SerializeField] GameObject StagePanel;
    [SerializeField] GameObject StageGroup;
    void Start()
    {
        Title.transform.DOScale(Vector3.one, 1).SetEase(Ease.InQuint);
        for (int i = 0; i <= LastClearStage;i++)
        {
            StageGroup.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
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
    public void ClickShow(int stage)
    {
        SceneManager.LoadScene(stage);
    }
}
