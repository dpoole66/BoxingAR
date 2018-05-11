using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GoogleARCore;

public class BlueController : MonoBehaviour {

    [HideInInspector] public Animator m_Anim;
    [HideInInspector] public Rigidbody m_Rigid;
    public Transform m_Red;
    private bool move = false;
    private Transform m_PlayerTrans;
    private Vector3 destinationPos;
    private float destinationDis;
    private bool inRange = false;
    public float setMovementRange = 0.001f;
    public float moveSpeed = 1.0f;
    public float rotSpeed = 0.5f;
    private float m_Speed;
    public GameObject Stage;

    //Combat
    public float enGuardRange = 0.2f;
    public float attackRange = 0.1f;
    public Button b_AttackL, b_AttackR, b_Defend;

    private void Awake() {

        Physics.gravity = new Vector3(0, -200f, 0);
        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();
        Stage = GameObject.FindGameObjectWithTag("Stage");
        m_PlayerTrans = transform;
        destinationPos = m_PlayerTrans.position;
        m_Red = GameObject.FindGameObjectWithTag("Enemy").transform;


    }

    void Start() {

        b_AttackL = GetComponentInChildren<Button>();
        b_AttackR = GetComponentInChildren<Button>();
        b_Defend = GetComponentInChildren<Button>();

        b_AttackL.onClick.AddListener(() => Player_AttackL());
        b_AttackL.onClick.AddListener(() => Player_AttackR());
        b_Defend.onClick.AddListener(() => Player_Defend());

        CurrentState = PLAYER_STATE.IDLE;

    }

    // Update is called once per frame
    private void Update() {
        if (m_Red != null) {

            Debug.Log("Found Red");

        }

        //Combat
        var combatRange = Vector3.Distance(m_PlayerTrans.position, m_Red.position);

        //touch/mouse input to move player
        if (Input.GetMouseButton(0) && !inRange && !IsPointerOverUIObject()) {

            //Touch awareness
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {
                //return;
            }

            CurrentState = PLAYER_STATE.MOVE;
        }
    }

    //-------------Player finite state machine

    public enum PLAYER_STATE { IDLE, MOVE, ATTACKL, ATTACKR, DEFEND, INJURED, DEAD };

    [SerializeField]
    private PLAYER_STATE currentState = PLAYER_STATE.IDLE;

    // get private currentState from public encapsulation and return corresponding state
    public PLAYER_STATE CurrentState {

        get { return currentState; }
        set {
            currentState = value;

            StopAllCoroutines();

            switch (currentState) {
                case PLAYER_STATE.IDLE:
                    StartCoroutine(Player_Idle());
                    break;

                case PLAYER_STATE.MOVE:
                    StartCoroutine(Player_Move());
                    break;

                case PLAYER_STATE.ATTACKL:
                    StartCoroutine(Player_AttackL());
                    break;

                case PLAYER_STATE.ATTACKR:
                    StartCoroutine(Player_AttackR());
                    break;

                case PLAYER_STATE.DEFEND:
                    StartCoroutine(Player_Defend());
                    break;

                case PLAYER_STATE.INJURED:
                    StartCoroutine(Player_Injured());
                    break;

                case PLAYER_STATE.DEAD:
                    StartCoroutine(Player_Dead());
                    break;

            }

        }

    }
    //-------------------------------------------------------
    public IEnumerator Player_Idle() {

        while (currentState == PLAYER_STATE.IDLE) {
  
            Quaternion targetRotation = Quaternion.LookRotation(m_Red.transform.position - transform.position);
            m_PlayerTrans.rotation = Quaternion.Slerp(m_PlayerTrans.rotation, targetRotation, Time.time * 0.07f);

            m_Anim.SetBool("Idle", true);
            m_Anim.SetBool("Move", false);
            m_Anim.SetBool("AttackL", false);
            m_Anim.SetBool("AttackR", false);
            m_Anim.SetBool("Defend", false);

            yield return null;

        }

        yield break;

    }

    public IEnumerator Player_Move() {

        while (currentState == PLAYER_STATE.MOVE) {
            m_Anim.SetBool("Idle", false);
            m_Anim.SetBool("Move", true);
            m_Anim.SetBool("AttackL", false);
            m_Anim.SetBool("AttackR", false);
            m_Anim.SetBool("Defend", false);

            m_Speed = moveSpeed;

            Plane playerPlane = new Plane(Vector3.up, Stage.transform.position.y + 0.4f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo = new RaycastHit();
            float hitdist = 0.0f;


            if (playerPlane.Raycast(ray, out hitdist)) {

                Vector3 targetPoint = ray.GetPoint(hitdist);
                destinationPos = ray.GetPoint(hitdist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                m_PlayerTrans.rotation = Quaternion.Slerp(m_PlayerTrans.rotation, targetRotation, Time.time * 0.07f);

                var Range = Vector3.Distance(m_PlayerTrans.position, targetPoint);


                m_PlayerTrans.position = Vector3.MoveTowards(m_PlayerTrans.position, destinationPos, m_Speed * Time.deltaTime);


                if (Range <= setMovementRange) {     //This is to stop the char from continuing to try to hit the target while in input touch down

                    inRange = false;
                    m_Speed = 0.0f;
                    m_PlayerTrans.position = this.transform.position;
                    CurrentState = PLAYER_STATE.IDLE;

                }

                if (Input.GetMouseButtonUp(0)) {

                    inRange = false;
                    m_Speed = 0.0f;
                    m_PlayerTrans.position = this.transform.position;
                    CurrentState = PLAYER_STATE.IDLE;

                }

            }

            yield return null;

        }

        yield break;
    }



    public IEnumerator Player_AttackL() {

        while (currentState == PLAYER_STATE.ATTACKL) {


            if (IsPointerOverUIObject()) {

                m_Anim.SetBool("Idle", false);
                m_Anim.SetBool("Move", false);
                m_Anim.SetBool("AttackL", true);
                m_Anim.SetBool("AttackR", false);
                m_Anim.SetBool("Defend", false);
      
            }

            yield return null;
            CurrentState = PLAYER_STATE.IDLE;

        }

        yield break;

    }

    public IEnumerator Player_AttackR() {

        while (currentState == PLAYER_STATE.ATTACKR) {


            if (IsPointerOverUIObject()) {

                m_Anim.SetBool("Idle", false);
                m_Anim.SetBool("Move", false);
                m_Anim.SetBool("AttackL", false);
                m_Anim.SetBool("AttackR", true);
                m_Anim.SetBool("Defend", false);
   
            }

            yield return null;
            CurrentState = PLAYER_STATE.IDLE;

        }

        yield break;

    }

    public IEnumerator Player_Defend() {

        while (currentState == PLAYER_STATE.DEFEND) {

            Vector3 relativePos = m_Red.transform.position - this.transform.position;

            if (IsPointerOverUIObject()) {

                Quaternion lookAtTarget = Quaternion.LookRotation(relativePos);
                this.transform.rotation = lookAtTarget;
                m_Anim.SetBool("Idle", false);
                m_Anim.SetBool("Move", false);
                m_Anim.SetBool("AttackL", false);
                m_Anim.SetBool("AttackR", false);
                m_Anim.SetBool("Defend", true);

            }

            yield return new WaitForSeconds(1);
            CurrentState = PLAYER_STATE.IDLE;

        }

        yield break;

    }

    public IEnumerator Player_Injured() {

        while (currentState == PLAYER_STATE.INJURED) {
            yield return null;
        }

        yield break;

    }

    public IEnumerator Player_Dead() {

        while (currentState == PLAYER_STATE.DEAD) {
            yield return null;
        }

        yield break;

    }
    //---------------------------------------------------------

    //Attack
    public void B_Attack_1L() {

        //StartCoroutine(Player_AttackL());
        CurrentState = PLAYER_STATE.ATTACKL;
        return;

    }

    public void B_Attack_1R() {

        //StartCoroutine(Player_AttackR());
        CurrentState = PLAYER_STATE.ATTACKR;
        return;

    }

    //Defend
    public void B_Defend_1() {

        CurrentState = PLAYER_STATE.DEFEND;
        return;

    }



    private bool IsPointerOverUIObject() {

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;

    }
}
