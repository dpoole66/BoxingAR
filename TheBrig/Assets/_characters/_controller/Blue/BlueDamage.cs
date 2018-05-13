using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueDamage : MonoBehaviour {

    Animator m_Anim;
    public Slider health;
    public string Opponent;

    private void Awake() {

        m_Anim = GetComponent<Animator>();

    }


    private void OnTriggerEnter(Collider other) {

        if (m_Anim && (other.gameObject.tag == Opponent)) {

            health.value -= 20;      
            m_Anim.SetBool("Idle", false);
            m_Anim.SetBool("HitHead", true);
            //return;


        }


        if (health.value <= 0) {

            m_Anim.SetBool("Dead", true);
            //return;

        }

    }

}
