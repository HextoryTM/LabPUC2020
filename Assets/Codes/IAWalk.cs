using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAWalk : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject target;
    public Animator anim;
    public Vector3 patrolPos;
    public float stoppedTime;

    public GameObject Axe;
    private bool hitting;

    public enum IAState
    {
        Idle,
        Following,
        Attacking,
        Hitted,
        Dying,
        Patrol
    }

    public IAState currentState;

    // Start is called before the first frame update
    void Start()
    {
        patrolPos = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y, transform.position.z + Random.Range(-10, 10));
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case IAState.Idle:
                StateIdle();
                break;
            case IAState.Following:
                StateFollowing();
                break;
            case IAState.Attacking:
                StateAttacking();
                break;
            case IAState.Hitted:
                StateHitted();
                break;
            case IAState.Dying:
                StateDying();
                break;
            case IAState.Patrol:
                StatePatrol();
                break;
        }
        
        anim.SetFloat("Velocity", agent.velocity.magnitude);
    }

    void StateIdle()
    {
        anim.SetBool("Hitted", false);
        agent.isStopped = true;
        anim.SetBool("Attacking", false);

        if(Vector3.Distance(transform.position, target.transform.position) > 3)
        {
            currentState = IAState.Patrol;
        }

        if (Vector3.Distance(transform.position, target.transform.position) < 3)
        {
            currentState = IAState.Attacking;
        }
    }

    void StateFollowing()
    {
        agent.isStopped = false;
        anim.SetBool("Attacking", false);

        agent.SetDestination(target.transform.position);

        if (Vector3.Distance(transform.position, target.transform.position) < 3)
        {
            currentState = IAState.Attacking;
        }
    }

    void StateAttacking()
    {
        agent.isStopped = true;
        anim.SetBool("Attacking", true);

        if (Vector3.Distance(transform.position, target.transform.position) < 3)
        {
            if (!hitting)
            {
                Axe.GetComponent<PhisicalWeapon>().hitting = false;
                StartCoroutine(Attack());
            }
        }

        if (Vector3.Distance(transform.position, target.transform.position) > 5)
        {
            currentState = IAState.Following;
        }
    }

    void StateHitted()
    {
        agent.isStopped = true;
        anim.SetBool("Attacking", false);
        anim.SetBool("Hitted", true);

        currentState = IAState.Idle;
    }

    void StateDying()
    {
        //agent.isStopped = true;
        anim.SetBool("Attacking", false);
        anim.SetBool("Dying", true);

        GetComponent<Collider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        Axe.GetComponent<Collider>().enabled = false;
        GetComponent<DamageControl>().enabled = false;
        this.enabled = false;
    }

    void StatePatrol()
    {
        agent.isStopped = false;
        anim.SetBool("Attacking", false);

        agent.SetDestination(patrolPos);

        if (agent.velocity.magnitude < 0.1)
        {
            stoppedTime += Time.deltaTime;
        }
        if(stoppedTime > 3f)
        {
            stoppedTime = 0f;
            patrolPos = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y, transform.position.z + Random.Range(-10, 10));
        }

        if (Vector3.Distance(transform.position, target.transform.position) < 15)
        {
            currentState = IAState.Attacking;
        }
    }

    IEnumerator Attack()
    {
        //equivalente ao Start
        anim.SetTrigger("Hitting");
        hitting = true;

        yield return new WaitForSeconds(2f);

        //saida do estado
        hitting = false;
        currentState = IAState.Idle;
    }
}
