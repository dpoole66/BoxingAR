using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedDamage : MonoBehaviour {

    //Animator m_Anim;
    //public Slider health;
    public string Opponent;
    private int count = 0;
    public StateControllerRed controller;


    private void OnTriggerEnter(Collider other) {

        if (controller.m_Anim && (other.gameObject.tag == Opponent)) {

            count = count + 1;
            controller.m_Health.value -= controller.redStats.attackDamage;
            controller.m_Score.text = count.ToString() ;
            controller.hit = true;

        }


        /*if (controller.m_Health.value <= 0)    {

            controller.m_Anim.SetBool("Dead", true);
            //return;

        }      */

    }

}
