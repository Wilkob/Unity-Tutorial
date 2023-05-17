using RPG.Attributes;
using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Combat { 
public class EnemyHealthDisplay : MonoBehaviour
{
        Fighter Playerfighter;

        private void Awake()
        {
            Playerfighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if (Playerfighter.GetTarget() == null) 
            {
                GetComponent<Text>().text = "N/A";
                return;
            }
            GetComponent<Text>().text = string.Format("{0:0}/{1:0}", Playerfighter.GetTarget().GetHP(), Playerfighter.GetTarget().GetMaxHP());
        }
    }
}
