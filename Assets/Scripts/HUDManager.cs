using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	public struct bar {

		private RectTransform baseBox;
		private RectTransform fillBox;
		private float currentValue;
		private float max;

		public bar (RectTransform _base, float _max) {
			baseBox = _base;
			fillBox = baseBox.GetChild(0) as RectTransform;
			max = _max;
			currentValue = max;
		}

		public void setValue(float _val) {
			currentValue = _val;
			if (_val < 0 || _val > max) {
				return;
			}
			fillBox.sizeDelta = new Vector2(baseBox.rect.width * (currentValue / max), baseBox.rect.height);
		}

		public void Show(bool _show) {
			baseBox.gameObject.SetActive(_show);
			fillBox.gameObject.SetActive(_show);
		}

	}

	[SerializeField] private RectTransform healthBarBase;
	[SerializeField] private RectTransform energyBarBase;

	public RectTransform interactText;
	public RectTransform ammoCounterText;
	public RectTransform aimingRect;
	public RectTransform decals;
	public RectTransform dialogueBox;

	public bar healthBar;
	public bar energyBar;

	// Use this for initialization
	void Start () {
		healthBar = new bar(healthBarBase, 100f);
		energyBar = new bar(energyBarBase, 50f);
	}

	public void SetHUDvisibility (bool _vis) {
		interactText.gameObject.SetActive(_vis);
		ammoCounterText.gameObject.SetActive(_vis);
		aimingRect.gameObject.SetActive(_vis);
		decals.gameObject.SetActive(_vis);
		healthBar.Show(_vis);
		energyBar.Show(_vis);
	}

}
