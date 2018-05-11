using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]

public class PlayerInputController : MonoBehaviour {

    [HideInInspector] public Animator m_Anim;
    [HideInInspector] public Rigidbody m_Rigid;
    public Transform m_Enemy;
    public Text m_Debug;
    private GameObject StageInstance;
    private Transform m_PlayerTrans;
    private Vector3 destinationPos;
    public float MoveClamp;     
    public float setMovementRange = 0.001f;
    public float Speed = 0.3f;
    private float m_Speed;
    //Combat
    public float enGuardRange = 0.2f;
    public float attackRange = 0.1f;
    public Button b_Attack;
    public Button b_Defend;


    void Start() {

        Physics.gravity = new Vector3(0, -200f, 0);
        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();
        m_PlayerTrans = this.transform;
        destinationPos = m_PlayerTrans.position;                    
        m_Enemy = GetComponent<Transform>() ;
        //attack - defend buttons
        b_Attack = GetComponent<Button>();
        b_Attack.onClick.AddListener(() => B_Attack_1());
        b_Defend = GetComponent<Button>();
        b_Defend.onClick.AddListener(() => B_Defend_1());

    }

    private void Update() {
        if (m_Enemy != null) {

            m_Debug.text = m_Enemy.transform.position.ToString();

        }

        //Combat
        var combatRange = Vector3.Distance(m_PlayerTrans.position, m_Enemy.position);
      
    
        //Mouse awareness (debug)
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        int tcTouches = Input.touchCount;

        if (tcTouches > 0) {

            for (int i = 0; i < Input.touchCount; i++) {

                Touch touch = Input.GetTouch(i);

                if (Input.GetTouch(i).phase == TouchPhase.Began && !IsPointerOverUIObject()) {

                    var targetPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position); 
                    Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                    m_PlayerTrans.rotation = targetRotation;
                    m_PlayerTrans.position = Vector3.MoveTowards(m_PlayerTrans.position, destinationPos, Speed * Time.deltaTime);

                }
            }
        }
  
    }

    // Update is called once per frame
    //void FixedUpdate() {

       // m_PlayerTrans.position = Vector3.MoveTowards(m_PlayerTrans.position, destinationPos, Speed * Time.deltaTime);

   // }

    //Combat
    public void B_Attack_1() {     //UI Attack button

        StartCoroutine(Attack_1());
        return;

    }
    public IEnumerator Attack_1() {    //Attack coro

        Vector3 relativePos = m_Enemy.transform.position - m_PlayerTrans.transform.position;
        Quaternion lookAtTarget = Quaternion.LookRotation(relativePos);
        m_PlayerTrans.rotation = lookAtTarget;
        m_Anim.SetTrigger("Attack");
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
        m_Anim.SetTrigger("Defend");
        yield break;


    }

    //UI blocking
    private bool IsPointerOverUIObject() {

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;

    }

    //Movement clamp
    public void GetMovementRange(Vector3 mvRange) {

        //move clamp     
        mvRange = Vector3.ClampMagnitude(StageInstance.transform.position, MoveClamp);

        return;

    }
}
