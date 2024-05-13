using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{

    Collider2D attackPoligon;
    // Start is called before the first frame update
    void Start()
    {
        attackPoligon = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            e.EnemyHit(1, (transform.position - e.transform.position).normalized, 100);
            return;
        }
    }
}
