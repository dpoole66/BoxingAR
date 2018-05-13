using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxer_AI/Actions/Damage")]

public class DamageAction : Action {

    public override void Act(StateControllerRed controller) {

        Damage(controller);

    }
    private void Damage(StateControllerRed controller) {
        
        if(controller.hit == true){
                Debug.Log("Damage");

                controller.m_Anim.SetBool("Idle", false);
                controller.m_Anim.SetBool("Move", false);
                controller.m_Anim.SetBool("AttackL", false);
                controller.m_Anim.SetBool("AttackR", false);
                controller.m_Anim.SetBool("Defend", false);
                controller.m_Anim.SetBool("HitBody", false);
                controller.m_Anim.SetBool("HitHead", true);
                controller.m_Anim.SetBool("HitBody", false);

            controller.hit = false;

        }


    }
}
