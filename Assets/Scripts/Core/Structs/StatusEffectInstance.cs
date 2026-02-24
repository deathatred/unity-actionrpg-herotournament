using System.Threading;

namespace Assets.Scripts.Core.Structs
{
    public class StatusEffectInstance
    {
        public EnemyStatusEffectSO Effect;
        public CancellationTokenSource Cts;

        public StatusEffectInstance(EnemyStatusEffectSO effect)
        {
            Effect = effect;
            Cts = new CancellationTokenSource();
        }
    }
}