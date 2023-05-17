using RPG.Attributes;
using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]



    public class CombatTarget : MonoBehaviour, IRaycastable
        
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        //public static List<CombatTarget> CombatTargetList = new List<CombatTarget>();//Not part of tutroial 
        //public static List<CombatTarget> GetCombatTargetList() { return CombatTargetList; }//Not part of tutroial 

        public bool HandleRaycast(PlayerController callingController)
        {
                if (!callingController.GetComponent<Fighter>().CanAttack(gameObject)) 
            {
                return false;
            }

                if (Input.GetMouseButton(0))
                {
                callingController.GetComponent<Fighter>().Attack(gameObject);
                // if (onFight != null) {onFight(this, EventArgs.Empty); }//Not part of tutroial 
            }
            return true;
            }
            
        

        //private void Awake()//Not part of tutroial 
        //{//Not part of tutroial 
          //  CombatTargetList.Add(this);//Not part of tutroial 
        //}//Not part of tutroial 
    }
    
}
