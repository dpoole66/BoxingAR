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
    public GameObject m_Blue;
    private bool move = false;
    private Transform m_Red;
    private Vector3 destinationPos;
    private float destinationDis;
    private bool inRange = false;          
    public float moveSpeed = 0.60f;
    public float rotSpeed = 0.5f;
    private float m_Speed;
    public Slider m_Health;
    public Text m_Score;
    private int score = 0;
    public string Opponent;

    //Combat
    float targetRange;
    public bool hit = false;
    bool dead = false;
    float speed;
    [Range(0.5f, 0.8f)]
    [SerializeField]
    private float caution;
    [Range(0.25f, 0.5f)]
    [SerializeField]
    private float danger;

    private void Awake() {

        Physics.gravity = new Vector3(0, -200f, 0);
        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();
        m_Red = GetComponent<Transform>();
        //m_Score = m_Blue.GetComponent<Text>();

    }

    void Start() {

        m_Health.value = 100;
        CurrentState = PLAYER_STATE.IDLE;

    }

    //Collison
    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag != Opponent)
            return;

        hit = true;
        m_Health.value -= 10;
        score = score + 10;
        m_Score.text = score.ToString();

        if(m_Health.value > 0){

            dead = false;
            return;

        }   else if (m_Health.value <= 0){

            dead = true;
            return;

        }   

    }

    // Update is called once per frame
    private void Update() {
        if (m_Blue != null) {

            Debug.Log("Found Blue");

        }

        Debug.Log(hit);

        //If dead, stop here and return
        if (m_Health.value < 0)
            return;


        //Rangefinder
        targetRange = Vector3.Distance(m_Red.position, m_Blue.transform.position);


        if (!hit && !dead && targetRange >= 0.75f) {

            CurrentState = PLAYER_STATE.MOVE;

        }
        
        if(!hit && !dead && targetRange <= 0.75f){

            CurrentState = PLAYER_STATE.IDLE;

        }  else{ 

        CurrentState = currentState; 

        }

        if (hit == true) {

            CurrentState = PLAYER_STATE.HIT;

        }


        if (dead == true) {

            dead = false;
            CurrentState = PLAYER_STATE.DEAD;

        }



    }



    //-------------Player finite state machine

    public enum PLAYER_STATE { IDLE, MOVE, ATTACK, RETREAT, DEFEND, STUN, HIT, DEAD, DEBUG };

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

                case PLAYER_STATE.RETREAT:
                    StartCoroutine(Player_Retreat());
                    break;

                case PLAYER_STATE.DEFEND:
                    StartCoroutine(Player_Defend());
                    break;

                case PLAYER_STATE.STUN:
                    StartCoroutine(Player_Stun());
                    break;

                case PLAYER_STATE.HIT:
                    StartCoroutine(Player_Hit());
                    break;

                case PLAYER_STATE.DEAD:
                    StartCoroutine(Player_Dead());
                    break;

                case PLAYER_STATE.DEBUG:
                    StartCoroutine(Player_Debug());
                    break;

            }

        }

    }
    //-------------------------------------------------------
    public IEnumerator Player_Idle() {

        while (currentState == PLAYER_STATE.IDLE) {
   
            Quaternion targetRotation = Quaternion.LookRotation(m_Blue.transform.position - transform.position);
            m_Red.rotation = Quaternion.Slerp(m_Red.rotation, targetRotation, Time.time * 0.07f);

            Debug.Log("Red Idle");
   
            m_Anim.SetBool("Idle", true);
            m_Anim.SetBool("Move", false);
            m_Anim.SetBool("Stun", false);
     
            yield return null;

         }

        yield break;

    }

    public IEnumerator Player_Move() {

        while (currentState == PLAYER_STATE.MOVE) {

          
            m_Anim.SetBool("Move", true);
 

            //Rotate
            Vector3 targetPoint = m_Blue.transform.position;     
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            m_Red.rotation = Quaternion.Slerp(m_Red.rotation, targetRotation, Time.time * 1.0f);

            //Move
            if(!dead){

                m_Speed = moveSpeed;
                m_Red.position = Vector3.MoveTowards(m_Red.position, m_Blue.transform.position, m_Speed * Time.deltaTime);

            }
 
            yield return null;

          

        }

        yield break;

    }



    public IEnumerator Player_Attack() {

        while (currentState == PLAYER_STATE.ATTACK) {  

            foreach (AnimatorControllerParameter parameter in m_Anim.parameters) {
                if (parameter.type == AnimatorControllerParameterType.Bool)
                    m_Anim.SetBool(parameter.name, false);
            }


            m_Anim.SetBool("AttackL", true);


            yield return null;

            m_Anim.SetBool("AttackL", false);

        }

        CurrentState = PLAYER_STATE.IDLE;

        yield break;

    }

    public IEnumerator Player_Retreat() {

        while (currentState == PLAYER_STATE.RETREAT) {

            foreach (AnimatorControllerParameter parameter in m_Anim.parameters) {
                if (parameter.type == AnimatorControllerParameterType.Bool)
                    m_Anim.SetBool(parameter.name, false);
            }


            m_Anim.SetBool("Backup", true);
   

            yield return null;
       

        }

        CurrentState = PLAYER_STATE.IDLE;

        yield break;

    }

    public IEnumerator Player_Defend() {

        while (currentState == PLAYER_STATE.DEFEND) {

            foreach (AnimatorControllerParameter parameter in m_Anim.parameters) {
                if (parameter.type == AnimatorControllerParameterType.Bool)
                    m_Anim.SetBool(parameter.name, false);
            }

            m_Anim.SetBool("Defend", true);


            yield return null;
        
            CurrentState = PLAYER_STATE.IDLE;    
         }

        yield break;

    }


    public IEnumerator Player_Stun() {

        while (currentState == PLAYER_STATE.STUN) {

            m_Anim.SetBool("HitBody", false);
            m_Anim.SetBool("Idle", false);
            m_Anim.SetBool("Stun", true);
            yield return null;

        }
        
        CurrentState = PLAYER_STATE.IDLE;

        yield break;

    }

    public IEnumerator Player_Dead() {


        while (currentState == PLAYER_STATE.DEAD) {

            foreach (AnimatorControllerParameter parameter in m_Anim.parameters) {
                if (parameter.type == AnimatorControllerParameterType.Bool)
                    m_Anim.SetBool(parameter.name, false);
            }

            m_Anim.SetBool("Dead", true);

            yield return null;

            yield return new WaitForSeconds(2);
            m_Health.value = 100;
            CurrentState = PLAYER_STATE.IDLE;

        }

        yield break;

    }

    public IEnumerator Player_Hit() {

        

        while (currentState == PLAYER_STATE.HIT) {

            m_Anim.SetBool("Stun", true);
            m_Anim.SetBool("Idle", false);

            yield return null;

        }

        hit = false;
        CurrentState = PLAYER_STATE.IDLE;

        yield break;

    }

    public IEnumerator Player_Debug() {

        while (currentState == PLAYER_STATE.DEBUG) {

            m_Anim.SetBool("HitHead", true);
            hit = false;

            yield return null;

        }

    }

}


