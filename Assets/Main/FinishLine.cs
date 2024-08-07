using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] float reloadDelay = 1f;
    [SerializeField] ParticleSystem finishEffect;

    private SceneController sceneController;

    private void Start() 
    {
        sceneController = FindObjectOfType<SceneController>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            GetComponent<AudioSource>().Play();
            finishEffect.Play();
            StartCoroutine(ReloadSceneAfterDelay());

        }
    }

    private IEnumerator ReloadSceneAfterDelay()
    {
        yield return new WaitForSeconds(reloadDelay);
        if (sceneController != null)
        {
            sceneController.ReloadScene();
        }
    }
}
