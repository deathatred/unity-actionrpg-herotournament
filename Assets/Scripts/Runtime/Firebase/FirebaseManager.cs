using Assets.Scripts.Core.Data;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Assets.Scripts.Runtime.Firebase
{
    public class FirebaseManager : IDisposable
    {
        private FirebaseBootstrap _firebase;
        private FirebaseSaveLoadManager _saveLoad;
        private CancellationTokenSource _cts;
        public bool IsReady { get; private set; }

        private FirebaseManager()
        {
            _firebase = new FirebaseBootstrap();
            _cts = new CancellationTokenSource();
            _firebase.Init(SetIsReadyTrue, false, _cts).Forget();
            _saveLoad = new FirebaseSaveLoadManager(_firebase);
        }
        private void SetIsReadyTrue()
        {
            IsReady = true;
        }
        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
        public async UniTask SavePlayerDataAsync(PlayerSaveData data)
        {
            await _saveLoad.SavePlayerDataToFirebaseAsync(data, _cts);
        }
        public async UniTask SaveLevelDataAsync(LevelSaveData data)
        {
            await _saveLoad.SaveLevelDataToFirebaseAsync(data, _cts);
        }
        public async UniTask<PlayerSaveData> LoadPlayerDataAsync()
        {
            var data = await _saveLoad.LoadPlayerDataFromFirebaseAsync(_cts);
            return data;
        }
        public async UniTask<LevelSaveData> LoadLevelDataAsync()
        {
            var data = await _saveLoad.LoadLevelDataFromFirebaseAsync(_cts);
            return data;
        }
        public async UniTask SaveNewLevelData(LevelSaveData levelData, PlayerSaveData playerData)
        {
            await _saveLoad.ClearLevelDataFromFirebaseAsync();
            await _saveLoad.SaveLevelDataToFirebaseAsync(levelData, _cts);
            await _saveLoad.SavePlayerDataToFirebaseAsync(playerData, _cts);
        }
        public async UniTask ClearAllData()
        {
            await _saveLoad.ClearAllDataFromFirebaseAsync();
        }
    }
}