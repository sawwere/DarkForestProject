using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class Nurse : GAgent
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            SubGoal s1 = new SubGoal("treatPatient", 1, false);
            goals.Add(s1, 3);
            SubGoal s2 = new SubGoal("rested", 1, false);
            goals.Add(s2, 1);

            Invoke("GetTired", Random.Range(10, 15));
        }

        void GetTired()
        {
            beliefs.ModifyState("exhausted", 1);
            Debug.Log(beliefs.HasState("exhausted"));
            Invoke("GetTired", Random.Range(5, 10));
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
