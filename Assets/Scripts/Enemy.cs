using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] float health;
    private float timeSinceAttack = 0;
    [SerializeField] float recoilLength;
    [SerializeField] float recoilFactor;
    [SerializeField] bool isRecoiling = false;

    float recoilTimer;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(health);
        if (health <= 0)
        {
            timeSinceAttack += Time.deltaTime;
            //if(timeSinceAttack > 0.3f) {
                Destroy(gameObject);
                Debug.Log("Dead");
                //timeSinceAttack = 0;
            //}
            
        }
        if (isRecoiling)
        {
            if (recoilTimer < recoilLength)
            {
                recoilTimer += Time.deltaTime;
            }
            else
            {
                isRecoiling = false;
                recoilTimer = 0;
            }
        }
    }

    public void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        //timeSinceAttack += Time.deltaTime;
        health -= _damageDone;
        Debug.Log("Damage");
        if (!isRecoiling)
        {
            rb.AddForce(-_hitForce * recoilFactor * _hitDirection);
            isRecoiling = true;
        }
    }
}
