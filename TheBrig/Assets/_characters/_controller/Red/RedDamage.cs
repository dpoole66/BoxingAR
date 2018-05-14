using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedDamage : MonoBehaviour {

    Animator m_Anim;
    public Slider health;
    public string Opponent;
    public StateControllerRed controller;


    private void OnTriggerEnter(Collider other) {

        if (controller.m_Anim && (other.gameObject.tag == Opponent)) {

            controller.m_Health.value -= controller.redStats.attackDamage;
            controller.hit = true;

        }


        if (health.value <= 0)    {

            controller.m_Anim.SetBool("Dead", true);
            //return;

        }

    }

}
