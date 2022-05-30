using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    [Header("Shape Object")]
    [SerializeField] private GameObject Shape;
    [SerializeField] private GameObject Shadow;

    private float cleartimer;
    private float cleartime;
    private bool pushNextbutton;
    private bool[] clearbool = new bool[5];
    private int clearcount = 0; 
    public int ClearCount
    {
        get{return clearcount;}
        set
        {
            clearcount++;
            clearbool[value] = true;
            if(clearcount == 5)
            {
                NextPageButton.transform.DOLocalMove(new Vector3(880, -286, 0), 1, false).SetEase(Ease.OutBounce);
                cleartime = cleartimer;
                //Clear();
            }
        } 
    }

        
    void Awake()
    {
        Instence = this;
        for(int i = 0;i<5;i++)
        {
            Shape.transform.GetChild(i).GetComponent<Drag>().ShapeShadow = Shadow.transform.GetChild(i).gameObject;
        }
    }
    private void Update()
    {
        cleartimer += Time.deltaTime;
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
        NextPageButton.transform.DOLocalMove(new Vector3(1045, -286, 0), 0.5f, false).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.5f);
        Panel.GetComponent<Image>().DOFade(0.5f, 1);
        yield return new WaitForSeconds(0.5f);
        ClearBackGround.transform.DOLocalMove(Vector3.zero,1f,false).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(1f);
        StarGroup.transform.DOLocalMove(new Vector3(0, 300, 0), 1, false).SetEase(Ease.OutBack);
        NextStageButton.transform.DOLocalMove(new Vector3(650, -300, 0), 1, false).SetEase(Ease.OutBack);
        ReGameButton.transform.DOLocalMove(new Vector3(480, -300, 0), 1, false).SetEase(Ease.OutBack);
        HomeButton.transform.DOLocalMove(new Vector3(310, -300, 0), 1, false).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1f);
        for(int i = 2; i >=  (int)(cleartime / 20); i--)
        {
            StarGroup.transform.GetChild(i + 3).DORotate(new Vector3(0, 0, 180), 0.3f);
            StarGroup.transform.GetChild(i+3).DORotate(new Vector3(0,0,360),0.3f); 
            StarGroup.transform.GetChild(i+3).DOScale(Vector3.one, 0.6f);
            yield return new WaitForSeconds(0.6f);
        }
        yield return null;
    }
}
