using UnityEngine;

namespace Assets.Scripts.Runtime.BaseLogic
{
    public interface IState
    {
        void Enter();
        void Update();
        void Exit();
    }
}