using UnityEngine;

namespace Assets.Scripts.Core.Data
{
    [SerializeField]
    public class GlobalSaveData : MonoBehaviour
    {
        public PlayerSaveData PlayerSaveData;
        public LevelSaveData LevelSaveData;
    }
}