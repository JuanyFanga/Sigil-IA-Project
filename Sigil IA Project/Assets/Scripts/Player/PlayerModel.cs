using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : Entity
{
    private bool _isDetectable;

    public void ModifyDetectable(bool isDetectable)
    {
        _isDetectable = isDetectable;
    }
}
