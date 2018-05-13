using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxer_AI/Decisions/Attack")]

public class AttackDecision : Decision {

    public override bool Decide(StateControllerRed controller) {

        bool targetVisible = Look(controller);
        return targetVisible;

    }

    private bool Look(StateControllerRed controller) {

        RaycastHit hit;
        Debug.DrawRay(controller.eyesRed.position, controller.eyesRed.forward.normalized * controller.redStats.lookRange, Color.red);

        if (Physics.SphereCast(controller.eyesRed.position, controller.redStats.lookSphereCastRadius, controller.eyesRed.forward, out hit, controller.redStats.attackRange)
           && hit.collider.CompareTag("Blue") ){
           // controller.chaseTarget = hit.transform;
            return true;
        } else {
            return false;
        }

    }

}