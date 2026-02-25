using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.SOScripts.ClassSO;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events
{
    public class PlayerSpecChosenEvent : GameEventBase
    {
        public ClassSpecSO Spec;
        public PlayerSpecChosenEvent(ClassSpecSO spec)
        {
            Spec = spec;
        }
    }
}