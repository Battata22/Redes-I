using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CargaConectando : MonoBehaviour
{
    [SerializeField] Image _fondo;
    [SerializeField] Sprite[] _imagenes;
    [SerializeField] float _timer;
    float wait;
    int _actualSprite = 0;

    void Update()
    {
        wait += Time.deltaTime;

        if (wait >= _timer)
        {
            wait = 0;
            if (_actualSprite + 1 >= _imagenes.Length)
            {
                _actualSprite = 0;
                _fondo.sprite = _imagenes[0];
            }
            else
            {
                _actualSprite += 1;
                _fondo.sprite = _imagenes[_actualSprite];
            }
        }
    }

}
