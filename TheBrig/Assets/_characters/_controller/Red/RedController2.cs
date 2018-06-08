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

    //Bool to set attacking state
    public bool isAttacking;
    //Bool to detect movement
    public bool isMoving;


    private void Awake() {

        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();
        m_Blue = GameObject.FindGameObjectWithTag("Blue").transform;


    }

    private void Start() {

        CurrentState = RED_STATE.IDLE;

    }


    // Standard updates
    private void Update() {

        //Rangefinding and direction
        range = Vector3.Distance(m_Blue.transform.position, this.transform.position);
        direction = m_Blue.position - this.transform.position;
        direction.y = 0.0f;

        //Set Idle if distance to Blue is less than movement range and Red is not currently attacking
        if (range <= movementRange && !isAttacking) {

            CurrentState = RED_STATE.IDLE;

        }

        //Red Move to Blue
        if (range >= movementRange) {

            //Switch
            CurrentState = RED_STATE.MOVE;

        }

        //Red Attack
        if (range <= attackRange) {

            //Switch
            CurrentState = RED_STATE.ATTACKING;

        }

    }

    public enum RED_STATE { IDLE, MOVE,ATTACKING, ATTACKL, ATTACKR, ATTACKCOMBO};
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

                case RED_STATE.ATTACKING:
                    StartCoroutine(RedAttacking());
                    break;

                case RED_STATE.ATTACKL:
                    StartCoroutine(RedAttackL());
                    break;

                case RED_STATE.ATTACKR:
                    StartCoroutine(RedAttackR());
                    break;

                case RED_STATE.ATTACKCOMBO:
                    StartCoroutine(RedAttackCombo());
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
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotateSpeed * Time.deltaTime);
          
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

            //Movement
            this.transform.position = Vector3.Lerp(this.transform.position, direction, moveSpeed * Time.deltaTime);
            //Rotation
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotateSpeed * Time.deltaTime);

            yield return null;

        }

    }

    public IEnumerator RedAttacking() {

        while (true) {
            //Debug.Log("In Attacking");
            isAttacking = true;
            System.Random randomizer = new System.Random();
            int attackToChoose = randomizer.Next(3);

            switch (attackToChoose) {

                case 0:
                    CurrentState = RED_STATE.ATTACKL;
                    //Debug.Log("switch Attack L");
                    break;

                case 1:
                    CurrentState = RED_STATE.ATTACKR;
                    //Debug.Log("switch Attack R");
                    break;

                case 2:
                    CurrentState = RED_STATE.ATTACKCOMBO;
                    //Debug.Log("switch Attack R");
                    break;

            }

            if(range > attackRange){

                isAttacking = false;
                CurrentState = RED_STATE.IDLE;

            }

            yield return null;

        }
    }

    public IEnumerator RedAttackL() {

        while (true) {
    
            //Rotation
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotateSpeed * Time.deltaTime);

            m_Anim.SetBool("Idle", false);
            m_Anim.SetBool("Move", false);
            m_Anim.SetBool("AttackL", true);
            m_Anim.SetBool("AttackR", false);
            m_Anim.SetBool("AttackCombo", false);
            m_Anim.SetBool("Defend", false);

            yield return new WaitForSeconds(0.5f);
            //isAttacking = false;
            CurrentState = RED_STATE.ATTACKING;

            yield return null;

        }
    }

    public IEnumerator RedAttackR() {

        while (true) {

            //Rotation
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotateSpeed * Time.deltaTime);

            m_Anim.SetBool("Idle", false);
            m_Anim.SetBool("Move", false);
            m_Anim.SetBool("AttackL", false);
            m_Anim.SetBool("AttackR", true);
            m_Anim.SetBool("AttackCombo", false);
            m_Anim.SetBool("Defend", false);

            yield return new WaitForSeconds(0.5f);
            //isAttacking = false;
            CurrentState = RED_STATE.ATTACKING;

            yield return null;

        }
    }

    public IEnumerator RedAttackCombo() {

        while (true) {

            //Rotation
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotateSpeed * Time.deltaTime);

            m_Anim.SetBool("Idle", false);
            m_Anim.SetBool("Move", false);
            m_Anim.SetBool("AttackL", false);
            m_Anim.SetBool("AttackR", false);
            m_Anim.SetBool("AttackCombo", true);
            m_Anim.SetBool("Defend", false);

            yield return new WaitForSeconds(0.5f);
            //isAttacking = false;
            CurrentState = RED_STATE.ATTACKING;

            yield return null;

        }
    }
}