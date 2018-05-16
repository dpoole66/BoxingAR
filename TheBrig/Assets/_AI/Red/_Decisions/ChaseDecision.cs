using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxer_AI/Decisions/Chase")]

public class ChaseDecision : Decision {

    public override bool Decide(StateControllerRed controller) {

        bool targetFound = Chase(controller);
        return targetFound;

    }

    private bool Chase(StateControllerRed controller){

        if(controller.chase == true) {
           
            return true;
         
        }     else {

            return false;

        }              
    }

}
