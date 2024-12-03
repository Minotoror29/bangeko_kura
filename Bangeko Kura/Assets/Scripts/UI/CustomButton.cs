using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private float scaleFactor = 1.25f;

    [SerializeField] private float maxUnderlineWidth;
    [SerializeField] private float underlineSpeed = 10f;
    [SerializeField] private Image underlineMask;

    private float _currentUnderlineWidth = 0f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Select();
    }

    public void Select()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            if (_currentUnderlineWidth < maxUnderlineWidth)
            {
                _currentUnderlineWidth += underlineSpeed * Time.deltaTime * maxUnderlineWidth;
            }
        }
        else
        {
            if (_currentUnderlineWidth > 0f)
            {
                _currentUnderlineWidth -= underlineSpeed * Time.deltaTime * maxUnderlineWidth;
            }
        }

        underlineMask.rectTransform.sizeDelta = new Vector2(_currentUnderlineWidth, 200f);
    }
}
