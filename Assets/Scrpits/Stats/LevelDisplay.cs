using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats { 
public class LevelDisplay : MonoBehaviour
{
        BaseStats XP;

        private void Awake()
        {
            XP = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            GetComponent<Text>().text = string.Format("{0:0}", XP.GetLevel());
        }
    }
}
