using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxer_AI/Decisions/Dead")]

public class DeadDecision : Decision {

    public override bool Decide(StateControllerRed controller){

            bool dead = Dead(controller);
            return dead;

    }
private bool Dead(StateControllerRed controller){

        if (controller.m_Health.value <= 0.0f) {

            return true;

        } else {

            return false;

        }

    }
	
}

