using UnityEditor;
using UnityEngine;

public class PathEffrctController : MonoBehaviour
{
    public GameObject fireworksEffect;
    public AudioClip turnSound;
    public float timeBeforeExit = 3f;
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
        Debug.Log("turnDetected outside");
        if (TurnDetected())
        {
            
            if (!isTurned)
            {
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
        Debug.Log("turnDetected outside");
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
        audioSource.Play();
        Invoke("StopTurnSound", turnSoundDuration);
    }

    private void StopTurnSound()
    {
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
        Debug.Log("turnDetected inside");
        string signal = acceleration.ySignal;
        Debug.Log(signal);
        return (signal.Equals("1") || signal.Equals("2") || signal.Equals("4") || signal.Equals("5"));
    }
}
