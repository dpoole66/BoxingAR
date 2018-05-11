using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GoogleARCore;

[RequireComponent(typeof(Animator))]

public class MiniTouchInputController : MonoBehaviour {


    [HideInInspector] public Animator m_Anim;   //MiniMovement
    [HideInInspector] public Rigidbody m_Rigid;
    [HideInInspector] public NavMeshAgent m_Nav;
    public Transform m_Enemy;
    private bool move = false;
    private Transform m_PlayerTrans;       //MiniMovement
    private bool inRange = false;

    public float speed = 0.3f;    //MiniMovement

    //Combat
    public float enGuardRange = 0.2f;
    public float attackRange = 0.1f;
    //UI
    public Button b_Attack, b_Defend;


    void Awake() {

        m_Anim = GetComponent<Animator>();
        m_Nav = GetComponent<NavMeshAgent>();
        m_Rigid = GetComponent<Rigidbody>();        
        m_Enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        //attack button
        b_Attack = GetComponentInChildren<Button>();
        b_Defend = GetComponentInChildren<Button>();

    }

    private void Start() {

        m_PlayerTrans = transform;  
        Physics.gravity = new Vector3(0, -200f, 0);
        b_Attack.onClick.AddListener(() => B_Attack_1());
        b_Defend.onClick.AddListener(() => B_Defend_1());

    }

    private void Update() {

        int tcTouches = Input.touchCount;

        if (tcTouches > 0) {

            for (int i = 0; i < Input.touchCount; i++) {

                Touch touch = Input.GetTouch(i);

                if (Input.GetTouch(i).phase == TouchPhase.Began && !IsPointerOverUIObject()) {

                        m_Nav.speed = speed;
                        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                        RaycastHit hit;

                    if (Physics.Raycast(ray, out hit)) {

                        Vector3 targetPoint = hit.point + hit.normal * 0.001f;
                        m_Nav.destination = targetPoint;
                        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                        m_Nav.transform.rotation = targetRotation;           

                    }
                } 
            }           
        } 

  

        //Combat
        var combatRange = Vector3.Distance(m_PlayerTrans.position, m_Enemy.position);

        //Movement
        //speed in reference to distance
        if (speed <= 0.1f) {

            m_Anim.SetBool("Idle", true);
            m_Anim.SetBool("Move", false);
            m_Anim.SetBool("Attack", false);
            m_Anim.SetBool("Defend", false);

        } else if (speed >= 0.2f) {

            m_Anim.SetBool("Idle", false);
            m_Anim.SetBool("Move", true);
            m_Anim.SetBool("Attack", false);
            m_Anim.SetBool("Defend", false);

        }

    }


    //UI Touch and Button control to prevent raycast passthrough
    private bool IsPointerOverUIObject() {

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;

    }

    //Combat
    public void B_Attack_1() {     //UI Attack button

        StartCoroutine(Attack_1());
        return;

    }
    public IEnumerator Attack_1() {    //Attack coro

        Vector3 relativePos = m_Enemy.position - m_PlayerTrans.position;
        Quaternion lookAtTarget = Quaternion.LookRotation(relativePos);
        m_PlayerTrans.rotation = lookAtTarget;
        m_Anim.SetBool("Idle", false);
        m_Anim.SetBool("Move", false);
        m_Anim.SetBool("Attack", true);
        m_Anim.SetBool("Defend", false);
        yield break;

    }

    //Defend
    public void B_Defend_1() {

        StartCoroutine(Defend_1());
        return;

    }

    public IEnumerator Defend_1() {     //Defend coro

        Vector3 relativePos = m_Enemy.position - m_PlayerTrans.position;
        Quaternion lookAtTarget = Quaternion.LookRotation(relativePos);
        m_PlayerTrans.rotation = lookAtTarget;
        m_Anim.SetBool("Idle", false);
        m_Anim.SetBool("Move", false);
        m_Anim.SetBool("Attack", false);
        m_Anim.SetBool("Defend", true);
        yield break;


    }
}
