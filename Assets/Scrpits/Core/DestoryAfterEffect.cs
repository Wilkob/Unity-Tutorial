using UnityEngine;
namespace RPG.Core
{

    public class DestoryAfterEffect : MonoBehaviour

{

        [SerializeField] GameObject targetToDestory = null;
    // Update is called once per frame
    
        void Update()
    
        {
        
            if (!GetComponent<ParticleSystem>().IsAlive()) 
        
            {
           
                if (targetToDestory != null) 
                {
                    Destroy(targetToDestory);
                }
                else {Destroy(gameObject);  }
                
            
            }
        }
    }
}