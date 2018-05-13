using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxer_AI/Decisions/Stop")]

public class StopDecision : Decision {

    public override bool Decide(StateControllerRed controller) {

        bool targetVisible = Look(controller);
        return targetVisible;

    }

    private bool Look(StateControllerRed controller) {

        RaycastHit hit;
        Debug.DrawRay(controller.eyesRed.position, controller.eyesRed.forward.normalized * controller.redStats.lookRange, Color.red);

        if (Physics.SphereCast(controller.eyesRed.position, controller.redStats.lookSphereCastRadius, controller.eyesRed.forward, out hit, controller.redStats.stopRange)
           && hit.collider.CompareTag("Blue")) {
            controller.m_Rigid.velocity = new Vector3(0,0,0);
            return true;
        } else {
            return false;
        }

    }

}
