using System;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;

namespace RPG.Control
{
	public class AIController : MonoBehaviour
	{
		[SerializeField] private float chaseDistance = 5f;
		[SerializeField] private float staySuspiciousTime = 3f;
		[Range(0, 1)] [SerializeField] private float patrolSpeedFraction = 0.2f;

		private GameObject _player;
		private Fighter _fighter;
		private ActionScheduler _scheduler;
		private Mover _mover;
		private Health _health;
		private Patroller _patrol;

		private float _suspiciousTime = Mathf.Infinity;
		private Vector3 _lastKnownLocation;
		private Vector3 _guardLocation;

		private void Awake()
		{
			_player = GameObject.FindWithTag("Player");
			_fighter = GetComponent<Fighter>();
			_scheduler = GetComponent<ActionScheduler>();
			_mover = GetComponent<Mover>();
			_health = GetComponent<Health>();
			_guardLocation = transform.position;
			_patrol = GetComponent<Patroller>();
		}

		private void Update()
		{
			if (_health.isDead) return;
			if (IsInChaseRange() && CanAttack(_player.GetComponent<CombatTarget>()))
			{
				_suspiciousTime = 0f;
				AttackBehaviour();
			}
			else if (_suspiciousTime < staySuspiciousTime)
			{
				_suspiciousTime += Time.deltaTime;
				SuspicionBehaviour();
			}
			else
			{
				GuardBehaviour();
			}
		}

		private void AttackBehaviour()
		{
			_fighter.Attack(_player.GetComponent<CombatTarget>());
			_scheduler.StartAction(_fighter);
			_lastKnownLocation = _player.transform.position;
		}

		private void SuspicionBehaviour()
		{
			_scheduler.StartAction(_mover);
			_mover.StartMoving(_lastKnownLocation, patrolSpeedFraction);
		}

		private void GuardBehaviour()
		{
			if (_patrol == null || !_patrol.HasPatrolPath())
			{
				_scheduler.StartAction(_mover);
				_mover.StartMoving(_guardLocation, patrolSpeedFraction);
			}
			else
			{
				_patrol.StartPatrolling();
			}
		}

		private bool IsInChaseRange()
		{
			return Vector3.Distance(transform.position, _player.transform.position) <= chaseDistance;
		}

		private static bool CanAttack(CombatTarget target)
		{
			return !(target == null || !target.CanBeAttacked()) && target.CompareTag("Player");
		}

		// Called by Unity during Editor Mode
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(transform.position, chaseDistance);
		}
	}
}
