using System.Collections;
using UnityEngine;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] private ParticleSystem crashEffect;
    [SerializeField] private float reloadDelay = 1f;

    private bool hasCrashed = false;

    private void Start()
    {
        if (crashEffect == null)
        {
            Debug.LogError("CrashEffect  isnt assigned");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasCrashed) return; // prevent multiple triggers

        if (other.CompareTag("Ground"))
        {
            //"Crash detection with "Ground"
            if (SceneController.Instance != null)
            {
                crashEffect?.Play();
                StartCoroutine(ReloadSceneAfterDelay());
                hasCrashed = true;
            }
            else
            {
                Debug.LogError("SceneController instance is null.");
            }
        }
    }

    private IEnumerator ReloadSceneAfterDelay()
    {
        yield return new WaitForSeconds(reloadDelay);
        SceneController.Instance.ReloadScene();
    }
}
