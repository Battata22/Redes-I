using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    Transform _owner;

    [SerializeField] float _yOffset;
    [SerializeField] Image _image;

    public void Initialize(Transform owner)
    {
        _owner = owner;
    }

    public void UpdateFill(float amount)
    {
        _image.fillAmount = amount;
    }

    public void UpdatePosition()
    {
        transform.position = _owner.position + Vector3.up * _yOffset;
    }
}
