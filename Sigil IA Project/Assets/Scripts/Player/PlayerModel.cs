using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : Entity
{
    private bool _isDetectable;


    //Hacer un getter para que los Enemies puedan acceder a la informaci�n del IsDetectable

    public void ModifyDetectable(bool isDetectable)
    {
        _isDetectable = isDetectable;
    }
}
