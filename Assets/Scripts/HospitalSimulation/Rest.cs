using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class Rest : GAction
    {
        public override bool PrePerform()
        {
            return true;
        }

        public override bool PostPerform()
        {
            beliefs.SetState("exhausted", -1);
            return true;
        }
    }
}
