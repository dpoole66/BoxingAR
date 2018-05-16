using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxer_AI/Decisions/Attack")]

public class AttackDecision : Decision {

    public override bool Decide(StateControllerRed controller) {

        bool targetVisible = Attack(controller);
        return targetVisible;

    }

    private bool Attack(StateControllerRed controller) {

        if (controller.redDirection.magnitude <= 0.8f) {
    
            return true;

        } else {

            return false;

        }
    }

}