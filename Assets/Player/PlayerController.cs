using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField, Tooltip("The amount of torque applied when moving left or right.")]
    private float torqueAmount = 10f;

    [Header("Air Time Settings")]
    [SerializeField, Tooltip("Time in seconds to start counting air time.")]
    private float airTimeThreshold = 0.1f;

    [SerializeField, Tooltip("Time in seconds after which the game should reload.")]
    private float maxAirTime = 1f;

    [SerializeField, Tooltip("Delay in seconds before reloading the game.")]
    private float reloadDelay = 1.5f;

    [Header("UI References")]
    [SerializeField, Tooltip("Reference to the TextMeshPro component for displaying air time and messages.")]
    private TMP_Text airTimeText;

    private Rigidbody2D rb2d;
    private bool isGrounded = false;
    private float airTime = 0f;
    private bool isReloading = false;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        // validations
        if (rb2d == null)
        {
            Debug.LogError("Rigidbody2D component missing from Player.");
        }

        if (airTimeText == null)
        {
            Debug.LogError("AirTimeText is not assigned in the inspector.");
        }
        else
        {
            airTimeText.gameObject.SetActive(false);
        }

        if (SceneController.Instance == null)
        {
            Debug.LogError("SceneController instance not found.");
        }
    }

    private void Update()
    {
        if (!isGrounded && !isReloading)
        {
            airTime += Time.deltaTime;

            if (airTime > airTimeThreshold)
            {
                float countTime = airTime - airTimeThreshold;
                UpdateAirTimeUI(countTime);

                if (countTime > maxAirTime)
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
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        rb2d.AddTorque(horizontalInput * torqueAmount);
    }

    private void UpdateAirTimeUI(float countTime)
    {
        if (airTimeText != null)
        {
            airTimeText.gameObject.SetActive(true);
            airTimeText.text = $"Air Time: {countTime:F2} seconds";
            float airTimePercentage = countTime / maxAirTime;
            airTimeText.color = airTimePercentage > 0.8f ? Color.red : Color.white;
        }
    }

    private void DisplayMessage(string message)
    {
        if (airTimeText != null)
        {
            airTimeText.text = message;
            airTimeText.color = Color.yellow;
        }
    }

    private void ResetAirTime()
    {
        airTime = 0f;

        if (airTimeText != null && !isReloading)
        {
            airTimeText.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            ResetAirTime();
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
        SceneController.Instance?.ReloadScene();
    }
}
