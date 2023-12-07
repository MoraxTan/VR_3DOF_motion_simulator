using UnityEditor;
using UnityEngine;

public class PathEffrctController : MonoBehaviour
{
    public GameObject fireworksEffect;
    public float timeBeforeExit = 3f;
    private bool isPathCompleted;

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
        
    }

    private void ExitPlayModeIfInEditor()
    {
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
    }

    private void ActiveFireworksEffect()
    {
        fireworksEffect.SetActive(true);
    }

    private bool PathIsCompleted()
    {
        return Time.time >= timeBeforeExit;
    }
}
