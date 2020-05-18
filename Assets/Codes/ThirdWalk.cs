using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThirdWalk : MonoBehaviour
{
    public enum States
    {
        Idle,
        Walk,
        Jump,
        Die,
        Attack,
        Hitted,
    }

    public States state;
    public Animator anim;
    public Rigidbody rb;

    public Vector3 move { get; private set; }
    public float moveForce = 50f;
    public float speed = 20f;
    private Vector3 direction;
    private GameObject camRefObj;
    public int vidas { get; private set; }

    public GameObject weaponSlot;
    private Collider weaponColl;
    private GameManager gm;
    private bool notDead = true;

    //Skills
    public GameObject[] skillsPrefab;
    private int indexWeapon;

    //Timer
    private float timer;
    private float holdDur;

    private void Awake()
    {
        vidas = 5;
        holdDur = 3f; //hold to reset level timer
    }
    void Start()
    {
        weaponColl = FindWeaponColl();
        if (weaponColl != null)
            weaponColl.enabled = false;
        camRefObj = Camera.main.GetComponent<ThirdCam>().GetReferenceObject();
        StartCoroutine(Idle());
    }

    void FixedUpdate()
    {
        //criacao de vetor de movimento local
        if(notDead)
            move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = camRefObj.transform.TransformDirection(move);

        //girar pra direcao das teclas
        if (move.magnitude > 0)
        {
            direction = move;
        }
        transform.forward = Vector3.Lerp(transform.forward, direction, Time.fixedDeltaTime * 10);

        if (notDead)
        {
            //rb.AddForce(move * (moveForce / (rb.velocity.magnitude + 1))); //* speed);
            rb.AddForce(move * moveForce * speed);

            Vector3 velocityDragWoY = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(-rb.velocity * 200);
        }
    }

    private void Update()
    {
        if (notDead)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                StartCoroutine(Attack());
            }

            if (Input.GetButtonDown("Jump"))
            {
                StartCoroutine(Jump());
            }

            if (Input.GetButtonDown("Skill1"))
            {
                Vector3 tempPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                indexWeapon = 0;
                GameObject myprojectile = Instantiate(skillsPrefab[indexWeapon], tempPos + transform.forward, Quaternion.identity);
                myprojectile.GetComponent<Rigidbody>().AddForce(transform.forward * 10,ForceMode.Impulse);
            }
            if (Input.GetButtonDown("Skill2"))
            {
                Vector3 tempPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                indexWeapon = 1;
                GameObject myprojectile = Instantiate(skillsPrefab[indexWeapon], tempPos + transform.forward, Quaternion.identity);
                myprojectile.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
            }
            if (Input.GetButtonDown("Skill3"))
            {
                Vector3 tempPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                indexWeapon = 2;
                GameObject myprojectile = Instantiate(skillsPrefab[indexWeapon], tempPos + transform.forward, Quaternion.identity);
                myprojectile.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
            }

        }

        ResetTimer();
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

    IEnumerator Jump()
    {
        //equivalente ao Start
        state = States.Jump;
        rb.AddForce(Vector3.up * 1000, ForceMode.Impulse);

        //
        while (state == States.Jump)
        {
            //equivalente ao Update
            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 65279))
            {
                anim.SetFloat("GroundDistance", hit.distance);

                if(hit.distance < 0.2 && rb.velocity.y <= 0)
                {
                    StartCoroutine(Idle());
                }
                Debug.DrawLine(transform.position, hit.point);
            }
            else
            {
                anim.SetFloat("GroundDistance", 3);
            }

            //
            yield return new WaitForEndOfFrame();
        }
        //saida do estado
    }

    IEnumerator Attack()
    {
        //equivalente ao Start
        weaponColl = FindWeaponColl();
        if (weaponColl != null)
            weaponColl.enabled = true;

        state = States.Attack;
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f); //tempo do ataque

        //saida do estado
        weaponColl.enabled = false;
        StartCoroutine(Idle());
    }

    IEnumerator Hitted()
    {
        //equivalente ao Start
        anim.SetTrigger("Hitted");

        if(vidas > 0)
            vidas--;

        if(vidas <= 0)
        {
            yield return new WaitForEndOfFrame();
            state = States.Die;
            StartCoroutine(Die());
        }

        state = States.Hitted;

        yield return new WaitForEndOfFrame();

        //saida do estado
        StartCoroutine(Idle());
    }

    IEnumerator Die()
    {
        //equivalente ao Start
        anim.SetBool("Die", true);
        state = States.Die;
        notDead = false;

        Defeat();
        yield return new WaitForSeconds(2f);
    }

    private Collider FindWeaponColl()
    {
        if (weaponSlot)
        {
            if (weaponSlot.GetComponentInChildren<Collider>())
            {
                return weaponSlot.GetComponentInChildren<Collider>();
            }
            else
                return null;
        }
        else
            return null;
    }

    private void ResetTimer()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            timer = Time.time;
        }
        else if (Input.GetKey(KeyCode.R))
        {
            if (Time.time - timer > holdDur)
            {
                //by making it positive inf, we won't subsequently run this code by accident,
                //since X - +inf = -inf, which is always less than holdDur
                timer = float.PositiveInfinity;

                //perform your action
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            timer = float.PositiveInfinity;
        }
    }

    void Defeat()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.SetDefeat();

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }

    public void Damage()
    {
        if (state != States.Die)
        {
            StopAllCoroutines();
            StartCoroutine(Hitted());
        }
    }
}
