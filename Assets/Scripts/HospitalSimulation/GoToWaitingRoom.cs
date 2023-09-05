using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class GoToWaitingRoom : GAction
    {
        public override bool PrePerform()
        {
            return true;
        }

        public override bool PostPerform()
        {
            GWorld.Instance.GetWorld().ModifyState("WaitingTreatment", 1);
            GWorld.Instance.AddPatient(this.gameObject);
            beliefs.ModifyState("atHospital", 1);
            return true;
        }
    }
}
