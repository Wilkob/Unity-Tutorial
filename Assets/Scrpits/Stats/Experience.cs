using RPG.Saving;
using System;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        public event Action onExperienceGained;
        public void gainXP(float xp) 
        {
            experiencePoints += xp;
            onExperienceGained();
        }
        public float GetExperiencePoints() { return experiencePoints; }
       
        public object CaptureState()
        {
            return experiencePoints;
        }


        public void RestoreState(object state)
        {
           
            experiencePoints =  (float)state;
        }
    }
}
