using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GOAP
{

    public class UpdateWorld : MonoBehaviour
    {
        public TMP_Text states;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void LateUpdate()
        {
            Dictionary<string, int> worldStates = GWorld.Instance.GetWorld().GetStates();
            states.text = "";
            foreach (var ws in worldStates)
            {
                states.text += $"{ws.Key}, {ws.Value}\n";
            }
        }
    }
}