﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonScreen : MonoBehaviour
{
	[SerializeField] protected Text _velocityText;
	[SerializeField] protected Image _velocityBar;
	[SerializeField] protected Text _angleText;
	[SerializeField] protected bool _angleAccuracy = false;

	protected BulletWeapon bulletWeaponParent;

	protected virtual void Start() {
		bulletWeaponParent = GetComponentInParent<BulletWeapon>();
	}

	protected virtual void Update() {
		UpdateVelocity();
		UpdateAngle();
	}

	protected void UpdateVelocity() {
		var velocity = bulletWeaponParent.CurrentVelocity;
		_velocityBar.fillAmount = velocity / bulletWeaponParent.maxVelocity;
		_velocityText.text = velocity.ToString("F2");
	}

	protected void UpdateAngle() {
		var radianAngle = bulletWeaponParent.GetShootAngle();

		if (_angleAccuracy) {
			var degreeAngle = FloatUtils.Floor(AngleUtils.RadiansToDegree(radianAngle), 2);
			_angleText.text = $"{degreeAngle}°";
		}
		else {
			var degreeAngle = Mathf.CeilToInt(AngleUtils.RadiansToDegree(radianAngle));
			_angleText.text = $"{degreeAngle.ToString()}°";
		}
	}
}
