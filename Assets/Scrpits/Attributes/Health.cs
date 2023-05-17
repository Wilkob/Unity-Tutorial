using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float RegenerateHealth = 70;
        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] UnityEvent onDie;
        bool isDead = false;
        //public GameObject Lastdamge;//Not part of tutroial 
        //float TimeSinceLastDamge = Mathf.Infinity;//Not part of tutroial 
        LazyValue <float> HealthPoints;

        private void OnEnable()
        {

                GetComponent<BaseStats>().onLevelUp += Levelup;

        }
        private void OnDisable()
        {

                GetComponent<BaseStats>().onLevelUp -= Levelup;

        }
        private void Levelup()
        {
            float RegenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (RegenerateHealth / 100);
            HealthPoints.value = Mathf.Max(HealthPoints.value, RegenHealthPoints);
            
        }

        private void Awake()
        {
            HealthPoints = new LazyValue<float>(GetInitalHealth);
        }

        private void Start()
        {
            HealthPoints.ForceInit();
        }
        
        private float GetInitalHealth() 
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        
        //void Update()//Not part of tutroial 
        //{//Not part of tutroial 
        //    TimeSinceLastDamge += Time.deltaTime;//Not part of tutroial 
        //    if (TimeSinceLastDamge > 1) { Lastdamge = null; }//Not part of tutroial 
        //}//Not part of tutroial 
        public bool IsDead()
        {
            return isDead;
        }
        public void TakeDamage(float Damage,GameObject instigator)//Not part of tutroial 
        {
            //TimeSinceLastDamge = 0;//Not part of tutroial 
            //Lastdamge = instigator;//Not part of tutroial 

            HealthPoints.value = Mathf.Max(HealthPoints.value - Damage, 0);

            if (HealthPoints.value == 0) 
            {
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
            else 
            { 
                takeDamage.Invoke(Damage);            
            }


        }
        public void Heal(float Heal) 
        {
            HealthPoints.value = Mathf.Min(HealthPoints.value + Heal, GetMaxHP());
        }
        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;         
            experience.gainXP(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
                    
        }

        public float GetHP()
        {

            return HealthPoints.value;
        }
        public float GetMaxHP() 
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage ()
            {

            return 100 * (HealthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health));
        }
        public float GetFraction()
        {

            return HealthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        private void Die()
        {
            if (isDead) return;
            
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
             isDead = true;
            
        }


        //saving
        public object CaptureState()
        {
            return HealthPoints.value;
        }

        public void RestoreState(object state)
        {
            float HP = (float)state;
            HealthPoints.value = HP;
            if (HealthPoints.value == 0) { Die(); }
        }
    }
}