using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{
    private bool ScreenTouch;
    [SerializeField] GameObject ClickText;
    void Start()
    {
        StartCoroutine(TextMove());
    }
    void Update()
    {
        
    }
    private IEnumerator TextMove()
    {
        ClickText.transform.DOLocalMove(new Vector3(0, -360, 0),2f,false);
        yield return new WaitForSeconds(2f);
        ClickText.transform.DOLocalMove(new Vector3(0, -320, 0),2f,false);
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(TextMove());
    }
    private void OnMouseDown()
    {
        if (ScreenTouch == false)
        {
            StartCoroutine(ClickShow());
        }
    }
    private IEnumerator ClickShow()
    {

        yield return null;
    }
}
