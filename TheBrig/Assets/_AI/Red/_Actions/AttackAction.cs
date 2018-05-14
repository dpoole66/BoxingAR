using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxer_AI/Actions/Attack")]

public class AttackAction : Action {

    public override void Act(StateControllerRed controller) {

        Attack(controller);

    }

    private void Attack(StateControllerRed controller) {

        if (controller.m_Health.value <= 0.0f) {

            return;

        }


        //Rotate
        controller.m_Red.transform.rotation = Quaternion.Slerp(controller.m_Red.transform.rotation,
        Quaternion.LookRotation(controller.redDirection), controller.redStats.rotateSpeed);

        Debug.Log("Red AttackL");
        controller.m_Anim.SetBool("Idle", false);
        controller.m_Anim.SetBool("Move", false);
        controller.m_Anim.SetBool("AttackL", true);

    }
}
