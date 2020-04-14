using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdWalk : MonoBehaviour
{
    public enum States
    {
        Idle,
        Walk,
        Jump,
        Die
    }

    public States state;
    public Animator anim;
    public Rigidbody rb;

    public Vector3 move { get; private set; }
    public float moveForce = 50f;
    public float speed = 20f;
    private Vector3 direction;
    private GameObject camRefObj;

    void Start()
    {
        camRefObj = Camera.main.GetComponent<ThirdCam>().GetReferenceObject();
        StartCoroutine(Idle());
    }

    void FixedUpdate()
    {
        //criacao de vetor de movimento local
        move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = camRefObj.transform.TransformDirection(move);

        //girar pra direcao das teclas
        if (move.magnitude > 0)
        {
            direction = move;
        }
        transform.forward = Vector3.Lerp(transform.forward, direction, Time.fixedDeltaTime * 10);

        //rb.AddForce(move * (moveForce / (rb.velocity.magnitude + 1))); //* speed);
        rb.AddForce(move * moveForce * speed);
    }

    IEnumerator Idle()
    {
        //equivalente ao Start
        state = States.Idle;


        //
        while (state == States.Idle)
        {
            //equivalente ao Update
            anim.SetFloat("Velocity", rb.velocity.magnitude);

            if(rb.velocity.magnitude > 0.1)
            {
                StartCoroutine(Walk());
            }

            //
            yield return new WaitForEndOfFrame();
        }
        //saida do estado
    }

    IEnumerator Walk()
    {
        //equivalente ao Start
        state = States.Walk;


        //
        while (state == States.Walk)
        {
            //equivalente ao Update
            anim.SetFloat("Velocity", rb.velocity.magnitude);

            if (rb.velocity.magnitude < 0.1)
            {
                StartCoroutine(Idle());
            }


            //
            yield return new WaitForEndOfFrame();
        }
        //saida do estado
    }
}
