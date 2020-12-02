using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
	public class Projectile : MonoBehaviour
	{
		[SerializeField] private float projectileSpeed = 1f;
		[SerializeField] private bool followTarget = false;
		[SerializeField] private GameObject hitEffect;
		[SerializeField] private float maxLifeTime = 10f;
		[SerializeField] private float lifeAfterImpact = 2f;
		[SerializeField] private List<GameObject> destroyOnHit = new List<GameObject>();

		private CombatTarget _target;
		private float _damage = 0f;
		private void Update()
		{
			if (_target == null) return;
			if (followTarget && !_target.IsDead()) transform.LookAt(GetAimLocation());
			transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
		}

		public void SetTarget(CombatTarget target, float damage, bool setDirection = false)
		{
			_target = target;
			_damage = damage;
			if (setDirection) transform.LookAt(GetAimLocation());

			Destroy(gameObject, maxLifeTime);
		}

		private Vector3 GetAimLocation()
		{
			CapsuleCollider targetCollider = _target.GetComponent<CapsuleCollider>();
			if (targetCollider == null) return _target.transform.position;
			return _target.transform.position + Vector3.up * targetCollider.height/1.7f;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.transform == _target.transform)
			{
				if (_target.IsDead()) return;
				_target.TakeDamage(_damage);
			}

			if (hitEffect != null)
			{
				Instantiate(hitEffect, GetAimLocation(), Quaternion.identity);
			}

			foreach (GameObject objectToDestroy in destroyOnHit)
			{
				Destroy(objectToDestroy);
			}

			projectileSpeed = 0f;
			Destroy(gameObject, lifeAfterImpact);
		}
	}
}
