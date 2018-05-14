using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxer_AI/Decisions/Stop")]

public class StopDecision : Decision {

    public override bool Decide(StateControllerRed controller) {

        bool targetNear = Stop(controller);
        return targetNear;

    }

    private bool Stop(StateControllerRed controller) {

        if (controller.redDirection.magnitude <= controller.redStats.stopRange) {

            controller.m_Anim.SetBool("Idle", true);
            controller.m_Anim.SetBool("Move", false);
            controller.m_Anim.SetBool("AttackL", false);
            controller.m_Anim.SetBool("Dead", false);

            return true;

        } else {

            return false;

        }
    }

}
