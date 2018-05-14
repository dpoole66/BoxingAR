using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxer_AI/Actions/Chase")]

public class ChaseAction : Action {
    public override void Act(StateControllerRed controller) {

        Chase(controller);

    }
    private void Chase(StateControllerRed controller) {

        if(controller.m_Health.value <= 0.0f){

                return;

        }

        Debug.Log("Red Chase");
        controller.m_Anim.SetBool("Idle", false);
        controller.m_Anim.SetBool("Move", true);
        controller.m_Anim.SetBool("AttackL", false);   

        //Rotate
        controller.m_Red.transform.rotation = Quaternion.Slerp(controller.m_Red.transform.rotation,
        Quaternion.LookRotation(controller.redDirection), controller.redStats.rotateSpeed);

        //Move       
        controller.m_Red.transform.position = Vector3.Lerp(controller.m_Red.transform.position, 
        controller.m_Blue.transform.position, controller.redStats.moveSpeed * Time.deltaTime);


    }

}
