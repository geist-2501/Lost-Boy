using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	struct bar {

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
			fillBox.sizeDelta = new Vector2(baseBox.rect.width * (currentValue / max), 12);
		}

	}

	[SerializeField] private RectTransform healthBarBase;
	[SerializeField] private RectTransform energyBarBase;
	[SerializeField] private RectTransform interactText;
	[SerializeField] private RectTransform ammoCounterText;
	[SerializeField] private RectTransform aimingRect;

	private bar healthBar;
	private bar energyBar;

	// Use this for initialization
	void Start () {
		healthBar = new bar(healthBarBase, 100f);
		energyBar = new bar(energyBarBase, 50f);
	}

	public void UpdateHealthBar(float _val) {
		healthBar.setValue(_val);
	} 

	public void UpdateEnergyBar(float _val) {
		energyBar.setValue(_val);
	}

	public void SetInteractText(bool _active) {
		interactText.gameObject.SetActive(_active);
	}

	public void SetRecticleVisibility(bool _active) {
		aimingRect.gameObject.SetActive(_active);
	}

}
