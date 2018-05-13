using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxer_AI/Actions/Chase")]

public class ChaseAction : Action {
    public override void Act(StateControllerRed controller) {

        Chase(controller);

    }
    private void Chase(StateControllerRed controller) {

        Debug.Log("Approach");

        controller.m_Anim.SetBool("Idle", false);
        controller.m_Anim.SetBool("Move", true);
        controller.m_Anim.SetBool("AttackL", false);
        controller.m_Anim.SetBool("AttackR", false);
        controller.m_Anim.SetBool("Defend", false);
        controller.m_Anim.SetBool("HitBody", false);
        controller.m_Anim.SetBool("HitHead", false);
        controller.m_Anim.SetBool("HitBody", false);

        //Rotate
        Vector3 targetPoint = controller.chaseTarget.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - controller.m_Red.transform.position);
        controller.m_Red.rotation = Quaternion.Slerp(controller.m_Red.rotation, targetRotation, Time.time * controller.redStats.rotateSpeed);
        //Move     
        controller.m_Red.position = Vector3.MoveTowards(controller.m_Red.position, targetPoint, controller.redStats.moveSpeed * Time.deltaTime);

    }

}
