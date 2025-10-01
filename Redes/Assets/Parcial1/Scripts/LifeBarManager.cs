using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarManager : MonoBehaviour
{
    public static LifeBarManager Instance;

    [SerializeField] LifeBar _lifeBarPrefab;

    //Coleccion con todas las barras de vida en juego
    List<LifeBar> _lifeBarsInUse;

    private void Awake()
    {
        Instance = this;

        _lifeBarsInUse = new List<LifeBar>();
    }


    public void CreateNewBar(PlayerBehaviour owner)
    {
        //Crear la barra
        var newBar = Instantiate(_lifeBarPrefab, transform);
        //Asignarle su owner
        newBar.Initialize(owner.transform);

        //Cuando el jugador actualice su vida, se actualiza la barra
        owner.OnLifeUpdate += newBar.UpdateFill;

        //Guardarla en la lista
        _lifeBarsInUse.Add(newBar);

        //Si el jugador muere, sacarla de la lista y eliminar
        owner.OnDespawn += () =>
        {
            _lifeBarsInUse.Remove(newBar);
            Destroy(newBar.gameObject);
        };
    }


    //Metodo para ejecutar el posicionamiento de todas las barras en juego
    private void LateUpdate()
    {
        foreach (var lifeBar in _lifeBarsInUse)
        {
            lifeBar.UpdatePosition();
        }
    }
}
