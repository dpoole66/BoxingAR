using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxer_AI/Actions/Idle")]


public class IdleAction : Action {

    public override void Act(StateControllerRed controller) {

        Idle(controller);

    }

    private void Idle(StateControllerRed controller){

        Quaternion targetRotation = Quaternion.LookRotation(controller.m_Blue.transform.position - controller.m_Red.transform.position);
        controller.m_Red.rotation = Quaternion.Slerp(controller.m_Red.rotation, targetRotation, Time.time * 0.07f);

        Debug.Log("Red Idle");

        controller.m_Anim.SetBool("Idle", true);
        controller.m_Anim.SetBool("Move", false);
        controller.m_Anim.SetBool("AttackL", false);
        controller.m_Anim.SetBool("AttackR", false);
        controller.m_Anim.SetBool("Defend", false);
        controller.m_Anim.SetBool("HitBody", false);
        controller.m_Anim.SetBool("HitHead", false);
        controller.m_Anim.SetBool("HitBody", false);

        //debug
        //controller.debugUI.color = Color.blue;

    }
        

}
