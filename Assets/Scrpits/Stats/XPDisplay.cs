using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats { 
public class XPDisplay : MonoBehaviour
{
        Experience XP;

        private void Awake()
        {
            XP = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            GetComponent<Text>().text = string.Format("{0:0}", XP.GetExperiencePoints());
        }
    }
}
