using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxer_AI/Actions/Attack")]

public class AttackAction : Action {

    public override void Act(StateControllerRed controller) {

        Attack(controller);

    }

    private void Attack(StateControllerRed controller) {

            Vector3 targetPoint = controller.chaseTarget.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - controller.m_Red.transform.position);
            controller.m_Red.rotation = Quaternion.Slerp(controller.m_Red.rotation, targetRotation, Time.time * controller.redStats.rotateSpeed);
            controller.m_Rigid.velocity = new Vector3(0, 0, 0);
            //controller.m_Red.position = controller.m_Rigid.position;

            Debug.Log("AttackL");

                controller.m_Anim.SetBool("Idle", false);
                controller.m_Anim.SetBool("Move", false);
                controller.m_Anim.SetBool("AttackL", true);
                controller.m_Anim.SetBool("AttackR", false);
                controller.m_Anim.SetBool("Defend", false);
                controller.m_Anim.SetBool("HitBody", false);
                controller.m_Anim.SetBool("HitHead", false);
                controller.m_Anim.SetBool("HitBody", false);

    }
}
