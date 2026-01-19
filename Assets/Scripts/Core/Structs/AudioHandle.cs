using System;
using UnityEngine;

public readonly struct AudioHandle
{
    public static readonly AudioHandle Empty = new AudioHandle(null, null);
    public readonly AudioSource Source;
    public readonly Action ReturnToPool;
    public bool IsValid => Source != null;

    public AudioHandle(AudioSource source, Action returnToPool)
    {
        Source = source;
        ReturnToPool = returnToPool;
    }
}
