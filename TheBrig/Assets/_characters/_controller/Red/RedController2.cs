using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GoogleARCore;

public class RedController2
: MonoBehaviour {

    [HideInInspector] public Animator m_Anim;
    [HideInInspector] public Rigidbody m_Rigid;
    public Transform m_Blue;
    public float moveSpeed = 0.7f;
    public float rotateSpeed = 0.7f;
    public float movementRange = 0.05f;
    public float attackRange = 0.03f;
    private Vector3 direction;
    private float range;
    private float speed;
    private Transform Blue;


    private void Awake() {

        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();
        m_Blue = GameObject.FindGameObjectWithTag("Blue").transform;


    }

    private void Start() {

        CurrentState = RED_STATE.IDLE;

    }

    // Rigidbody velocity update
    private void FixedUpdate() {

        speed = m_Rigid.velocity.magnitude;
        Debug.Log(speed);

    }

    // Standard updates
    private void Update() {

        if (speed < 0.05f) {

            CurrentState = RED_STATE.IDLE;

        }

        //Rangefinding and direction
        range = Vector3.Distance(m_Blue.transform.position, this.transform.position);
        direction = m_Blue.position - this.transform.position;
        direction.y = 0.0f;

        //Red Move to Blue
        if(range >= movementRange) {

            //Switch
            CurrentState = RED_STATE.MOVE;

        }

        //Red Attack
        if (range <= attackRange) {

            //Switch
            CurrentState = RED_STATE.ATTACK;

        }

    }

    public enum RED_STATE { IDLE, MOVE, ATTACK };
    [SerializeField] RED_STATE currentState = RED_STATE.IDLE;
    public RED_STATE CurrentState {

        get { return currentState; }
        set {

            currentState = value;
            StopAllCoroutines();

            switch (currentState) {

                case RED_STATE.IDLE:
                    StartCoroutine(RedIdle());
                    break;

                case RED_STATE.MOVE:
                    StartCoroutine(RedMove());
                    break;

                case RED_STATE.ATTACK:
                    StartCoroutine(RedAttack());
                    break;

            }

        }

    }

    public IEnumerator RedIdle() {

        while (true) {
            m_Anim.SetBool("Idle", true);
            m_Anim.SetBool("Move", false);
            m_Anim.SetBool("AttackL", false);
            m_Anim.SetBool("AttackR", false);
            m_Anim.SetBool("Defend", false);

            //Rotation
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotateSpeed);
   
            yield return null;

        }

    }

    public IEnumerator RedMove() {

        while (true) {
            m_Anim.SetBool("Idle", false);
            m_Anim.SetBool("Move", true);
            m_Anim.SetBool("AttackL", false);
            m_Anim.SetBool("AttackR", false);
            m_Anim.SetBool("Defend", false);

            this.transform.position = Vector3.MoveTowards(this.transform.position, m_Blue.transform.position, moveSpeed * Time.deltaTime);

            yield return null;

        }

    }

    public IEnumerator RedAttack() {

        while (true) {
            m_Anim.SetBool("Idle", false);
            m_Anim.SetBool("Move", false);
            m_Anim.SetBool("AttackL", true);
            m_Anim.SetBool("AttackR", false);
            m_Anim.SetBool("Defend", false);

            yield return null;

        }
    }
}