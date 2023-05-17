using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using GameDevTV.Utils;
using RPG.Attributes;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        
        [SerializeField] float shoutDistance = 5f;
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float supicionTime = 5f;
        [SerializeField] float aggroCooldownTime = 5f; 
        [SerializeField] float waypointDwellTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [Range(0,1)]
        [SerializeField] float PatrolSpeedFraction = 0.2f;
        Fighter fighter;
        GameObject player;
        //GameObject close;//Not part of tutroial 
        LazyValue<Vector3> guardPosition;
        float TimeSinceseenplayer = Mathf.Infinity;
        float TimeSinceAggrevated = Mathf.Infinity;
        float TimeonSinceArriedAtWaypoint = 0;
        int CurrentWaypointIndex = 0;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }
        
        private Vector3 GetGuardPosition() 
        {
            return transform.position;
        }
        private void Start()
        {
            guardPosition.ForceInit();
        }
        private void Update()
        {
            if (GetComponent<Health>().IsDead()) return;
            //close = closestFriend();//Not part of tutroial changes made to allow AI to target an NPC Player Ally
            // if (close != null && InAttackRange(close) && fighter.CanAttack(close))//Not part of tutroial 
            // {
            //     TimeSinceseenplayer = 0;
            //    fighter.Attack(close.gameObject);
            // }else

            if (IsAggrevated(player) && fighter.CanAttack(player))
            {

                AttackBehaviour();
            }
            else if (TimeSinceseenplayer < supicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();

            }
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            TimeSinceseenplayer += Time.deltaTime;
            TimeSinceAggrevated += Time.deltaTime;
        }

        public void Aggrevate() 
        {
            TimeSinceAggrevated = 0;
        }
        private void PatrolBehaviour()
        {
            Vector3 nextPositiion = guardPosition.value;
            if (patrolPath != null)
            {
                
                if (AtWaypoint())
                {
                    TimeonSinceArriedAtWaypoint += Time.deltaTime;

                    if (TimeonSinceArriedAtWaypoint > waypointDwellTime)
                    {
                        CycleWaypoint();
                        TimeonSinceArriedAtWaypoint = 0;
                    }
                    
                }
                nextPositiion = GetCurrentWaypoint();
            }
            GetComponent<Mover>().StartMoveAction(nextPositiion, PatrolSpeedFraction);

        }

        private bool AtWaypoint()
        {
            {

                float distanceToWaypoint = Vector3.Distance(GetCurrentWaypoint(), transform.position);

                return distanceToWaypoint < waypointTolerance;
            }
        }

        private void CycleWaypoint()
        {
            CurrentWaypointIndex = patrolPath.GetNextIndex(CurrentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(CurrentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            TimeSinceseenplayer = 0;
            fighter.Attack(player.gameObject);
            AggrevateNearblyEnemies();

        }

        private void AggrevateNearblyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits) 
            {
                AIController hitAI = hit.collider.GetComponent<AIController>();
                if (hitAI == null) { continue; }

                hitAI.Aggrevate();

            }
        }

        private bool IsAggrevated(GameObject target)//Not part of tutroial changed to be generic not focused on player
        {

            float distanceToplayer = Vector3.Distance(target.transform.position, transform.position);

            return distanceToplayer < chaseDistance || TimeSinceAggrevated < aggroCooldownTime;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

       // private GameObject closestFriend() //Not part of tutroial finds the closest Players ally NPC
       // {//Not part of tutroial 

         //   GameObject Closest = null;//Not part of tutroial 
         //   float disB = Mathf.Infinity;//Not part of tutroial 

           // foreach (FriendController friend in FriendController.GetFriendsList())//Not part of tutroial 
           // {
              //  float DisF = Vector3.Distance(friend.transform.position, transform.position);//Not part of tutroial 
              //  if (disB > DisF) //Not part of tutroial 
              //  { 
              //      Closest = friend.gameObject;//Not part of tutroial 
               //     disB = DisF;//Not part of tutroial 
            //    }//Not part of tutroial 


          //  }//Not part of tutroial 
          //  return Closest; //Not part of tutroial 
        //}//Not part of tutroial 

    }
}