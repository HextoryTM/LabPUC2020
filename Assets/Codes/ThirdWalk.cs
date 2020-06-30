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
        Fly,
    }

    public States state;
    public Animator anim;
    public Rigidbody rb;

    //Jump
    public float jumpForce = 1000;
    float jumpTime = .5f;
    bool jumpPressed;

    //Movement
    public Vector3 move { get; private set; }
    public float moveForce = 10000f;
    public float dragForce = 400f;
    private Vector3 direction;

    private GameObject camRefObj;
    public int vidas { get; private set; }

    public GameObject weaponSlot;
    private Collider weaponColl;
    private GameManager gm;
    private bool notDead = true;

    //ComboControl
    private int comboNum = -1;
    private bool attacking = false;
    private bool combo1, combo2;

    //Skills
    public GameObject[] skillsPrefab;
    private int indexWeapon;
    private bool canSkill = true;

    //Timer
    private float timer;
    private float holdDur;

    //ik
    public bool ikActive = false;

    private void Awake()
    {
        vidas = 10;
        holdDur = 3f; //hold to reset level timer
    }
    void Start()
    {
        if (CommonStatus.lastPosition.magnitude > 1 && SceneManager.GetActiveScene().name == "MainScene")
        {
            transform.position = (CommonStatus.lastPosition - (Vector3.forward * 2));
            if (CommonStatus.Vida != -1)
                SetVida(CommonStatus.Vida);
        }
        

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
        transform.forward = Vector3.Lerp(transform.forward, direction, Time.fixedDeltaTime * 20);

        if (notDead)
        {
            rb.AddForce(move * (moveForce / (rb.velocity.magnitude + 1))); //reduz a força de movimento de acordo com a velocidade pra ter muita força de saida mas pouca velocidade. 

            Vector3 velocityDragWoY = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(-velocityDragWoY * dragForce);
        }

        
    }

    private void Update()
    {
        //Ground Distance
        if (Physics.Raycast(transform.position + Vector3.up * .5f, Vector3.down, out RaycastHit hit, 65279))
        {
            anim.SetFloat("GroundDistance", hit.distance);
        }

        if (notDead)
        {
            ;
            if (Input.GetButtonDown("Fire1"))
            {
                if (!combo2)
                {
                    if (combo1 && attacking)
                    {
                        comboNum = 2;
                        combo2 = true;
                        StartCoroutine(Attack());
                    }
                    else
                    {
                        comboNum = 1;
                        combo1 = true;
                        StartCoroutine(Attack());
                    }

                    comboNum = -1;
                }
            }

            if (Input.GetButtonDown("Jump"))
            {
                StartCoroutine(Jump());
            }
            if (Input.GetButtonUp("Jump"))
            {
                jumpTime = 0;
            }

            if (Input.GetButtonDown("Skill1") && canSkill)
            {
                canSkill = false;
                indexWeapon = 0;
                StartCoroutine(Skill());
            }
            if (Input.GetButtonDown("Skill2") && canSkill)
            {
                canSkill = false;
                indexWeapon = 1;
                StartCoroutine(Skill());
            }
            if (Input.GetButtonDown("Skill3") && canSkill)
            {
                canSkill = false;
                indexWeapon = 2;
                StartCoroutine(Skill());
            }

        }

        ResetTimer();
    }

    IEnumerator Skill()
    {
        anim.SetTrigger("Skill1");

        yield return new WaitForSeconds(0.5f);
        Vector3 tempPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        GameObject myprojectile = Instantiate(skillsPrefab[indexWeapon], tempPos + transform.forward, Quaternion.identity);
        myprojectile.GetComponent<Rigidbody>().AddForce(transform.forward * 15, ForceMode.Impulse);
        canSkill = true;
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
        jumpTime = 0.5f;

        //checa se esta no chao
        if (Physics.Raycast(transform.position + Vector3.up * .5f, Vector3.down, out RaycastHit hit, 65279))
        {
            if (hit.distance > 0.6)
            {
                StartCoroutine(Idle());
            }
            Debug.DrawLine(transform.position, hit.point);
        }

        while (state == States.Jump)
        {
            //equivalente ao Update
            rb.AddForce(Vector3.up * jumpForce * jumpTime); //adiciona forca eqt o tempo diminui
            jumpTime -= Time.fixedDeltaTime;

            if(jumpTime < 0)
            {
                StartCoroutine(Idle());
            }
            //
            yield return new WaitForFixedUpdate();
        }
        //saida do estado
    }

    IEnumerator Attack()
    {
        //equivalente ao Start
        bool onCombo = combo2; //no inicio do Attack
        int tempComboNum = comboNum; //no inicio do Attack
        attacking = true;

        weaponColl = FindWeaponColl();
        if (weaponColl != null)
            weaponColl.enabled = true;

        state = States.Attack;
        anim.SetTrigger("Attack");

        float tempTimer = 0.75f; //tempo combo2
        if (!onCombo)
            tempTimer = 0.6f; //tempo combo1

        yield return new WaitForSeconds(tempTimer); //tempo do ataque

        //saida do estado

        if (onCombo && tempComboNum == 2) //tem segundo combo e este é o segundo attack
        {
            combo2 = false;
            attacking = false;
            StartCoroutine(Idle());
            weaponColl = FindWeaponColl(); //Caso haja uma troca de armas na hora do ataque, essa verificacao impede de desligar o collider de uma arma que ja foi dropada.
            weaponColl.enabled = false;
            //fim attack2
        }
        else if (!onCombo && tempComboNum == 1) //nao tem segundo combo
        {
            combo1 = false;
            attacking = false;
            StartCoroutine(Idle());
            weaponColl = FindWeaponColl(); //Caso haja uma troca de armas na hora do ataque, essa verificacao impede de desligar o collider de uma arma que ja foi dropada.
            weaponColl.enabled = false;
        }
        else //tem segundo combo e esse é o primeiro attack
        {
            combo1 = false;
        }
    }

    IEnumerator Hitted()
    {
        //equivalente ao Start
        anim.SetTrigger("Hitted");
        state = States.Hitted;

        if (vidas > 0)
            vidas--;

        //Saida do estado
        if (vidas <= 0)
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(Die());
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(Idle());
        }
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

    IEnumerator Fly()
    {
        //equivalente ao Start
        state = States.Fly;


        //
        while (state == States.Fly)
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

    public void TakeDamage()
    {
        if (state != States.Die)
        {
            StartCoroutine(Hitted());
        }
    }

    public void SetVida(int valor)
    {
        vidas = valor;
    }

    void FinishAllCoroutines()
    {
        combo1 = false;
        combo2 = false;
        attacking = false;
        StopAllCoroutines();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (ikActive)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);

            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out RaycastHit hit, 3, 65279))
            {
                if (hit.collider.CompareTag("Push"))
                {
                    anim.SetIKPosition(AvatarIKGoal.LeftHand, hit.point - transform.right * 0.2f);
                    anim.SetIKPosition(AvatarIKGoal.RightHand, hit.point + transform.right * 0.2f);
                }
            }
        }
    }
}
