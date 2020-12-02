using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.SceneManagement
{
	public class Fader : MonoBehaviour
	{
		private CanvasGroup _cg;

		private void Awake()
		{
			_cg = GetComponent<CanvasGroup>();
		}

		public void FadeOutImmediate()
		{
			_cg.alpha = 1;
		}

		public IEnumerator FadeIn(float time)
		{
			while (_cg.alpha > 0)
			{
				_cg.alpha -= Time.deltaTime / time;
				yield return null;
			}
		}

		public IEnumerator FadeOut(float time)
		{
			while (_cg.alpha < 1)
			{
				_cg.alpha += Time.deltaTime / time;
				yield return null;
			}
		}
	}
}
