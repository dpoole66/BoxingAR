using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class StateControllerRed : MonoBehaviour {

    public StateRed currentState;
    public Stats redStats;
    public Transform eyesRed;
    public StateRed remainState;
    public bool hit = false;

    //debug
    [HideInInspector] public Image debugUI;

    [HideInInspector] public Transform m_Red;
    [HideInInspector] public Transform m_Blue;
    [HideInInspector] public Rigidbody m_Rigid;
    [HideInInspector] public Animator m_Anim;
    [HideInInspector] public RedDamage m_Damage;
    [HideInInspector] public Slider m_Health;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public float stateTimeElapsed;

    private void Awake() {

        m_Blue = GameObject.FindGameObjectWithTag("Blue").transform;
        m_Health = GetComponentInChildren<Slider>();
        m_Red = GetComponent<Transform>();
        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();

    }

    void Update() {

        currentState.UpdateState(this);

    }

    void OnDrawGizmos() {
        if (currentState != null && eyesRed != null) {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(eyesRed.position, redStats.lookSphereCastRadius);
        }
    }

    public void TransitionToState(StateRed nextState) {
        if (nextState != remainState) {
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration) {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState() {
        stateTimeElapsed = 0;
    }
}