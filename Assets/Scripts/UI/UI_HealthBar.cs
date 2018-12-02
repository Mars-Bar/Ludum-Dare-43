using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar
	: MonoBehaviour
{
	protected void Awake()
	{
		Slider.minValue = 0;
		if (_healthObj != null)
		{
			Slider.maxValue = _healthObj.MaxHealth;
			Slider.value = _healthObj.Health;
		}

		_rectTransform = transform as RectTransform;
		_initialSize = _rectTransform.sizeDelta;

		_follow = GetComponent<UI_Follow>();
	}

	Vector2 _initialSize;

	private UI_Follow _follow;
	private RectTransform _rectTransform;
	private HealthComponent _healthObj;
	public HealthComponent HealthObj
	{
		get
		{
			return _healthObj;
		}
		set
		{
			_healthObj = value;
			_objSize = _healthObj.GetComponent<ObjectSize>();
		}
	}
	private ObjectSize _objSize;
	public GameObject SliderRootObj;
	public Slider Slider;

	private void Update()
	{
		if (_healthObj == null)
		{
			SliderRootObj.SetActive(false);
			return;
		}

		int health = (int)_healthObj.Health;
		int maxHealth = (int)_healthObj.MaxHealth;

		SliderRootObj.SetActive(health < maxHealth);

		if ((int)Slider.maxValue != maxHealth)
			Slider.maxValue = maxHealth;

		if ((int)Slider.value != health)
			Slider.value = health;

		if(_objSize != null)
		{
			Vector2 newSize = _initialSize;
			newSize.x *= _objSize.Width;
			_rectTransform.sizeDelta = newSize;

			if (_objSize.Anchor == ObjectSize.ObjAnchor.BottomLeft)
			{
				_follow.OffsetWorldPos = new Vector3(
					_objSize.Width / 2f,
					0f,
					0f);
			}
			else if(_objSize.Anchor == ObjectSize.ObjAnchor.Centre)
			{
				_follow.OffsetWorldPos = new Vector3(
					0f,
					-_objSize.Height / 2f,
					0f);
			}
		}
	}
}
