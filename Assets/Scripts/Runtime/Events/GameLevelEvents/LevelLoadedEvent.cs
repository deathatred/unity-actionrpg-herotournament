using UnityEngine;

public class LevelLoadedEvent : GameEventBase
{
    public Transform SpawnPoint;
    public LevelLoadedEvent(Transform spawnPoint)
    {
        SpawnPoint = spawnPoint;
    }
}
