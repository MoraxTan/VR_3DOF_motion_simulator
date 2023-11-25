using UnityEditor;
using UnityEngine;

public class PathEffrctController : MonoBehaviour
{
    public GameObject fireworksEffect;
    public AudioClip turnSound;
    public float timeBeforeExit = 10f;
    public float turnSoundDuration = 4f;

    private AudioSource audioSource;
    private bool isPathCompleted;
    private bool isTurned;
    private float elapsedTime;

    private Acceleration acceleration;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = turnSound;

        acceleration = GetComponent<Acceleration>();
    }

    private void Update()
    {
        if (PathIsCompleted())
        {
            if (!isPathCompleted)
            {
                ActiveFireworksEffect();
            }
        }
        else
        {
            isPathCompleted = false;
        }

        if (TurnDetected())
        {
            if (!isTurned)
            {
                // 如果正在播放音效，停止当前音效
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }

                PlayTurnSound();
                isTurned = true;
            }
        }
        else
        {
            isTurned = false;
        }

        if (fireworksEffect.activeSelf)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= timeBeforeExit)
            {
                ExitPlayModeIfInEditor();
            }
        }
    }

    private void ExitPlayModeIfInEditor()
    {
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
    }

    private void PlayTurnSound()
    {
        // 播放转向音效
        audioSource.Play();
        // 在指定时间后停止音效
        Invoke("StopTurnSound", turnSoundDuration);
    }

    private void StopTurnSound()
    {
        // 在指定时间后停止音效
        audioSource.Stop();
    }

    private void ActiveFireworksEffect()
    {
        fireworksEffect.SetActive(true);
    }

    private bool PathIsCompleted()
    {
        return Time.time >= timeBeforeExit;
    }

    private bool TurnDetected()
    {
        string signal = acceleration.ySignal;
        return (signal.Equals("1") || signal.Equals("2") || signal.Equals("4") || signal.Equals("5"));
    }
}
