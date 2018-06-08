using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GoogleARCore;

public class BlueController2
: MonoBehaviour {

    [HideInInspector] public Animator m_Anim;
    [HideInInspector] public Rigidbody m_Rigid;
    public Transform m_Red;
    public float moveSpeed = 0.7f;
    public float rotateSpeed = 0.7f;
    private Vector3 moveDirection;
    private Vector3 destination;
    private float speed;

    //Combat UI
    public Button b_AttackL, b_AttackR, b_Defend;


    private void Awake() {

        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();
        m_Red = GameObject.FindGameObjectWithTag("Red").transform;


    }

    private void Start() {

        b_AttackL = GetComponentInChildren<Button>();
        b_AttackR = GetComponentInChildren<Button>();
        b_Defend = GetComponentInChildren<Button>();             
        b_AttackL.onClick.AddListener(() => B_Attack_1L());
        b_AttackL.onClick.AddListener(() => B_Attack_1R());
        b_Defend.onClick.AddListener(() => B_Defend_1());

        CurrentState = BLUE_STATE.IDLE;

    }

    // Rigidbody velocity update
    private void FixedUpdate() {

        speed = m_Rigid.velocity.magnitude;
        //Debug.Log(speed);

    }

    // Standard updates
    private void Update() {

        if(b_AttackL){

                Debug.Log("Attack L button found");

        }

        if (speed < 0.05f && CurrentState == BLUE_STATE.MOVE) {

            CurrentState = BLUE_STATE.IDLE;

        }

        //touch/mouse input to move player
        if (Input.GetMouseButton(0)) {

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10.0f) && hit.transform.tag == "Ring") {

                destination = hit.point;

                //Move
                this.transform.position = Vector3.MoveTowards(this.transform.position, hit.point, moveSpeed * Time.deltaTime);
         
                //Rotation
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(moveDirection), rotateSpeed);
                moveDirection = destination - this.transform.position;
                moveDirection.y = 0.0f;

                //Switch
                CurrentState = BLUE_STATE.MOVE;

            }

        }

    }

    public enum BLUE_STATE { IDLE, MOVE, ATTACKL, ATTACKR, DEFEND };
    [SerializeField] BLUE_STATE currentState = BLUE_STATE.IDLE;
    public BLUE_STATE CurrentState {

        get { return currentState; }
        set {

            currentState = value;
            StopAllCoroutines();

            switch (currentState) {

                case BLUE_STATE.IDLE:
                    StartCoroutine(BlueIdle());
                    break;

                case BLUE_STATE.MOVE:
                    StartCoroutine(BlueMove());
                    break;

                case BLUE_STATE.ATTACKL:
                    StartCoroutine(BlueAttackL());
                    break;

                case BLUE_STATE.ATTACKR:
                    StartCoroutine(BlueAttackR());
                    break;

                case BLUE_STATE.DEFEND:
                    StartCoroutine(BlueDefend());
                    break;

            }

        }

    }

    public IEnumerator BlueIdle() {

        while (true) {

            m_Anim.SetBool("Idle", true);
            m_Anim.SetBool("Move", false);
            m_Anim.SetBool("AttackL", false);
            m_Anim.SetBool("AttackR", false);
            m_Anim.SetBool("Defend", false);
            //Rotation
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(moveDirection), rotateSpeed);
            moveDirection = m_Red.position - this.transform.position;
            moveDirection.y = 0.0f;

            yield return null;

        }

    }

    public IEnumerator BlueMove() {

        while (true) {

            m_Anim.SetBool("Idle", false);
            m_Anim.SetBool("Move", true);
            m_Anim.SetBool("AttackL", false);
            m_Anim.SetBool("AttackR", false);
            m_Anim.SetBool("Defend", false);

            yield return null;

         }

    }

    public IEnumerator BlueAttackL() {

        while (true) {

            m_Anim.SetBool("Idle", false);
            m_Anim.SetBool("Move", false);
            m_Anim.SetBool("AttackL", false);
            m_Anim.SetBool("AttackR", true);
            m_Anim.SetBool("Defend", false);

            yield return new WaitForSeconds(0.5f);

            CurrentState = BLUE_STATE.IDLE;

            yield break;

        }
     
    }

    public IEnumerator BlueAttackR() {

        while (true) {
            
            m_Anim.SetBool("Idle", false);
            m_Anim.SetBool("Move", false);
            m_Anim.SetBool("AttackL", true);
            m_Anim.SetBool("AttackR", false);
            m_Anim.SetBool("Defend", false);

            yield return new WaitForSeconds(0.5f);

            CurrentState = BLUE_STATE.IDLE;

            yield break;

         }
    }

    public IEnumerator BlueDefend() {

        while (true) {
            //Debug.Log("Dum");
            m_Anim.SetBool("Idle", false);
            m_Anim.SetBool("Move", false);
            m_Anim.SetBool("AttackL", false);
            m_Anim.SetBool("AttackR", false);
            m_Anim.SetBool("Defend", true);

            yield return new WaitForSeconds(0.5f);

            CurrentState = BLUE_STATE.IDLE;

            yield break;

        }
    }

    //UI "IsPointerOver" detection

    private bool IsPointerOverUIObject() {

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;

    }

    //Public methods for button calls

    //Attack
    public void B_Attack_1L() {
        //Debug.Log("WEIRD");
        CurrentState = BLUE_STATE.ATTACKL;
        //return;

    }

    public void B_Attack_1R() {
        //Debug.Log("Button Attack R");
        CurrentState = BLUE_STATE.ATTACKR;
        //return;

    }

    //Defend
    public void B_Defend_1() {
        //Debug.Log("Button Defend");
        CurrentState = BLUE_STATE.DEFEND;
        //return;

    }
}