using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxer_AI/Actions/Idle")]


public class IdleAction : Action {

    public override void Act(StateControllerRed controller) {

        Idle(controller);

    }

    private void Idle(StateControllerRed controller){

        if (controller.m_Health.value <= 0.0f) {

            return;

        }

        //Rotate
        controller.redRotation = Quaternion.Slerp(controller.m_Red.transform.rotation, 
        Quaternion.LookRotation(controller.redDirection), controller.redStats.rotateSpeed);

        controller.m_Anim.SetBool("Idle", true);
        controller.m_Anim.SetBool("Move", false);
        controller.m_Anim.SetBool("HitHead", false);
        controller.m_Anim.SetBool("AttackL", false);
        controller.m_Anim.SetBool("AttackR", false);
        controller.m_Anim.SetBool("Dead", false);

        Debug.Log("Red Idle");

    }
        

}
