using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
	[RequireComponent(typeof(Health))]
	public class CombatTarget : MonoBehaviour
	{
		private Health _health;
		private Fighter _fighter;
		private void Awake()
		{
			_health = GetComponent<Health>();
			_fighter = GetComponent<Fighter>();
		}

		public void TakeDamage(float damage)
		{
			if (_health == null) return;
			_health.TakeDamage(damage);
			if (IsDead()) _fighter.Die();
		}

		public bool CanBeAttacked()
		{
			return !IsDead();
		}

		public bool IsDead()
		{
			return _health.isDead;
		}

		public Health GetHealth()
		{
			return _health;
		}
	}
}
