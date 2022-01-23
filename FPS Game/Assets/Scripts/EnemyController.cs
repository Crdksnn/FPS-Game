using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    [Header("Target Settings")]
    private bool chasing;
    [SerializeField] float distanceToChase = 10f;
    [SerializeField] float distanceToLose = 15f;
    [SerializeField] float distanceToStop = 2f;
    private Vector3 targetPoint;

    NavMeshAgent agent;

    private Vector3 startPoint;
    [SerializeField] float keepChasingTime = 5f;
    private float chaseCounter;

    [Header("Bullet Settings")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;

    [SerializeField] float fireRate;
    [SerializeField] float waitBetweenShots = 2f;
    [SerializeField] float timeToShoot = 1f;
    private float fireCount;
    private float shotWaitCounter;
    private float shootTimeCounter;

    [SerializeField] Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPoint = transform.position;
        shootTimeCounter = timeToShoot;
        shotWaitCounter = waitBetweenShots;
    }

    
    void Update()
    {

        targetPoint = PlayerController.instance.transform.position;
        targetPoint.y = transform.position.y;

        if (!chasing)
        {
            if(Vector3.Distance(transform.position, targetPoint) < distanceToChase)
            {
                chasing = true;
                shootTimeCounter = timeToShoot;
                shotWaitCounter = waitBetweenShots;
            }

            if(chaseCounter > 0)
            {
                chaseCounter -= Time.deltaTime;

                if(chaseCounter <= 0)
                {
                    agent.destination = startPoint;
                }
            }

            if(agent.remainingDistance < 0.25f)
            {
                anim.SetBool("isMoving", false);
            }

            else
            {
                anim.SetBool("isMoving", true);
            }

        }
        else
        {
           if(Vector3.Distance(transform.position,targetPoint) > distanceToStop)
            {
                agent.destination = targetPoint;
            }

            else
            {
                agent.destination = transform.position;
            } 

            if (Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                chasing = false;
                chaseCounter = keepChasingTime;
            }

            if(shotWaitCounter > 0)
            {
                shotWaitCounter -= Time.deltaTime;

                if(shotWaitCounter <= 0)
                {
                    shootTimeCounter = timeToShoot;
                    
                }

                anim.SetBool("isMoving", true);
            }

            else
            {

                if (PlayerController.instance.gameObject.activeInHierarchy)
                {

                    shootTimeCounter -= Time.deltaTime;

                    if (shootTimeCounter > 0)
                    {
                        fireCount -= Time.deltaTime;

                        if (fireCount <= 0)
                        {
                            fireCount = fireRate;
                            //firePoint.LookAt(PlayerController.instance.transform.position + new Vector3(0f, 1.2f, 0f));
                            firePoint.LookAt(PlayerController.instance.transform.position);


                            //Check the angle player
                            Vector3 targetDirection = PlayerController.instance.transform.position - transform.position;
                            float angle = Vector3.SignedAngle(targetDirection, transform.forward, Vector3.up);

                            if (Mathf.Abs(angle) < 30f)
                            {
                                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                                anim.SetTrigger("fireShot");
                                agent.destination = transform.position;
                            }
                            else
                            {
                                shotWaitCounter = waitBetweenShots;
                                agent.destination = PlayerController.instance.transform.position;
                            }

                        }
                    }

                    else
                    {
                        shotWaitCounter = waitBetweenShots;
                    }
                }
                anim.SetBool("isMoving", false);
            }
        }

    }
}
