using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class Register : GAction
    {
        public override bool PrePerform()
        {
            return true;
        }

        public override bool PostPerform()
        {
            return true;
        }
    }
}
