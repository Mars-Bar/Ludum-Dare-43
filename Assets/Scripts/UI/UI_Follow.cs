using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Follow 
	: MonoBehaviour
{
	private bool _validTarget;

	public Transform Target;
	public Vector3 OffsetWorldPos;
	public Vector2 OffsetScreenPos;

	RectTransform FindCanvasRect()
	{
		Canvas canvas = null;
		RectTransform rt = transform as RectTransform;
		while(rt != null && canvas == null)
		{
			canvas = rt.GetComponent<Canvas>();
			rt = rt.parent as RectTransform;
		}
		if(canvas == null)
		{
			Debug.LogError("UI_Follow has no canvas parent.");
			return null;
		}
		return canvas.transform as RectTransform;
	}

	void Update ()
	{
		RectTransform canvasRect = FindCanvasRect();
		if (canvasRect == null)
			return;

		if(Target == null)
		{
			if(_validTarget)
			{
				// we had a valid target and lost it
				Destroy(gameObject);
			}
			return;
		}


		Vector3 worldPos = Target.position + OffsetWorldPos;

		Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPos);

		Vector2 canvasPos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out canvasPos);
		
		transform.localPosition = canvasPos;

		_validTarget = true;
	}
}
