using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Attributes;
using RPG.Movement;

namespace RPG.Combat {
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig Weapon = null;
        [SerializeField] float respawnTime = 5;
        [SerializeField] float healthtoHeal = 0;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject)
        {
            if (Weapon != null) 
            { 
            subject.GetComponent<Fighter>().EquipWeapon(Weapon);
            }
            if (healthtoHeal > 0)
            {
                subject.GetComponent<Health>().Heal(healthtoHeal);

            }
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds) 
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {

            GetComponent<Collider>().enabled = shouldShow;
            transform.GetChild(0).gameObject.SetActive(shouldShow);
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }

        }
        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButton(0)) 
            {
                //Pickup(callingController.gameObject);
                if (!callingController.GetComponent<Mover>().CanMoveTo(transform.position)) { return false; }
                callingController.GetComponent<Mover>().MoveTo(transform.position, 1f);
            }
            return true;
        }


    }
}