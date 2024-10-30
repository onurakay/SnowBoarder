using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private float reloadDelay = 1f;
    [SerializeField] private ParticleSystem finishEffect;

    private AudioSource audioSource;
    private bool hasFinished = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing from FinishLine.");
        }

        if (finishEffect == null)
        {
            Debug.LogError("FinishEffect is not assigned in the inspector.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasFinished) return;

        if (other.CompareTag("Player"))
        {
            audioSource?.Play();
            finishEffect?.Play();
            StartCoroutine(LoadNextSceneAfterDelay());
            hasFinished = true;
        }
    }

    private IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(reloadDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            //return to the first scene
            SceneManager.LoadScene(0);
        }
    }
}
