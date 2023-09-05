using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class Patient : GAgent
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            SubGoal s1 = new SubGoal("isWaitingTreatment", 1, true);
            goals.Add(s1, 3);
            SubGoal s2 = new SubGoal("isCured", 1, true);
            goals.Add(s2, 5);
            SubGoal s3 = new SubGoal("isHome", 1, true);
            goals.Add(s3, 4);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
