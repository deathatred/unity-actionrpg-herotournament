using Assets.Scripts.Core.Structs;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{

    [Header("Pool Settings")]
    [SerializeField] private int pool2Dsize = 8;
    [SerializeField] private int pool3Dsize = 20;
    [SerializeField] private bool expandable = true;

    [Header("3D sound settings")]
    [SerializeField] private float minDistance = 1.5f;
    [SerializeField] private float maxDistance = 12f;

    [Header("Music Settings")]
    [SerializeField] private float _musicVolume = 1f;
    private AudioSource _musicSource;

    [Header("Ambient Settings")]
    [SerializeField] private float _ambientVolume = 1f;
    private AudioSource _ambientSource;

    [Header("Mixer Groups")]
    [SerializeField] private AudioMixerGroup _sfxGroup;
    [SerializeField] private AudioMixerGroup _musicGroup;
    [SerializeField] private AudioMixerGroup _ambientGroup;

    private readonly Queue<AudioSource> pool2D = new Queue<AudioSource>();
    private readonly Queue<AudioSource> pool3D = new Queue<AudioSource>();

    private Transform _root2D;
    private Transform _root3D;

    private CancellationTokenSource _musicCts;
    private CancellationTokenSource _ambientCts;
    public void Awake()
    {
        _root2D = new GameObject("Audio_2D").transform;
        _root2D.SetParent(transform);
        _root3D = new GameObject("Audio_3D").transform;
        _root3D.SetParent(transform);

        for (int i = 0; i < pool2Dsize; i++)
        {
            pool2D.Enqueue(Create2DSource());
        }
        for (int i = 0; i < pool3Dsize; i++)
        {
            pool3D.Enqueue(Create3DSource());
        }

        _musicSource = CreateLoopingSource(null, _musicGroup);
        _ambientSource = CreateLoopingSource(null, _ambientGroup);
    }

    private AudioSource Create2DSource()
    {
        GameObject newGameObject = new GameObject("2D_SFX");
        newGameObject.transform.SetParent(_root2D);
        AudioSource audioSource = newGameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
        audioSource.outputAudioMixerGroup = _sfxGroup;
        audioSource.dopplerLevel = 0f;
        newGameObject.SetActive(false);
        return audioSource;
    }
    private AudioSource Create3DSource()
    {
        GameObject newGameObject = new GameObject("3D_SFX");
        newGameObject.transform.SetParent(_root3D);
        AudioSource audioSource = newGameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Custom;
        audioSource.outputAudioMixerGroup = _sfxGroup;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;

        var curve = new AnimationCurve(
            new Keyframe(0f, 1f),
            new Keyframe(minDistance, 1f),
            new Keyframe(maxDistance * 0.6f, 0.2f),
            new Keyframe(maxDistance, 0f)
        );
        audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, curve);
        audioSource.dopplerLevel = 1f;
        newGameObject.SetActive(false);
        return audioSource;
    }
    private AudioSource Get2D()
    {
        if (pool2D.Count > 0)
        {
            return pool2D.Dequeue();
        }
        if (expandable) return Create2DSource();
        return null;
    }
    private AudioSource Get3D()
    {
        if (pool3D.Count > 0)
        {
            return pool3D.Dequeue();
        }
        if (expandable)
        {
            return Create3DSource();
        }
        return null;
    }
    private void Return2D(AudioSource audioSource)
    {
        audioSource.Stop();
        audioSource.clip = null;
        audioSource.gameObject.SetActive(false);
        audioSource.transform.SetParent(_root2D);
        pool2D.Enqueue(audioSource);
    }
    private void Return3D(AudioSource audioSource)
    {
        audioSource.Stop();
        audioSource.clip = null;
        audioSource.gameObject.SetActive(false);
        audioSource.transform.SetParent(_root3D);
        pool3D.Enqueue(audioSource);
    }
    private AudioSource CreateLoopingSource(AudioClip clip, AudioMixerGroup group)
    {
        GameObject go = new GameObject(group.name + "_Source");
        go.transform.SetParent(transform);
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.spatialBlend = 0f;
        audioSource.outputAudioMixerGroup = group;
        audioSource.volume = 1f;
        return audioSource;
    }
    public void PlayMusic(AudioClip clip)
    {
        PlayLoopingTrackAsync(_musicSource, _musicCts, clip, _musicVolume).Forget();
    }

    public void PlayAmbient(AudioClip clip)
    {
        PlayLoopingTrackAsync(_ambientSource, _ambientCts, clip, _ambientVolume).Forget();
    }
    private IEnumerator FollowWhilePlaying(AudioSource audioSource, Transform target, bool is3D)
    {
        while (audioSource != null && audioSource.isPlaying && target != null)
        {
            audioSource.transform.position = target.position;
            yield return null;
        }
        if (audioSource == null)
        {
            yield break;
        }
        if (is3D) Return3D(audioSource);
        else { Return2D(audioSource); }
    }
 
    public void Play2D(AudioClip clip, float volume = 1f, float pitchVariance = 0f)
    {
        if (clip == null) { return; }
        AudioSource audioSource = Get2D();
        if (audioSource == null) { return; }

        audioSource.gameObject.SetActive(true);
        audioSource.volume = Mathf.Clamp01(volume);
        audioSource.pitch = 1f + Random.Range(-pitchVariance, pitchVariance);
        audioSource.clip = clip;
        audioSource.Play();

        ReturnAfterAsync(audioSource, clip.length / Mathf.Max(0.01f, audioSource.pitch), false).Forget();
    }
    public void Play3D(AudioClip clip, Transform follow,
        float volume = 1f, float pitchVariance = 0f)
    {
        if (clip == null) { return; }
        AudioSource audioSource = Get3D();
        if (audioSource == null) { return; }

        audioSource.transform.position = follow.position;
        audioSource.gameObject.SetActive(true);
        audioSource.volume = Mathf.Clamp01(volume);
        audioSource.pitch = 1f + Random.Range(-pitchVariance, pitchVariance);
        audioSource.clip = clip;
        audioSource.Play();

        ReturnAfterAsync(audioSource,clip.length / Mathf.Max(0.01f, audioSource.pitch), true).Forget();
    }
    public AudioHandle Play3DFollow(AudioClip clip, Transform follow,
    float volume = 1f, bool loop = false, float pitch = 1f)
    {
        if (clip == null || follow == null)
        {
            return AudioHandle.Empty;
        }

        AudioSource audioSource = Get3D();
        if (audioSource == null) return AudioHandle.Empty;

        audioSource.transform.position = follow.position;
        audioSource.gameObject.SetActive(true);
        audioSource.volume = Mathf.Clamp01(volume);
        audioSource.pitch = pitch;
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
        StartCoroutine(FollowWhilePlaying(audioSource, follow, true));

        return new AudioHandle(audioSource, () => Return3D(audioSource));
    }
    public void Stop(AudioHandle handle)
    {
        if (!handle.IsValid) return;
        if (handle.Source != null)
        {
            handle.Source.Stop();
            handle.ReturnToPool?.Invoke();
        }
    }
    private async UniTask FadeVolumeAsync( AudioSource source,float from,float to,float fadeTime,CancellationToken token)
    {
        float elapsed = 0f;

        while (elapsed < fadeTime)
        {
            if (token.IsCancellationRequested || source == null)
                return;

            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(from, to, elapsed / fadeTime);
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        if (source != null)
            source.volume = to;
    }
    private async UniTask FadeMusicToAsync(AudioSource source, float targetVolume, AudioClip newClip,
        float fadeTime,
        CancellationToken token)
    {
        await FadeVolumeAsync(source, source.volume, 0f, fadeTime * 0.5f, token);

        if (source == null || token.IsCancellationRequested)
            return;

        source.clip = newClip;
        source.Play();

        await FadeVolumeAsync(source, 0f, targetVolume, fadeTime * 0.5f, token);
    }
    public async UniTask PlayLoopingTrackAsync(AudioSource source, CancellationTokenSource cts,AudioClip clip,
    float targetVolume,
    float fadeTime = 3f)
    {
        if (clip == null) return;

        cts?.Cancel();
        cts = new CancellationTokenSource();

        var token = cts.Token;

        if (!source.isPlaying)
        {
            source.clip = clip;
            source.volume = 0f;
            source.Play();

            await FadeVolumeAsync(source, 0f, targetVolume, fadeTime, token);
        }
        else
        {
            await FadeMusicToAsync(source, targetVolume, clip, fadeTime, token);
        }
    }
    private async UniTask ReturnAfterAsync(AudioSource audioSource,float seconds,bool is3D)
    {
        float time = 0f;

        while (time < seconds && audioSource != null && audioSource.isPlaying)
        {
            time += Time.deltaTime;
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        if (audioSource == null)
            return;

        if (is3D) Return3D(audioSource);
        else Return2D(audioSource);
    }
}


