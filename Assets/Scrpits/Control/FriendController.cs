//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using RPG.Combat;
//using RPG.Movement;
//using System;
//using RPG.Attributes;

//namespace RPG.Control
//{
//    public class FriendController : MonoBehaviour
//    {
//        GameObject player;
//        Fighter fighter;
//        [SerializeField] float chaseDistance = 5f;
//        [SerializeField] float CloseDistance = 1f;
//        public static List<FriendController> FriendList = new List<FriendController>();
//        public static List<FriendController> GetFriendsList() { return FriendList; }
//        // Start is called before the first frame update

//        private void Awake()
//        {
//            FriendList.Add(this);
//            GameObject.FindWithTag("Player").GetComponent<PlayerController>().onFight += Onfight_attack;
//            GetComponent<Animator>().Update(5);
//            GetComponent<Animator>().enabled = false;
//            GetComponent<Animator>().enabled = true;
//        }
//        void Start()
//        {
//            fighter = GetComponent<Fighter>();
//            player = GameObject.FindWithTag("Player");
//        }

//        // Update is called once per frame
//        void Update()
//        {
//            if (GetComponent<Health>().IsDead()) return;
//            if (GetComponent<Health>().Lastdamge != null)
//            {
//                Attack(GetComponent<Health>().Lastdamge);
//                return;
//            }

//            if (RangetoPlayer() > chaseDistance) { GetComponent<Mover>().MoveTo(player.transform.position,1f); }
//            if (RangetoPlayer() < CloseDistance) { GetComponent<Mover>().Cancel(); }

//        }

//        private void Attack(GameObject Target)
//        {
//            if (fighter.CanAttack(Target))
//            {
//                print("I " + gameObject.name + " attack " + Target.name);
//                fighter.Attack(Target);
//            }
//            else
//            {
//                GetComponent<Fighter>().Cancel();
//            }
//        }
//        private void Onfight_attack(object sender, EventArgs e)
//        {
//            print(" attack event");
//            Attack(closestCombatTarget());
//        }
//        private GameObject closestCombatTarget() //Not part of tutroial 
//        {//Not part of tutroial 

//            GameObject Closest = null;//Not part of tutroial 
//            float disB = Mathf.Infinity;//Not part of tutroial 

//            foreach (CombatTarget foe in CombatTarget.GetCombatTargetList())//Not part of tutroial 
//            {
//                if (!fighter.CanAttack(foe.gameObject)) { continue; }
//                float DisF = Vector3.Distance(foe.transform.position, transform.position);//Not part of tutroial 
//                if (disB > DisF) //Not part of tutroial 
//                {
//                    Closest = foe.gameObject;//Not part of tutroial 
//                    disB = DisF;//Not part of tutroial 
//                }//Not part of tutroial 


//            }//Not part of tutroial 
//            return Closest; //Not part of tutroial 
//        }//Not part of tutroial 
//        private float RangetoPlayer()
//        {

//            float distanceToplayer = Vector3.Distance(player.transform.position, transform.position);
//            return distanceToplayer;
//        }
//        void OnDestroy()
//        {
//            FriendList.Remove(this);
//        }
//    }
//}
