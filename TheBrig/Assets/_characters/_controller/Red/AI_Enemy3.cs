using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Enemy3 : MonoBehaviour {

    public float rotSpeed = 0.5f;        

    //bools for universal awarerness
    public bool idle = false;
    public bool moving = false;
    public bool attacking = false;
    public bool injured = false;
    public bool dead = false;
    //----------
    private Transform m_Player;
    private NavMeshAgent m_Agent;
    private Animator m_Anim;   
    private Vector3 direction;
    private float distance;


    void Start () {

        m_Player = GameObject.FindGameObjectWithTag("Blue").GetComponent<Transform>(); 
        m_Agent = GetComponent<NavMeshAgent>();
        m_Anim = GetComponent<Animator>();
        CurrentState = RED_STATE.IDLE;
        m_Agent.updatePosition = false;

    }
	

	void Update () {

         //direction
        direction = m_Player.position - this.transform.position;
        direction.y = 0.0f;
         //range
        distance = Vector3.Distance(m_Player.position, this.transform.position);

        //if (distance >= 0.04f){

        //    idle = false;
        //    moving = true;
        //    attacking = false;
        //    direction = m_Player.position - this.transform.position;
        //    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
        //    m_Agent.isStopped = false;
        //    m_Agent.destination = m_Player.transform.position;
        //    m_Anim.SetBool("Idle", false);
        //    m_Anim.SetBool("Chase", true);
        //    m_Anim.SetBool("Attack", false);

        //}   

        if (Vector3.Distance(m_Player.position, this.transform.position) > 0.03f && !attacking) {

            CurrentState = RED_STATE.MOVE;                     

        }

        //if (Vector3.Distance(m_Player.position, this.transform.position) < 0.02f && !moving) {

        //    StartCoroutine(Red_AttackL());
            

        //} else { attacking = false; };

    }

    //-------------Player finite state machine

    public enum RED_STATE { IDLE, MOVE, ATTACKL, ATTACKR, DEFEND, INJURED, DEAD };

    [SerializeField]
    private RED_STATE currentState = RED_STATE.IDLE;

    // get private currentState from public encapsulation and return corresponding state
    public RED_STATE CurrentState {

        get { return currentState; }
        set {
            currentState = value;

            StopAllCoroutines();

            switch (currentState) {
                case RED_STATE.IDLE:
                    StartCoroutine(Red_Idle());
                    break;

                case RED_STATE.MOVE:
                    StartCoroutine(Red_Move());
                    break;

                case RED_STATE.ATTACKL:
                    StartCoroutine(Red_AttackL());
                    break;

                case RED_STATE.ATTACKR:
                    StartCoroutine(Red_AttackR());
                    break;

                case RED_STATE.DEFEND:
                    StartCoroutine(Red_Defend());
                    break;

                case RED_STATE.INJURED:
                    StartCoroutine(Red_Injured());
                    break;
             
                case RED_STATE.DEAD:
                    StartCoroutine(Red_Dead());
                    break;

            }

        }

    }
    //-------------------------------------------------------

    public IEnumerator Red_Idle() {

        idle = true;
        moving = false;
        attacking = false;
        //m_Anim
        m_Agent.isStopped = true;
        m_Agent.destination = this.transform.position;
        m_Anim.SetBool("Idle", true);
        m_Anim.SetBool("Chase", false);
        m_Anim.SetBool("Attack", false);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);

        if(distance >= m_Agent.stoppingDistance && m_Agent.hasPath){

            CurrentState = RED_STATE.MOVE;
            yield break;

        }

        yield return null;

    }

    public IEnumerator Red_Move() {

        idle = false;
        moving = true;
        attacking = false;
        //m_Anim
        m_Agent.isStopped = true;
        m_Agent.destination = m_Player.transform.position;
        m_Anim.SetBool("Idle", false);
        m_Anim.SetBool("Chase", true);
        m_Anim.SetBool("Attack", false);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);

        //Nav Mesh movement   
        m_Agent.isStopped = false;
        m_Agent.destination = m_Player.transform.position;

        //Check if Agent is at destination
        if (distance <= m_Agent.stoppingDistance){

            m_Agent.isStopped = true;
            CurrentState = RED_STATE.IDLE;

        }

        yield return null;

    }


    public IEnumerator Red_AttackL(){

        idle = false;
        moving = false;
        attacking = true;   

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);

        m_Agent.isStopped = true;
        m_Agent.destination = this.transform.position;
        m_Anim.SetBool("Idle", false);
        m_Anim.SetBool("Chase", false);
        m_Anim.SetBool("Attack", true);

        yield return null;

    }

    public IEnumerator Red_AttackR() {

        idle = false;
        moving = false;
        attacking = true;         
        
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);

        m_Agent.isStopped = true;
        m_Agent.destination = this.transform.position;
        m_Anim.SetBool("Idle", false);
        m_Anim.SetBool("Chase", false);
        m_Anim.SetBool("Attack", true);

        yield return null;

    }

    public IEnumerator Red_Defend() {

        idle = false;
        moving = false;
        attacking = false;

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);

        m_Agent.isStopped = true;
        m_Agent.destination = this.transform.position;
        m_Anim.SetBool("Idle", false);
        m_Anim.SetBool("Chase", false);
        m_Anim.SetBool("Attack", false);

        yield return null;

    }

    public IEnumerator Red_Injured() {

        idle = false;
        moving = false;
        attacking = false;

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);

        m_Agent.isStopped = true;
        m_Agent.destination = this.transform.position;
        m_Anim.SetBool("Idle", false);
        m_Anim.SetBool("Chase", false);
        m_Anim.SetBool("Attack", false);

        yield return null;

    }

    public IEnumerator Red_Dead() {

        idle = false;
        moving = false;
        attacking = false;
        dead = true;

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);

        m_Agent.isStopped = true;
        m_Agent.destination = this.transform.position;
        m_Anim.SetBool("Idle", false);
        m_Anim.SetBool("Chase", false);
        m_Anim.SetBool("Attack", false);

        yield return null;

    }

    private void OnAnimatorMove() {

        transform.position = m_Agent.nextPosition;

    }
}
