using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using System;
using RPG.Attributes;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {


        public event EventHandler onFight; 


        [System.Serializable]
        struct CursorMapping 
        {
           public  CursorType Type;
           public Texture2D texture;
           public Vector2 hotspot;
        }
        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDis = 1f;
        [SerializeField] float RaycastRadius = 1f;

        void Update()
        {
            if (InteractWithUI()) return;

            if (GetComponent<Health>().IsDead())
            {
                SetCursor(CursorType.None); 
                return;
            }
            if (IneractWithComponent()) return;

            if (InteractWithMovement()) return;
            SetCursor(CursorType.None);

        }


        private bool InteractWithUI()
        {
            
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }
        private bool IneractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                    if (raycastable.HandleRaycast(this)) 
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hashit = RaycastNavMesh(out target);
         
            if (hashit)
            {

                if (!GetComponent<Mover>().CanMoveTo(target)) return false;
                if (Input.GetMouseButton(0))
            
                {
                    GetComponent<Mover>().StartMoveAction(target,1f);

                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;

        }
        private bool RaycastNavMesh(out Vector3 target) 
        {
            target = new Vector3();
            RaycastHit hit;
            bool hashit = Physics.Raycast(GetRay(), out hit);
            if (!hashit) return false;

            NavMeshHit nvaMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out nvaMeshHit, maxNavMeshProjectionDis, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;
            
            
            target = nvaMeshHit.position;



            return true;
        }



        private void SetCursor(CursorType Type)
        {
            CursorMapping maping = GetCursorMapping(Type);
            Cursor.SetCursor(maping.texture,
                             maping.hotspot,
                             CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type) 
        { 
            foreach (CursorMapping mapping in cursorMappings) 
            { 
            if (mapping.Type == type) { return mapping; }
                    
            }
            return cursorMappings[0];
        }

        RaycastHit[] RaycastAllSorted() 
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetRay(),RaycastRadius);
            float[] distances= new float[hits.Length];
            for (int i = 0; i < hits.Length; i++) 
            {
                distances[i] = hits[i].distance;

            }
            Array.Sort(distances,hits);
            return hits;
        }       
        private static Ray GetRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
