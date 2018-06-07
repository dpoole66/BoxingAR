using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]

public class MiniInputController : MonoBehaviour {

    [HideInInspector] public Animator m_Anim;
    [HideInInspector] public Rigidbody m_Rigid;
    public Transform m_Enemy;
    private bool move = false;
    private Transform m_PlayerTrans;
    //added controller vars
    private float m_Distance;
    private Vector3 m_Direction;
    //--
    private Vector3 destinationPos;
    private float destinationDis;
    private bool inRange = false;
    public float setMovementRange = 0.001f;
    public float Speed = 0.3f;
    private float m_Speed;
    //Combat
    public float enGuardRange = 0.2f;
    public float AttackLRange = 0.1f;
    private Button b_AttackL, b_Defend;


    void Start() {

        Physics.gravity = new Vector3(0, -200f, 0);
        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();
        m_PlayerTrans = transform;
        destinationPos = m_PlayerTrans.position;
        m_Speed = Speed;
        m_Enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        b_AttackL = GameObject.FindGameObjectWithTag("ButtonAttackL").GetComponent<Button>();
        b_Defend = GameObject.FindGameObjectWithTag("ButtonDefend").GetComponent<Button>();

        //combat buttons
        b_AttackL.onClick.AddListener(() => B_AttackL_1());
        b_Defend.onClick.AddListener(() => B_Defend_1());

    }

    private void Update() {
        if (m_Enemy != null) {

            Debug.Log(m_Enemy.position);

        }

        //Combat
        var combatRange = Vector3.Distance(m_PlayerTrans.position, m_Enemy.position);

        //Rotation, direction and distance
        m_PlayerTrans.transform.rotation = Quaternion.Slerp(m_PlayerTrans.transform.rotation, Quaternion.LookRotation(m_Direction), 0.5f);
        m_Distance = Vector3.Distance(m_PlayerTrans.position, m_Enemy.position);
        m_Direction = m_Enemy.position - m_PlayerTrans.transform.position;
        m_Direction.y = 0.0f;              


        ////Movement
        ////speed in reference to distance
        //if (m_Speed <= 0.1f) {

        //    m_Anim.SetBool("AttackL", false);
        //    m_Anim.SetBool("Defend", false);
        //    m_Anim.SetBool("Move", false);
        //    m_Anim.SetBool("Idle", true);

        //} else if (m_Speed >= 0.2f) {

        //    m_Anim.SetBool("AttackL", false);
        //    m_Anim.SetBool("Defend", false);
        //    m_Anim.SetBool("Move", true);
        //    m_Anim.SetBool("Idle", false);

        //}

        //touch/mouse input to move player
        if (Input.GetMouseButton(0) && !inRange && !IsPointerOverUIObject()) {

            //Touch awareness
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {
                return;
            }

            //m_Speed = 3.0f;
            Plane playerPlane = new Plane(Vector3.up, m_PlayerTrans.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float hitdist = 0.0f;


            if (playerPlane.Raycast(ray, out hitdist)) {

                Vector3 targetPoint = ray.GetPoint(hitdist);
                destinationPos = ray.GetPoint(hitdist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                m_PlayerTrans.rotation = targetRotation;

                var Range = Vector3.Distance(m_PlayerTrans.position, targetPoint);

                m_PlayerTrans.position = Vector3.MoveTowards(m_PlayerTrans.position, destinationPos, Speed * Time.deltaTime);
                m_Anim.SetBool("AttackL", false);
                m_Anim.SetBool("Defend", false);
                m_Anim.SetBool("Move", true);
                m_Anim.SetBool("Idle", false);

                if (Range <= setMovementRange) {     //This is to stop the char from continuing to try to hit the target while in input touch down

                    inRange = true;
                    m_Anim.SetBool("AttackL", false);
                    m_Anim.SetBool("Defend", false);
                    m_Anim.SetBool("Move", false);
                    m_Anim.SetBool("Idle", true);

                }

            }

        } else {

            m_Anim.SetBool("AttackL", false);
            m_Anim.SetBool("Defend", false);
            m_Anim.SetBool("Move", false);
            m_Anim.SetBool("Idle", true);

        }

        if (Input.GetMouseButtonUp(0)) {

            m_Anim.SetBool("AttackL", false);
            m_Anim.SetBool("Defend", false);
            m_Anim.SetBool("Move", false);
            m_Anim.SetBool("Idle", true);

        }

    }

    // Update is called once per frame
    void FixedUpdate() {

        //monitor distance between player and destinationPos
        destinationDis = Vector3.Distance(destinationPos, m_PlayerTrans.position);

    }

    //Combat
    public void B_AttackL_1() {     //UI AttackL button

        StartCoroutine(AttackL_1());
        return;

    }
    public IEnumerator AttackL_1() {    //AttackL coro

        //Vector3 relativePos = m_Enemy.position - m_PlayerTrans.position;
        //Quaternion lookAtTarget = Quaternion.LookRotation(relativePos);
        //m_PlayerTrans.rotation = lookAtTarget;
        m_PlayerTrans.transform.rotation = Quaternion.Slerp(m_PlayerTrans.transform.rotation, Quaternion.LookRotation(m_Direction), 0.5f);
        m_Anim.SetBool("AttackL", true);
        m_Anim.SetBool("Defend", false);
        m_Anim.SetBool("Move", false);
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
        m_Anim.SetBool("AttackL", false);
        m_Anim.SetBool("Defend", true);
        m_Anim.SetBool("Move", false);
        m_Anim.SetBool("Idle", false);
        yield break;

    }

    private bool IsPointerOverUIObject() {

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;

    }
}