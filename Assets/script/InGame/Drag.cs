using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private bool draged;
    private bool overlap_Shape;//기본적으로 false이기 때문에 쓸모없는 코드이다
    private Vector3 firstPos;
    public GameObject ShapeShadow;
    private void OnMouseDrag()
    {
        if (draged == false)
        {
            draged = true;
            firstPos = transform.position;
        }
        else
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }
    }
    private void OnMouseUp()
    {
        draged = false;
        if (overlap_Shape == true)
        {
            Destroy(this);
            transform.position = ShapeShadow.transform.position;
            InGameManager.Instence.ClearCount = int.Parse(name);
        }
        else
        {
            transform.position = firstPos;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == ShapeShadow)
            overlap_Shape = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        overlap_Shape = false;
    }
}
