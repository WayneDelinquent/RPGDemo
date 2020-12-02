using System;
using System.Security.Cryptography;
using UnityEngine;

namespace RPG.Combat
{
	[Serializable]
	[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
	public class Weapon : ScriptableObject
	{
		[SerializeField] private GameObject weaponPrefab;
		[SerializeField] private AnimatorOverrideController animatorOverride;
		[SerializeField] private float weaponRange = 2f;
		[SerializeField] private float attackDamage = 35f;
		[SerializeField] private bool isRightHanded = true;
		[SerializeField] private Projectile _projectile;

		private const string WeaponName = "Weapon";

		public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
		{
			DestroyOldWeapon(rightHand, leftHand);
			if (weaponPrefab != null)
			{
				GameObject weapon = Instantiate(weaponPrefab, isRightHanded ? rightHand : leftHand);
				weapon.name = WeaponName;
			}
			if (animatorOverride != null)
			{
				animator.runtimeAnimatorController = animatorOverride;
			}
			else
			{
				AnimatorOverrideController overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
				if (overrideController != null)
				{
					animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
				}
			}
		}

		private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
		{
			Transform oldWeapon = rightHand.Find(WeaponName);
			if (oldWeapon == null)
			{
				oldWeapon = leftHand.Find(WeaponName);
			}
			if (oldWeapon == null) return;
			oldWeapon.name = "Destroying";
			Destroy(oldWeapon.gameObject);
		}

		public float GetWeaponRange()
		{
			return weaponRange;
		}

		public float GetWeaponDamage()
		{
			return attackDamage;
		}

		public bool HasProjectile()
		{
			return _projectile != null;
		}

		public void LaunchProjectile(Transform rightHand, Transform leftHand, CombatTarget target)
		{
			Projectile projectile = Instantiate(_projectile, (isRightHanded ? rightHand : leftHand).position, Quaternion.identity);
			projectile.SetTarget(target, attackDamage, true);
		}
	}
}
