using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAttackState : State<StateEnum>
{
    IAttack _attack;
    public EnemyAttackState(IAttack attack)
    {
        _attack = attack;
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entro al ataque");
    }
    public override void Execute()
    {
        base.Execute();
        SceneManager.LoadScene("DefeatScreen");
    }
}
