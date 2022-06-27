using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    private float movecount;
    [HideInInspector] public Rigidbody2D rg;
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        StartCoroutine(MoveShadow());
    }
    private void Update()
    {
        if (InGameManager.Instence.StopTime == true)
            rg.velocity = Vector3.zero;
    }
    //-11,3.5
    private IEnumerator MoveShadow()
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
            while (timer < 1&&InGameManager.Instence.StopTime == false)
            {
                rg.velocity = new Vector3(movecount, 0, 0) * timer;
                timer += Time.deltaTime / 2;
                yield return null;
            }
        }
        yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        yield return StartCoroutine(MoveShadow());
        yield return null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            movecount = movecount * -1;
            if (movecount < 0)
                transform.rotation = new Quaternion(0, 180, 0, 0);
            else
                transform.rotation = new Quaternion(0, 0, 0, 0);
            rg.velocity = new Vector3(movecount, 0, 0);
        }
    }
}
