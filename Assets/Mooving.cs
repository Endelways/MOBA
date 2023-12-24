using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mooving : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    [Header("Movement")] [SerializeField] ParticleSystem clickEffect;

    public float rotateSpeedMovement = 0.1f;
    public float rotateVelocity;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        RaycastHit hit;
        Anim();
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                agent.SetDestination(hit.point);

                StartCoroutine( DestroyGOm(Instantiate(clickEffect, hit.point += new Vector3(0, 0.1f, 0), clickEffect.transform.rotation).gameObject));
                Quaternion rotationLookAt = Quaternion.LookRotation(hit.point - transform.position);
                float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    rotationLookAt.eulerAngles.y,
                    ref rotateVelocity,
                    rotateSpeedMovement * (Time.deltaTime * 5));

                transform.eulerAngles = new Vector3(0, rotationY, 0);
            }
        } 
    }

    IEnumerator DestroyGOm(GameObject go)
    {
        yield return new WaitForSecondsRealtime(1);
        Destroy(go);
    }

    void Anim()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    animator.SetBool("isRun", false);
                    return;
                }
            }
        }
        animator.SetBool("isRun", true);
        
    }
}
