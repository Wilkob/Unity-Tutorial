using RPG.Attributes;
using RPG.Core;
using UnityEngine;


namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons / Make new Weapon", order = 0)]

    public class WeaponConfig : ScriptableObject {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] float WeaponRange = 2f;
        [SerializeField] float WeaponDamage = 5f;
        [SerializeField] float WeaponPercentageBonusDamage = 0f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile= null;

        const string weaponName = "Weapon";
        public Weapon Spawn(Transform righthand, Transform lefthand, Animator animator)
        {
            DestroyOldWeapon(righthand, lefthand);

            Weapon weapon = null;

            if (equippedPrefab != null)
            {
                Transform handTransform = GetTransform(righthand, lefthand);
                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)    
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
 
            }
            return weapon;
            
        }

        private void DestroyOldWeapon(Transform righthand, Transform lefthand)
        {
            Transform oldWeapon = righthand.Find(weaponName);
            if (oldWeapon == null) 
            {
                 oldWeapon = lefthand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.gameObject.name = "Destroying";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform righthand, Transform lefthand)
        {
            Transform handTransform;
            if (isRightHanded)
            {
                handTransform = righthand.transform;
            }
            else
            {
                handTransform = lefthand.transform;
            }

            return handTransform;
        }

        public bool HasProjectile() 
        { 
            return projectile != null;
        }

        public void LaunchProjectile (Transform righthand, Transform lefthand, Health target, GameObject instigator,float CalculatedDamage) 
        { 
            Projectile projectileInstance = Instantiate(projectile, GetTransform(righthand, lefthand).position,Quaternion.identity);
            projectileInstance.SetTarget(target, CalculatedDamage, instigator);
        }

        public float GetRange()
        {
            return WeaponRange;
        }
        public float GetDamage()
        {
            return WeaponDamage;
        }
        public float GetPercentageBouns() 
        {
            return WeaponPercentageBonusDamage;
        }
        
    }
}