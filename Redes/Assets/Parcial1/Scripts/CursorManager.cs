using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    [SerializeField] Image _crosshair;
    [SerializeField] float _speedAnimation;
    bool _doingShootAnimation = false;
    float _shootAnimationControl;

    void Start()
    {
        TurnOffCursor();
        TurnOnCrosshair();
    }


    void Update()
    {
        SetCrosshairPosition();

        if (Input.GetMouseButtonDown(0))
        {
            _doingShootAnimation = true;
        }

        if (_doingShootAnimation)
        {
            CrosshairShootEffect();
        }
    }

    void CrosshairShootEffect()
    {
        if (_shootAnimationControl < 360)
        {
            _crosshair.transform.eulerAngles += new Vector3(0, 0, _speedAnimation * Time.deltaTime);
            _shootAnimationControl += _speedAnimation * Time.deltaTime;
        }
        else
        {
            _crosshair.transform.eulerAngles = Vector3.zero;
            _doingShootAnimation = false;
            _shootAnimationControl = 0;
        }
    }

    void SetCrosshairPosition()
    {
        _crosshair.transform.position = Input.mousePosition;
    }

    void TurnOffCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void TurnOffCrosshair()
    {
        _crosshair.enabled = false;
    }

    void TurnOnCrosshair()
    {
        _crosshair.enabled = true;
    }

    void TurnOnCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
