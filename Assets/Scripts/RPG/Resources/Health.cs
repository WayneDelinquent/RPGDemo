using System;
using RPG.Core;
using RPG.Progression;
using RPG.Saving;
using UnityEngine;

namespace RPG.Resources
{
	public class Health : MonoBehaviour, ISaveable
	{
		[SerializeField] private float maxHealth = 100f;
		[SerializeField] private float currentHealth = 100f;

		private ActionScheduler _scheduler;

		public bool isDead = false;

		private void Awake()
		{
			_scheduler = GetComponent<ActionScheduler>();
			maxHealth = GetComponent<BaseStats>().GetHealth();
			currentHealth = maxHealth;
		}

		private void LoseHealth(float healthLost)
		{
			currentHealth = Mathf.Max(currentHealth - healthLost, 0f);
			if (!(Math.Abs(currentHealth) < Mathf.Epsilon)) return;

			_scheduler.StopCurrentAction();
			isDead = true;
		}

		public void TakeDamage(float damage)
		{
			LoseHealth(damage);
		}

		public float GetPercentage()
		{
			return Mathf.Round(100 * currentHealth / maxHealth);
		}

		public object CaptureState()
		{
			return new HealthSave
			{
				maxHealth = maxHealth,
				currentHealth = currentHealth,
				isDead = isDead
			};
		}

		public void RestoreState(object state)
		{
			HealthSave save = (HealthSave) state;
			currentHealth = save.currentHealth;
			maxHealth = save.maxHealth;
			isDead = save.isDead;
		}
	}

	[Serializable]
	internal struct HealthSave
	{
		public float maxHealth;
		public float currentHealth;
		public bool isDead;
	}
}
