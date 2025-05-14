

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private bool isDead = false;
    private int foodCount = 0;

    public Text foodScoreText; // top-left live food score (can stay as old Text)
    
    public GameObject gameOverText;
    public GameObject restartButton;

    // TMP text only — no panels
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;

    void Start()
    {
        // Debu("this is start function.");
        
        
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody>();
        isDead = false;

        UpdateFoodText();

        if (gameOverText != null) gameOverText.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);
        if (winText != null) winText.gameObject.SetActive(false);
        if (loseText != null) loseText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Debug.Log("this is update function.");
        if (isDead) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }
    

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            Debug.Log("this is food trigger.");
            foodCount++;
            UpdateFoodText();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("NegativeFood"))
        {
            Debug.Log("this is negative food trigger.");
            foodCount--;
            UpdateFoodText();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("FinishWall"))
        {
            isDead = true;
            ShowEndMessage();
        }

        if (!isDead && other.CompareTag("Enemy"))
        {
            isDead = true;
            rb.AddForce(Vector3.up * 300f + Vector3.back * 300f);
            GetComponent<PlayerMovement>().enabled = false;

            if (gameOverText != null) gameOverText.SetActive(true);
            if (restartButton != null) restartButton.SetActive(true);
        }
    }

    void ShowEndMessage()
    {
        Time.timeScale = 0f;
        if (restartButton != null) restartButton.SetActive(true);

        if (foodCount > 0)
        {
            if (winText != null)
            {
                winText.text = "🎉 You Win! Foods: " + foodCount;
                winText.gameObject.SetActive(true);
            }
        }
        else
        {
            if (loseText != null)
            {
                loseText.text = "💀 You Lose! Final Score: " + foodCount;
                loseText.gameObject.SetActive(true);
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateFoodText()
    {
        if (foodScoreText != null)
            foodScoreText.text = "Foods: " + foodCount;
    }
}
