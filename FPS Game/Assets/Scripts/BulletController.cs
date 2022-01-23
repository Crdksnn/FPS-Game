using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float lifeTime;
    Rigidbody rb;

    [Header("Impact Effect Settings")]
    [SerializeField] GameObject impactEffect;

    [Header("Damage Settings")]
    [SerializeField] int damage;
    [SerializeField] bool damageEnemy;
    [SerializeField] bool damagePlayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.velocity = transform.forward * moveSpeed;

        lifeTime -= Time.deltaTime;

        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
        var _impactEffect = Instantiate(impactEffect, transform.position , transform.rotation);
        Destroy(_impactEffect, 2f);
        if(other.gameObject.tag == "Enemy" && damageEnemy)
        {
            other.gameObject.GetComponent<TargetHealthController>().TakeDamage(damage);
        }

        if(other.gameObject.tag == "HeadShot" && damageEnemy)
        {
            other.transform.parent.GetComponent<TargetHealthController>().TakeDamage(damage * 2);
        }

        if(other.gameObject.tag == "Player" && damagePlayer)
        {
            PlayerHealthController.instance.TakeDamage(damage);
        }

    }

}
