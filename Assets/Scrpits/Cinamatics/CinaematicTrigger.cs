using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics 

{ 
    public class CinaematicTrigger : MonoBehaviour
    {
        bool AlreadyTriggered = false;
        private void OnTriggerEnter(Collider other)
        {

            if (!AlreadyTriggered && other.gameObject.tag == "Player")
            {
                AlreadyTriggered = true;
                GetComponent<PlayableDirector>().Play();

            }
            
        }
    }
}
