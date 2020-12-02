using RPG.Core.Interfaces;
using UnityEngine;

namespace RPG.Core
{
	public class ActionScheduler : MonoBehaviour
	{
		private ISchedulableAction _currentAction;

		public void StartAction(ISchedulableAction action)
		{
			if (action == _currentAction) return;
			_currentAction?.StopAction();
			_currentAction = action;
		}

		public void StopCurrentAction()
		{
			_currentAction?.StopAction();
		}
	}
}
