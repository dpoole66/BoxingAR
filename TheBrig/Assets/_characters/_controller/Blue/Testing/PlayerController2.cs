using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GoogleARCore;

[RequireComponent(typeof(Animator))]

public class PlayerController2 : MonoBehaviour {

    [HideInInspector] public Animator m_Anim;
    [HideInInspector] public Rigidbody m_Rigid;
    [HideInInspector] public NavMeshAgent m_Nav;
    public Transform m_Enemy;
    private bool move = false;
    private Transform m_PlayerTrans;
    private Vector3 destinationPos;
    public float setMovementRange = 0.1f;
    public float Speed = 1.0f;
    private float m_Speed;

    //Combat
    //public float enGuardRange;
    //public float attackRange;
    public Button b_Attack;
    public Button b_Defend;
   
    void Awake() {
        Physics.gravity = new Vector3(0, -200f, 0);
        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();
        m_Nav = GetComponent<NavMeshAgent>();
        m_PlayerTrans = this.transform;
        m_Enemy = GameObject.FindGameObjectWithTag("Enemy").transform;

        //attack and defend buttons
        b_Attack = GetComponent<Button>();
        b_Attack.onClick.AddListener(() => B_Attack_1());
        b_Defend = GetComponent<Button>();
        b_Defend.onClick.AddListener(() => B_Defend_1());


    }

    private void Start() {

        destinationPos = m_Nav.destination;
        m_Speed = Speed;

    }

    private void Update() {
        if (m_Enemy != null) {

            Debug.Log(m_Enemy.position);

        }

        //Combat
        var combatRange = Vector3.Distance(m_PlayerTrans.position, m_Enemy.position);

        if (Vector3.Distance(destinationPos, m_PlayerTrans.position) > 1.0f) {

            destinationPos = m_PlayerTrans.position;
            m_Nav.destination = destinationPos;

        }


        //attack
        if (Input.GetMouseButtonDown(1)) {

            StartCoroutine(Attack_1());

        }

        //defend
        if (Input.GetMouseButton(2)) {

            StartCoroutine(Defend_1());

        }


       

        //touch/mouse input to move player
        int tcTouches = Input.touchCount;

        if (tcTouches > 0) {

            for (int i = 0; i < Input.touchCount; i++) {

                Touch touch = Input.GetTouch(i);

                if (Input.GetTouch(i).phase == TouchPhase.Began && !IsPointerOverUIObject()) {

                    var targetPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                    m_Nav.destination = targetPoint;    
                    Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);    
                    m_Nav.transform.rotation = targetRotation;
          
                }
            }
        }   
    }

    private bool IsPointerOverUIObject() {

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;

    }

    //Methods

    //Combat
    public void B_Attack_1() {     //UI Attack button

        StartCoroutine(Attack_1());
        return;

    }
    public IEnumerator Attack_1() {    //Attack coro

        Vector3 relativePos = m_Enemy.position - m_PlayerTrans.position;
        Quaternion lookAtTarget = Quaternion.LookRotation(relativePos);
        m_PlayerTrans.rotation = lookAtTarget;
        m_Anim.SetBool("Attack", true);
        m_Anim.SetBool("Idle", false);

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
        m_Anim.SetBool("Attack", false);
        m_Anim.SetBool("Idle", true);

        yield break;

    }

}
