using RPG.Core;
using RPG.Core.Interfaces;
using RPG.Movement;
using RPG.Resources;
using RPG.Saving;
using UnityEngine;

namespace RPG.Combat
{
	public class Fighter : MonoBehaviour, ISchedulableAction, ISaveable
	{
		private CombatTarget _target;
		private Mover _mover;
		private Animator _anim;
		private ActionScheduler _scheduler;
		private Health _health;

		[SerializeField] private float attackSpeed = 1f;

		[SerializeField] private Transform rightHand;
		[SerializeField] private Transform leftHand;
		[SerializeField] private Weapon currentWeapon;
		[SerializeField] private Weapon defaultWeapon;

		private static readonly int Attack1 = Animator.StringToHash("attack");

		private float _timeSinceLastAttack = Mathf.Infinity;
		private static readonly int Die1 = Animator.StringToHash("die");
		private bool _isDead = false;
		private static readonly int StopAttack = Animator.StringToHash("stopAttack");

		private void Awake()
		{
			_mover = GetComponent<Mover>();
			_anim = GetComponent<Animator>();
			_scheduler = GetComponent<ActionScheduler>();
			_health = GetComponent<Health>();
			if (_health.isDead) Die();
			if (currentWeapon == null) EquipWeapon(defaultWeapon);
		}

		private void Update()
		{
			_timeSinceLastAttack += Time.deltaTime;
			if (_target == null) return;
			if (_target.IsDead())
			{
				StopAttacking();
				return;
			}
			bool isInRange = Vector3.Distance(transform.position, _target.transform.position) <= currentWeapon.GetWeaponRange();
			if (isInRange)
			{
				_mover.Stop();
				PerformAttack();
			}
			else
			{
				_mover.MoveTo(_target.transform.position, 1f);
			}
		}

		public void EquipWeapon(Weapon weapon)
		{
			if (weapon == null) return;
			currentWeapon = weapon;
			weapon.Spawn(rightHand, leftHand, _anim);
		}

		public CombatTarget GetTarget()
		{
			return _target;
		}

		public void Attack(CombatTarget target)
		{
			transform.LookAt(target.transform);
			_target = target;
		}

		private void PerformAttack()
		{
			if (_timeSinceLastAttack < 1f / attackSpeed) return;
			_timeSinceLastAttack = 0f;
			// This triggers the Hit() event
			_anim.ResetTrigger(StopAttack);
			_anim.SetTrigger(Attack1);
		}

		// This is called from an Animation Event
		public void Hit()
		{
			if (_target == null) return;
			_target.TakeDamage(currentWeapon.GetWeaponDamage());
		}

		// This is called from an Animation Event
		public void Shoot()
		{
			if (_target == null) return;
			if (currentWeapon.HasProjectile())
			{
				currentWeapon.LaunchProjectile(rightHand, leftHand, _target);
			}
			else
			{
				_target.TakeDamage(currentWeapon.GetWeaponDamage());
			}
		}

		private void StopAttacking()
		{
			_target = null;
			_anim.ResetTrigger(Attack1);
			_anim.SetTrigger(StopAttack);
		}

		public void StopAction()
		{
			StopAttacking();
			GetComponent<Mover>().Stop();
		}

		public void Die()
		{
			if (_isDead) return;
			_anim.SetTrigger(Die1);
			_isDead = true;
		}

		public object CaptureState()
		{
			return currentWeapon.name;
		}

		public void RestoreState(object state)
		{
			Weapon weapon = !(state is string weaponName) ? defaultWeapon : UnityEngine.Resources.Load<Weapon>(weaponName);
			EquipWeapon(weapon);
		}
	}
}
