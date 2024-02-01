using UnityEngine;

[CreateAssetMenu(menuName ="States/Player/Walks")]

public class PlayerWalkState : PlayerState<PlayerController>
{
    private Rigidbody rb;

    public override void Init(PlayerController p)
    {
        base.Init(p);
    }

    public override void CaptureInput()
    {
        throw new System.NotImplementedException();
    }

    public override void ChangeState()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void FixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void Start()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}
