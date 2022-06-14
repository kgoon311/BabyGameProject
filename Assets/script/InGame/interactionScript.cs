using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class interactionScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> list;
    
    private void Update()
    {
        if (list.Count != 0)
        {
            list.OrderBy(x => x.transform.position.x);
            gameObject.transform.parent.GetComponent<Drag>().Interaction(list[0]);
        }
    }
    private void LateUpdate()
    {
         gameObject.SetActive(false);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Animal"&&collision.gameObject != gameObject.transform.parent.gameObject)
        {
            list.Add(collision.gameObject);
        }
;
    }
}
