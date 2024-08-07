using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("The amount of torque applied when moving left or right.")]
    private float torqueAmount = 10f;

    [SerializeField, Tooltip("Time in seconds to start counting air time.")]
    private float airTimeThreshold = 0.1f;

    [SerializeField, Tooltip("Time in seconds after which the game should reload.")]
    private float maxAirTime = 1f;

    [SerializeField, Tooltip("Delay in seconds before reloading the game.")]
    private float reloadDelay = 1f;

    [SerializeField, Tooltip("Delay in seconds before showing the message.")]
    private float messageDelay = 0f;

    [SerializeField, Tooltip("Reference to the TextMeshPro component for displaying air time and messages.")]
    private TMP_Text airTimeText;

    private Rigidbody2D rb2d;
    private SceneController sceneController;

    private bool isGrounded = false;
    private float airTime = 0f;
    private float countTime = 0f;
    private bool messageDisplayed = false;
    private bool isReloading = false;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sceneController = FindObjectOfType<SceneController>();

        if (airTimeText == null)
        {
            Debug.LogError("AirTimeText is not assigned in the inspector.");
            return;
        }

        airTimeText.gameObject.SetActive(false);

        if (sceneController == null)
        {
            Debug.LogError("SceneController not found in the scene.");
        }
    }

    private void Update()
    {
        HandleMovement();

        if (!isGrounded && !isReloading)
        {
            HandleAirTime();
        }
        else
        {
            ResetAirTime();
        }
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb2d.AddTorque(torqueAmount);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rb2d.AddTorque(-torqueAmount);
        }
    }

    private void HandleAirTime()
    {
        airTime += Time.deltaTime;

        if (airTime > airTimeThreshold)
        {
            countTime = airTime - airTimeThreshold;
            UpdateAirTimeText();

            if (countTime > messageDelay && !messageDisplayed)
            {
                DisplayMessage("Where are you going!");
            }

            if (countTime > maxAirTime && messageDisplayed)
            {
                DisplayMessage("You've been flying too much! Time to land!");
                if (!isReloading)
                {
                    isReloading = true;
                    StartCoroutine(ReloadSceneAfterDelay());
                }
            }
        }
    }

    private void UpdateAirTimeText()
    {
        if (airTimeText != null)
        {
            airTimeText.gameObject.SetActive(true);
            airTimeText.text = $"Air Time: {countTime:F2} seconds";
        }
    }

    private void DisplayMessage(string message)
    {
        if (airTimeText != null)
        {
            airTimeText.text = message;
            messageDisplayed = true;
        }
    }

    private void ResetAirTime()
    {
        airTime = 0f;

        if (airTimeText != null && !isReloading)
        {
            airTimeText.gameObject.SetActive(false);
            messageDisplayed = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
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
