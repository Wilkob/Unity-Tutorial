//using RPG.Combat;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable 
    {

        NavMeshAgent navMeshAgent;
        [SerializeField] float maxNavMeshPathLength = 40f;

        [SerializeField] float MaxSpeed = 6f;
        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            navMeshAgent.enabled = !GetComponent<Health>().IsDead();
                
            UpateAnimator();
        }

        public void StartMoveAction(Vector3 destination,float SpeedFraction) 
        {
            GetComponent<ActionScheduler>().Startaction(this);
            
            MoveTo(destination, SpeedFraction);
        }
        public bool CanMoveTo(Vector3 destination) 
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);

            if (!hasPath) return false;

            if (path.status != NavMeshPathStatus.PathComplete) return false;

            if (GetPathLength(path) > maxNavMeshPathLength) return false;

            return true;
        }

        public void MoveTo(Vector3 destination, float SpeedFraction)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = MaxSpeed * Mathf.Clamp01(SpeedFraction);
            navMeshAgent.destination = destination;
            
        }
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }
        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);

            }
            return total;
        }
        private void UpateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localvelocity = transform.InverseTransformDirection(velocity);
            float speed = localvelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)// after awake before start
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = ((SerializableVector3)data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}

// same save done with struct not dicionary
//[System.Serializable]
// struct MoverSaveData
// {
//   public SerializeVerctor3 position;
//   public SerializeVerctor3 rotation;
// }

//        public object CaptureState()
//{
//    MoverSaveData data = new MoverSaveData();
//    data.position = new SerializableVector3(transform.position);
//   data.rotation = new SerializableVector3(transform.eulerAngles);
//   return data;
//}

//public void RestoreState(object state)// after awake before start
//{
//    MoverSaveData data = (MoverSaveData)state;
//    GetComponent<NavMeshAgent>().enabled = false;
//    transform.position = data.position.ToVector();
//    transform.eulerAngles = data.rotation.ToVector();
//    GetComponent<NavMeshAgent>().enabled = true;
//}