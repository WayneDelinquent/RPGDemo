using RPG.Core.Interfaces;
using RPG.Resources;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
	public class Mover : MonoBehaviour, ISchedulableAction, ISaveable
	{
		[SerializeField] private float maxSpeed = 6f;

		private NavMeshAgent _agent;
		private Camera _main;
		private Animator _anim;
		private Health _health;
		private static readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");

		private void Awake()
		{
			_agent = GetComponent<NavMeshAgent>();
			_anim = GetComponent<Animator>();
			_health = GetComponent<Health>();
		}

		// Update is called once per frame
		private void Update()
		{
			_agent.enabled = !_health.isDead;
			UpdateAnimator();
		}

		public void StartMoving(Vector3 destination, float speedFraction = 1f)
		{
			MoveTo(destination, speedFraction);
		}

		public void MoveTo(Vector3 destination, float speedFraction = 1f)
		{
			_agent.isStopped = false;
			_agent.speed = Mathf.Clamp01(speedFraction) * maxSpeed;
			_agent.SetDestination(destination);
		}

		public void Stop()
		{
			_agent.isStopped = true;
		}

		private void UpdateAnimator()
		{
			_anim.SetFloat(ForwardSpeed, transform.InverseTransformDirection(_agent.velocity).z);
		}

		public void StopAction()
		{
			Stop();
		}

		public object CaptureState()
		{
			return new SerializableVector3(transform.position);
		}

		public void RestoreState(object state)
		{
			SerializableVector3 position = (SerializableVector3) state;
			_agent.Warp(position.ToVector());
		}
	}
}
