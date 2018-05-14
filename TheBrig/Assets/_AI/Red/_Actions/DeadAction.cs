using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxer_AI/Actions/Dead")]

public class DeadAction : Action {

    public override void Act(StateControllerRed controller) {

        Dead(controller);

    }

    private void Dead(StateControllerRed controller){

        controller.m_Anim.SetBool("Idle", false);
        controller.m_Anim.SetBool("Move", false);
        controller.m_Anim.SetBool("HitHead", false);
        controller.m_Anim.SetBool("AttackL", false);
        controller.m_Anim.SetBool("AttackR", false);
        controller.m_Anim.SetBool("Dead", true);

    }

}
