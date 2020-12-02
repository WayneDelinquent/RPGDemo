using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
	public class PlayerController : MonoBehaviour
	{
		private Mover _mover;
		private Fighter _fighter;
		private Camera _main;
		private ActionScheduler _scheduler;
		private CombatTarget _combatTarget;

		// Start is called before the first frame update
		private void Start()
		{
			_main = Camera.main;
			_mover = GetComponent<Mover>();
			_fighter = GetComponent<Fighter>();
			_scheduler = GetComponent<ActionScheduler>();
			_combatTarget = GetComponent<CombatTarget>();
		}

		// Update is called once per frame
		private void Update()
		{
			if (_combatTarget.IsDead()) return;
			PerformPrioritizedAction();
		}

		private void PerformPrioritizedAction()
		{
			if (Combat()) return;
			if (Move()) return;
		}

		private bool Move()
		{
			if (!Physics.Raycast(GetMouseRay(), out var info)) return false;
			if (!Input.GetMouseButton(0)) return true;
			_scheduler.StartAction(_mover);
			_mover.StartMoving(info.point, 1f);
			return true;

		}

		private bool Combat()
		{
			RaycastHit[] hits = new RaycastHit[50];
			int size = Physics.RaycastNonAlloc(GetMouseRay(), hits );
			for (int index = 0; index < size; index++)
			{
				var target = hits[index].transform.GetComponent<CombatTarget>();
				if (!CanAttack(target)) continue;
				if (!Input.GetMouseButton(0)) return true;
				_fighter.Attack(target);
				_scheduler.StartAction(_fighter);
				return true;
			}

			return false;
		}

		private bool CanAttack(CombatTarget target)
		{
			return !(target == null || !target.CanBeAttacked()) && !target.CompareTag("Player");
		}

		private Ray GetMouseRay()
		{
			return _main.ScreenPointToRay(Input.mousePosition);
		}
	}
}
