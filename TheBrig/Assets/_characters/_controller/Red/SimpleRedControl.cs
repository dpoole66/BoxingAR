using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using GoogleARCore;

public class SimpleRedControl : MonoBehaviour {

    private Transform m_RedTrans;
    private Animator m_RedAnim; 
    public Transform m_BlueTrans;

    //Combat    
    float targetRange;

    //Health
    public float Health;
    public float MaxHealth = 100.0f;
    public float RegenAmt = 10.0f;
    public bool hit;
    public bool dead;
    public int HitCount;
    public Slider m_HealthUI;
    public Text m_ScoreUI;


    public enum RED_STATE {HIT, IDLE, CHASE, ATTACK, RETREAT, DIE}
    [SerializeField] private RED_STATE currentState = RED_STATE.IDLE;

    //Get and set CurrentState using priveat var currentState and switch case

    public RED_STATE CurrentState{

        get{ return currentState; }
        set{

                currentState = value;

                StopAllCoroutines();

            //Switching
            switch (currentState) {

                case RED_STATE.HIT:
                    StartCoroutine(RedHit());
                    break;

            }

            switch (currentState){

                    case RED_STATE.IDLE :
                        StartCoroutine(RedIdle());
                    break;

                }

            switch (currentState) {

                case RED_STATE.CHASE:
                    StartCoroutine(RedChase());
                    break;

            }

            switch (currentState) {

                case RED_STATE.ATTACK:
                    StartCoroutine(RedAttack());
                    break;

            }

            switch (currentState) {

                case RED_STATE.RETREAT:
                    StartCoroutine(RedRetreat());
                    break;

            }

            switch (currentState) {

                case RED_STATE.DIE:
                    StartCoroutine(RedDie());
                    break;

            }

        }
    }

    //Coroutines
    public IEnumerator RedHit() {

        while (currentState == RED_STATE.HIT) {

            m_RedAnim.SetBool("Hit", true);

            m_RedAnim.SetBool("Idle", false);
            m_RedAnim.SetBool("Attack", false);
            m_RedAnim.SetBool("Chase", false);
            m_RedAnim.SetBool("Retreat", false);


            Vector3 targetPoint = m_BlueTrans.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            m_RedTrans.rotation = Quaternion.Slerp(m_RedTrans.rotation, targetRotation, Time.time * 1.0f);

            //If hit again, retreat
            if (HitCount >= 1 & Health >= 90.0) {
         
                CurrentState = RED_STATE.ATTACK;

            }   

            yield return null;

        }

        yield break;

    }

    public IEnumerator RedIdle() {

         while(currentState == RED_STATE.IDLE){

            m_RedAnim.SetBool("Idle", true);

            m_RedAnim.SetBool("Hit", false);
            m_RedAnim.SetBool("Attack", false);
            m_RedAnim.SetBool("Chase", false);
            m_RedAnim.SetBool("Retreat", false);


            Vector3 targetPoint = m_BlueTrans.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            m_RedTrans.rotation = Quaternion.Slerp(m_RedTrans.rotation, targetRotation, Time.time * 1.0f);


            if (hit == true) {

                hit = false;
                CurrentState = RED_STATE.HIT;

            }

            
            yield return null;

          }

        yield break;

    }

    public IEnumerator RedChase() {

        while (currentState == RED_STATE.CHASE) {

            m_RedAnim.SetBool("Chase", true);

            m_RedAnim.SetBool("Idle", false);
            m_RedAnim.SetBool("Hit", false);
            m_RedAnim.SetBool("Attack", false);   
            m_RedAnim.SetBool("Retreat", false);

            Vector3 targetPoint = m_BlueTrans.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            m_RedTrans.rotation = Quaternion.Slerp(m_RedTrans.rotation, targetRotation, Time.time * 1.0f);
            m_RedTrans.position = Vector3.MoveTowards(m_RedTrans.position, m_BlueTrans.transform.position, 1.0f * Time.deltaTime);

            if (targetRange <= 1.0f) {

                CurrentState = RED_STATE.IDLE;

            }

            yield return null;

        }

    }

    public IEnumerator RedAttack() {

        while (currentState == RED_STATE.ATTACK) {

            m_RedAnim.SetBool("Attack", true);

            m_RedAnim.SetBool("Idle", false);
            m_RedAnim.SetBool("Hit", false);
            m_RedAnim.SetBool("Chase", false);
            m_RedAnim.SetBool("Retreat", false);

            Vector3 targetPoint = m_BlueTrans.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            m_RedTrans.rotation = Quaternion.Slerp(m_RedTrans.rotation, targetRotation, Time.time * 1.0f);

            if(HitCount >=2 && Health <= 80.0f){

                hit = false;
                CurrentState = RED_STATE.RETREAT;

            } 
    
            yield return null;

         }


    }

    public IEnumerator RedRetreat() {

        while (currentState == RED_STATE.RETREAT) {


            m_RedAnim.SetBool("Retreat", true);

            m_RedAnim.SetBool("Hit", false);
            m_RedAnim.SetBool("Idle", false);
            m_RedAnim.SetBool("Attack", false);
            m_RedAnim.SetBool("Chase", false);

            yield return new WaitForSeconds(0.7f); 

            CurrentState = RED_STATE.IDLE;

            yield break;

         }

    }

    public IEnumerator RedDie() {

        while (currentState == RED_STATE.DIE) {

            m_RedAnim.SetBool("Die", true);
            yield return null;

         }

    }

   // Monobehaviour
    void Awake () {

        m_RedAnim = GetComponent<Animator>();
        m_RedTrans = GetComponent<Transform>();

	}

    void Update () {

        //Rangefinder
        targetRange = Vector3.Distance(m_RedTrans.position, m_BlueTrans.position);
        //Debug.Log(targetRange);

        if (targetRange >= 1.0f) {

            CurrentState = RED_STATE.CHASE;

        }

        
        if (Health <= MaxHealth) {

            Health += RegenAmt * Time.deltaTime;
            m_HealthUI.value = Health;
            Debug.Log(Health);

        } else if (Health > MaxHealth) {

            Health = MaxHealth;

        }
    }
}
