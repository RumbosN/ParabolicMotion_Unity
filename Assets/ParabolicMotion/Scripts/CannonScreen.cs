using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonScreen : MonoBehaviour
{
	[SerializeField] protected Text _velocityText;
	[SerializeField] protected Image _velocityBar;
	[SerializeField] protected Text _angleText;

	protected SnowWeapon _snowWeaponParent;

	protected virtual void Start() {
		_snowWeaponParent = GetComponentInParent<SnowWeapon>();
	}

	protected virtual void Update() {
		UpdateVelocity();
		UpdateAngle();
	}

	protected void UpdateVelocity() {
		var velocity = _snowWeaponParent.CurrentVelocity;
		_velocityBar.fillAmount = velocity / _snowWeaponParent.maxVelocity;
		_velocityText.text = velocity.ToString("F2");
	}

	protected void UpdateAngle() {
		var radianAngle = _snowWeaponParent.GetShootAngle();
		var degreeAngle = Mathf.CeilToInt(AngleUtils.RadiansToDegree(radianAngle));
		_angleText.text = $"{degreeAngle.ToString()}°";
	}
}
