using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP
{
    public abstract class GAction : MonoBehaviour
    {
        public string actionName = "Action";
        public float cost = 1.0f;
        public GameObject target;
        public string targetTag;
        public float duration = 0.0f;
        public WorldState[] preConditions; // for visibility in the Inspector
        public WorldState[] afterEffects;
        public NavMeshAgent agent;

        public Dictionary<string, int> preconditions;
        public Dictionary<string, int> effects;

        public WorldStates agentBeliefs;

        //REDO
        public GInventory inventory;
        public WorldStates beliefs;

        public bool running = false;

        public GAction()
        {

            preconditions = new Dictionary<string, int>();
            effects = new Dictionary<string, int>();
        }

        public void Awake()
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
            if (preConditions != null)
            {
                foreach (WorldState ws in preConditions)
                {
                    preconditions.Add(ws.key, ws.value);
                }
            }
            if (afterEffects != null)
            {
                foreach (WorldState ws in afterEffects)
                {
                    effects.Add(ws.key, ws.value);
                }
            }
            inventory = this.GetComponent<GAgent>().inventory;
            this.beliefs = GetComponent<GAgent>().beliefs;
        }

        public bool IsAchievable()
        {
            return true;
        }

        public bool IsAchievableGiven(Dictionary<string, int> conditions)
        {
            foreach (var precondition in preconditions)
            {
                if (!conditions.ContainsKey(precondition.Key))
                    return false;
            }
            return true;
        }

        public abstract bool PrePerform();
        public abstract bool PostPerform();
    }
}