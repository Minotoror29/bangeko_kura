using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private float maxUnderlineWidth;
    [SerializeField] private float underlineSpeed = 10f;
    [SerializeField] private Image underlineMask;

    private float _currentUnderlineWidth = 0f;

    private EventInstance _selectSound;
    private EventInstance _clickSound;

    private void Start()
    {
        _selectSound = RuntimeManager.CreateInstance("event:/UI/SelectUI");
        _clickSound = RuntimeManager.CreateInstance("event:/UI/Click UI");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Select();
    }

    public void Select()
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            _selectSound.start();
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    public void PlayClickSound()
    {
        _clickSound.start();
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
