using Cysharp.Threading.Tasks;
using Firebase.Database;
using System;
using System.Threading;
using UnityEngine;

public class FirebaseSaveLoadManager
{
    private FirebaseBootstrap _firebase;

    public FirebaseSaveLoadManager(FirebaseBootstrap firebase)
    {
        _firebase = firebase;
    }
    public async UniTask SavePlayerDataToFirebaseAsync(PlayerSaveData playerData, CancellationTokenSource cts)
    {
        try
        {
            var db = FirebaseBootstrap.Db;
            var uid = FirebaseBootstrap.Uid;

            string jsonKey = JsonUtility.ToJson(playerData);
            await db.Child($"users/{uid}/PlayerData")
                 .SetRawJsonValueAsync(jsonKey)
                 .AsUniTask().AttachExternalCancellation(cts.Token);
        }
        catch (OperationCanceledException)
        {
            Debug.LogError("Saving cancelled");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Saving failed, reason: {ex}");
        }
    }
    public async UniTask SaveLevelDataToFirebaseAsync(LevelSaveData levelData, CancellationTokenSource cts)
    {
        try
        {
            var db = FirebaseBootstrap.Db;
            var uid = FirebaseBootstrap.Uid;

            string jsonKey = JsonUtility.ToJson(levelData);
            await db.Child($"users/{uid}/LevelData")
                 .SetRawJsonValueAsync(jsonKey)
                 .AsUniTask().AttachExternalCancellation(cts.Token);
        }
        catch (OperationCanceledException)
        {
            Debug.LogError("Saving cancelled");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Saving failed, reason: {ex}");
        }
    }
    public async UniTask<PlayerSaveData> LoadPlayerDataFromFirebaseAsync(CancellationTokenSource cts) 
    {
        try
        {
            if (!FirebaseBootstrap.IsReady)
            {
                var readyTask = UniTask.WaitUntil(() => FirebaseBootstrap.IsReady);
                var timeoutTask = UniTask.WaitForSeconds(10f, cancellationToken: cts.Token); 

                int finished = await UniTask.WhenAny(readyTask, timeoutTask);
                if (finished == 1) 
                {
                    Debug.LogError("Firebase not ready after 10 seconds");
                    return null; 
                }
            }

            var db = FirebaseBootstrap.Db;
            var uid = FirebaseBootstrap.Uid;
            var snapshot = await db.Child($"users/{uid}/PlayerData").GetValueAsync().AsUniTask().AttachExternalCancellation(cts.Token);
            if (!snapshot.Exists || snapshot.Value == null)
            {
                return null;
            }
            var json = snapshot.GetRawJsonValue();
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning("Save Data json is empty");
                return null;
            }
            PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);

            return data;
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("Player Data loading cancelled");
            return null;
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.ToString());
            return null;
        }   
    }
    public async UniTask<LevelSaveData> LoadLevelDataFromFirebaseAsync(CancellationTokenSource cts)
    {
        try
        {
            if (!FirebaseBootstrap.IsReady)
            {
                var readyTask = UniTask.WaitUntil(() => FirebaseBootstrap.IsReady);
                var timeoutTask = UniTask.WaitForSeconds(10f, cancellationToken: cts.Token);

                int finished = await UniTask.WhenAny(readyTask, timeoutTask);
                if (finished == 1)
                {
                    Debug.LogError("Firebase not ready after 10 seconds");
                    return null;
                }
            }

            var db = FirebaseBootstrap.Db;
            var uid = FirebaseBootstrap.Uid;
            var snapshot = await db.Child($"users/{uid}/LevelData").GetValueAsync().AsUniTask().AttachExternalCancellation(cts.Token);
            if (!snapshot.Exists || snapshot.Value == null)
            {
                return null;
            }
            var json = snapshot.GetRawJsonValue();
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning("Save Data json is empty");
                return null;
            }
            LevelSaveData data = JsonUtility.FromJson<LevelSaveData>(json);
            return data;
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("Level Data loading cancelled");
            return null;
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.ToString());
            return null;
        }
    }
    public async UniTask ClearLevelDataFromFirebaseAsync(CancellationTokenSource cts = default)
    {
        var db = FirebaseBootstrap.Db;
        var uid = FirebaseBootstrap.Uid;

        var t = db.Child($"users/{uid}/LevelData").RemoveValueAsync();
        if (cts != null)
        {
            await t.AsUniTask().AttachExternalCancellation(cts.Token);
        }
        else
        {
            await t.AsUniTask();
        }
    }
    public async UniTask ClearAllDataFromFirebaseAsync(CancellationTokenSource cts = default)
    {
        var db = FirebaseBootstrap.Db;
        var uid = FirebaseBootstrap.Uid;

        var t = db.Child($"users/{uid}").RemoveValueAsync();
        if (cts != null)
        {
            await t.AsUniTask().AttachExternalCancellation(cts.Token);
        }
        else
        {
            await t.AsUniTask();
        }
    }
}
