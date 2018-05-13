using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxer_AI/Decisions/ActiveState")]

public class ActiveStateDecision : Decision {

    public override System.Boolean Decide(StateControllerRed controller) {

        bool chaseTargetIsActive = controller.chaseTarget.gameObject.activeSelf;
        return chaseTargetIsActive;

    }

}
