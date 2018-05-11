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
    private Vector3 destinationPos;
    private float destinationDis;
    private bool inRange = false;
    public float setMovementRange = 0.001f;
    public float Speed = 0.3f;
    private float m_Speed;
    //Combat
    public float enGuardRange = 0.2f;
    public float attackRange = 0.1f;
    private Button b_Attack, b_Defend;


    void Start() {

        Physics.gravity = new Vector3(0, -200f, 0);
        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();
        m_PlayerTrans = transform;
        destinationPos = m_PlayerTrans.position;
        m_Speed = Speed;
        m_Enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        b_Attack = GameObject.FindGameObjectWithTag("ButtonAttack").GetComponent<Button>();
        b_Defend = GameObject.FindGameObjectWithTag("ButtonDefend").GetComponent<Button>();

        //combat buttons
        b_Attack.onClick.AddListener(() => B_Attack_1());
        b_Defend.onClick.AddListener(() => B_Defend_1());

    }

    private void Update() {
        if (m_Enemy != null) {

            Debug.Log(m_Enemy.position);

        }

        //Combat
        var combatRange = Vector3.Distance(m_PlayerTrans.position, m_Enemy.position);

        //Movement
        //speed in reference to distance
        if (m_Speed <= 0.1f) {

            m_Anim.ResetTrigger("Move");
            m_Anim.SetTrigger("Idle");

        } else if (m_Speed >= 0.2f) {

            m_Anim.ResetTrigger("Idle");
            m_Anim.SetTrigger("Move");

        }

        //touch/mouse input to move player
        if (Input.GetMouseButton(0) && !inRange && !IsPointerOverUIObject()) {
           
             //Touch awareness
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {    
                return;                   
            }

                m_Speed = 3.0f;
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
                    Debug.Log(Speed);

                    if (Range <= setMovementRange) {     //This is to stop the char from continuing to try to hit the target while in input touch down

                        inRange = true;
                        m_Speed = 0.0f;

                    }

                }

        } else {

            m_Speed = 0.0f;

        }

        if (Input.GetMouseButtonUp(0)) {

            inRange = false;
            m_Speed = 0.0f;

        }

    }

    // Update is called once per frame
    void FixedUpdate() {

        //monitor distance between player and destinationPos
        destinationDis = Vector3.Distance(destinationPos, m_PlayerTrans.position);

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
