using System.Collections;
using System.Linq;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private bool overlap_Shape;
    private bool finish;
    private Vector3 firstPos;
    [HideInInspector] public Rigidbody2D rg;
    private float movecount;
    [Header("모양")]
    public GameObject ShapeShadow;
    private RectTransform MyRectTransform;
    private void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        MyRectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (InGameManager.Instence.StopTime == true)
            rg.velocity = Vector3.zero;
    }
    private void OnMouseDown()
    {
        firstPos = transform.position;
    }
    private void OnMouseDrag()
    {
        if (InGameManager.Instence.StopTime == false)
        {
            InGameManager.Instence.OrderLayer();
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }
    }
    private void OnMouseUp()
    {
        if (InGameManager.Instence.StopTime == false)
        {
            InGameManager.Instence.OrderLayer();
            ExitScreen();
            if (overlap_Shape == true && finish == false)
            {
                finish = true;
                gameObject.layer.ToString("Animal");
                StartCoroutine(Move());
                transform.position = ShapeShadow.transform.position;
                ShapeShadow.gameObject.SetActive(false);
                InGameManager.Instence.ClearCount = int.Parse(name);
            }//클리어
            else if (finish == false)
            {
                transform.position = firstPos;
            }//실패
            else
            {
                LayerMask mask = LayerMask.GetMask("Animal");
                Collider2D[] overlap = Physics2D.OverlapBoxAll(transform.position, new Vector2(5, 2), 0, mask);
                if (overlap.Length > 1)
                {
                    GameObject[] order = (from x in overlap
                                          orderby Vector3.Distance(transform.position, x.transform.position) descending
                                          select x.gameObject).Reverse().ToArray();
                    StartCoroutine(InGameManager.Instence.Interaction(this.gameObject, order[1]));
                }
            }//클리어 후 상호작용 체크
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == ShapeShadow)
            overlap_Shape = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall" && InGameManager.Instence.StopTime == false)
        {
            movecount = movecount * -1;
            if (movecount < 0)
                transform.rotation = new Quaternion(0, 180, 0, 0);
            else
                transform.rotation = new Quaternion(0, 0, 0, 0);
            rg.velocity = new Vector3(movecount, 0, 0);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        overlap_Shape = false;
    }

    public void ExitScreen()
    {
        if (MyRectTransform.anchoredPosition.x > 16)
        {
            MyRectTransform.anchoredPosition = new Vector3(16, MyRectTransform.anchoredPosition.y, 0);
        }
        if (MyRectTransform.anchoredPosition.x < 3)
        {
            MyRectTransform.anchoredPosition = new Vector3(3, MyRectTransform.anchoredPosition.y, 0);
        }
        if (MyRectTransform.anchoredPosition.y > 3)
        {
            MyRectTransform.anchoredPosition = new Vector3(MyRectTransform.anchoredPosition.x, 3f, 0);
        }
        if (MyRectTransform.anchoredPosition.y < -1.5)
        {
            MyRectTransform.anchoredPosition = new Vector3(MyRectTransform.anchoredPosition.x, -1.5f, 0);
        }
    }//화면 밖으로 이동 시 안쪽으로
    private IEnumerator Move()
    {
        if (InGameManager.Instence.StopTime == false)
        {
            while (movecount == 0)
                movecount = Random.Range(-2, 3);
            if (movecount < 0)
                transform.rotation = new Quaternion(0, 180, 0, 0);
            else
                transform.rotation = new Quaternion(0, 0, 0, 0);
            float timer = 0;
            while (timer < 1 && InGameManager.Instence.StopTime == false)
            {
                rg.velocity = new Vector3(movecount, 0, 0) * timer;
                timer += Time.deltaTime / 2;
                yield return null;
            }
        }
        yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        yield return StartCoroutine(Move());
        yield return null;
    }
}
