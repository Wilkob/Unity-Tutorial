using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using GameDevTV.Utils;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour,IAction, ISaveable, IModifierProvider
    {
        Health Target;
        float TimeSinceLastAttack = Mathf.Infinity;
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;

        [SerializeField] float TimeBetweenAttacks = 1f;
        [SerializeField] Transform leftHandTransform = null; 
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        
        private void Awake()
        {

            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
            //EquipWeapon(defaultWeapon);
            
        }
        private void Start()
        {
            //currentWeaponConfig.ForceInit();
            currentWeapon.ForceInit();
        }

        private Weapon SetupDefaultWeapon()
        {
            
            return AttachWeapon(defaultWeapon);
        }

        void Update()
        {
            TimeSinceLastAttack += Time.deltaTime;
            if (Target == null || Target.IsDead()) return;
                
            if( GetInrange(Target.transform))
            {
                GetComponent<Mover>().MoveTo(Target.transform.position,1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }
        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            
            return weapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
        }

        private void AttackBehaviour()
        {
            if (TimeSinceLastAttack >= TimeBetweenAttacks)
            {
                TriggerAttack();
                TimeSinceLastAttack = 0;
            }

        }

        private void TriggerAttack()
        {
            //this will trigger the hit event
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        private bool GetInrange(Transform targetTranform)
        {
            return Vector3.Distance(targetTranform.transform.position, transform.position) > currentWeaponConfig.GetRange();
        }

        public void Attack (GameObject CombatTarget)
        {
            GetComponent<ActionScheduler>().Startaction(this);
            Target = CombatTarget.GetComponent<Health>();
           
        }
        public bool CanAttack(GameObject CombatTarget) 
        {
            if (CombatTarget == null) { return false; }
            if (!GetComponent<Mover>().CanMoveTo(CombatTarget.transform.position) && !GetInrange(CombatTarget.transform) )
            { 
                return false; 
            }
            Health targetToTest = CombatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }
        public void Cancel()
        {
            Target = null;
            GetComponent<Mover>().Cancel();
            StopAttack();
        }

        public Health GetTarget() 
        {
            return Target;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<Animator>().ResetTrigger("attack");
        }
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
                
            {
            yield return currentWeaponConfig.GetDamage();
            }
        }
        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)

            {
                yield return currentWeaponConfig.GetPercentageBouns();
            }
        }
        //animation event 
        void Shoot() { Hit(); }
        void Hit()
        {

            if (Target == null) { return; }
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeapon.value != null) 
            {
                currentWeapon.value.OnHit();
            }
                
            if (currentWeaponConfig.HasProjectile()) 
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, Target, gameObject,damage);
            }
            else 
            {
                
                Target.TakeDamage(damage, gameObject); 
            }
            
        }

        public object CaptureState()
        {
            print(currentWeaponConfig.name);
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
           string weaponName = (string) state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }


    }
}