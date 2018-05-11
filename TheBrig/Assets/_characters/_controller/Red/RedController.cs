using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GoogleARCore;

public class RedController : MonoBehaviour {

    [HideInInspector] public Animator m_Anim;
    [HideInInspector] public Rigidbody m_Rigid;
    public Transform m_Blue;
    private bool move = false;
    private Transform m_Red;
    private Vector3 destinationPos;
    private float destinationDis;
    private bool inRange = false;          
    public float moveSpeed = 0.60f;
    public float rotSpeed = 0.5f;
    private float m_Speed;

    //Combat
    float targetRange;

    private void Awake() {

        Physics.gravity = new Vector3(0, -200f, 0);
        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();
        m_Red = GetComponent<Transform>();
        m_Blue = GameObject.FindGameObjectWithTag("Player").transform;


    }

    void Start() {

        CurrentState = PLAYER_STATE.IDLE;

    }

    // Update is called once per frame
    private void Update() {
        if (m_Blue != null) {

            Debug.Log("Found Blue");

        }           


        //Rangefinder
        targetRange = Vector3.Distance(m_Red.position, m_Blue.position);


        if (targetRange >= 0.75f) {

            CurrentState = PLAYER_STATE.MOVE;

        }else if(targetRange <= 0.75f){

            CurrentState = PLAYER_STATE.IDLE;

        }
    }



    //-------------Player finite state machine

    public enum PLAYER_STATE { IDLE, MOVE, ATTACK, DEFEND, INJURED, DEAD };

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

                case PLAYER_STATE.ATTACK:
                    StartCoroutine(Player_Attack());
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

            Quaternion targetRotation = Quaternion.LookRotation(m_Blue.transform.position - transform.position);
            m_Red.rotation = Quaternion.Slerp(m_Red.rotation, targetRotation, Time.time * 0.07f);

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

            //Rotate
            Vector3 targetPoint = m_Blue.transform.position;     
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            m_Red.rotation = Quaternion.Slerp(m_Red.rotation, targetRotation, Time.time * 1.0f);
            //Move
            m_Speed = moveSpeed;
            m_Red.position = Vector3.MoveTowards(m_Red.position, m_Blue.position, m_Speed * Time.deltaTime);
    
 
            yield return null;

         }

        yield break;

    }



    public IEnumerator Player_Attack() {

        while (currentState == PLAYER_STATE.ATTACK) {
    
            m_Anim.SetBool("Idle", false);
            m_Anim.SetBool("Move", false);
            m_Anim.SetBool("AttackL", false);
            m_Anim.SetBool("AttackR", false);
            m_Anim.SetBool("Defend", false);
 
            yield return new WaitForSeconds(2);
            CurrentState = PLAYER_STATE.IDLE;

         }

        yield break;

    }

    public IEnumerator Player_Defend() {

        while (currentState == PLAYER_STATE.DEFEND) {

                m_Anim.SetBool("Idle", false);
                m_Anim.SetBool("Move", false);
                m_Anim.SetBool("AttackL", false);
                m_Anim.SetBool("AttackR", false);
                m_Anim.SetBool("Defend", true);

                yield return new WaitForSeconds(2);
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

}
