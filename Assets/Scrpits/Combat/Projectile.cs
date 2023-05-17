using RPG.Attributes;
using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        
        [SerializeField] float speed = 1f;
        [SerializeField] float maxLifetime = 10f;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] GameObject[] destoyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2f;
        [SerializeField] UnityEvent onHit;


        Health Target = null;
        GameObject instigator = null;
        float damage = 0f;

        private void Start()
        {
            this.transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            if (Target == null) return;
            
            if (isHoming && !Target.IsDead()) 
            {
                transform.LookAt(GetAimLocation()); 
            }
            
            
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        public void SetTarget(Health intarget, float damage,GameObject instigator) 
        { 
            
            this.Target= intarget;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifetime);

        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = Target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) 
            {
                return Target.transform.position;
                    
            }
            return Target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != Target) return;
            if (Target.IsDead()) return;
            Target.TakeDamage(damage, instigator);
            Impact();
            foreach (GameObject toDestory in destoyOnHit) 
            { 
            Destroy(toDestory);
            }
            Destroy(gameObject, lifeAfterImpact);

        }

        private void Impact()
        {
            onHit.Invoke();
            speed = 0;
            if(Target == null) return;
            Instantiate(hitEffect, this.transform.position, this.transform.rotation);
        }
    }
}
