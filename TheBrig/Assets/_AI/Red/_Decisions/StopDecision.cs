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

        if (controller.redDirection.magnitude <= 1.0f) {

            return true;

        } else {

            return false;

        }
    }

}
