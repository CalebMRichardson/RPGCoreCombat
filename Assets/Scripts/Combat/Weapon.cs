using UnityEngine;
using RPG.Resources;

namespace RPG.Combat {
    
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject {
    
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponDamage = 5f; 
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string WEAPON_NAME = "Weapon";

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator) {

            DestroyOldWeapon(rightHand, leftHand);
            
            if (equippedPrefab != null) {
                Transform handTransform = GetTransform(rightHand, leftHand);
                GameObject weapon = Instantiate(equippedPrefab, handTransform);
                weapon.name = WEAPON_NAME;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride != null) {
                
                animator.runtimeAnimatorController = animatorOverride; 
                
            } else if (overrideController != null) {
            
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand) {
            
            Transform oldWeapon = rightHand.Find(WEAPON_NAME);
            
            if (oldWeapon == null) {
                oldWeapon = leftHand.Find(WEAPON_NAME);
            }

            if (oldWeapon == null) return;

            // Rename oldWeapon before naming new weapon
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand) {

            Transform handTransform;

            if (isRightHanded) {
                
                handTransform = rightHand;
            } else {
                
                handTransform = leftHand;
            }

            return handTransform;
        }

        public bool HasProjectile() {
         
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator) {
            
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);

            projectileInstance.SetTarget(target, instigator, weaponDamage);
        }

        public float GetRange() {
         
            return weaponRange;
        }

        public float GetDamage() {
            
            return weaponDamage; 
        }

    }
}