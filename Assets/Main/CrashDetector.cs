using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] ParticleSystem crashEffect;
    [SerializeField] float reloadDelay = 1f;

    SceneController sceneController;

    private void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
        if (sceneController == null)
        {
            Debug.LogError("SceneController not found in the scene. Please make sure it's added to the scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Ground"))
        {
            if (sceneController != null)
            {
                crashEffect.Play();
                StartCoroutine(ReloadSceneAfterDelay());
            }
            else
            {
                Debug.LogError("SceneController reference is null.");
            }
        }
    }

    private IEnumerator ReloadSceneAfterDelay()
    {
        yield return new WaitForSeconds(reloadDelay);
        sceneController.ReloadScene();
    }
}
