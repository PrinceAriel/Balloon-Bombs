using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    public float topBound = 15.0f; // Maximum height the balloon can reach
    public float bottomBound = 1.0f; // Minimum height (ground level)
    public float bounceForce = 15.0f; // Force applied when bouncing off ground
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound; // Sound effect for ground bounce

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerRb = GetComponent<Rigidbody>(); // FIX: Assign the Rigidbody component
        playerAudio = GetComponent<AudioSource>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver && transform.position.y < topBound)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }
        
        // Prevent balloon from going above the height limit
        if (transform.position.y > topBound)
        {
            // Clamp the position to the top bound
            Vector3 pos = transform.position;
            pos.y = topBound;
            transform.position = pos;
            
            // Stop any upward velocity to prevent bouncing against the ceiling
            if (playerRb.velocity.y > 0)
            {
                Vector3 velocity = playerRb.velocity;
                velocity.y = 0;
                playerRb.velocity = velocity;
            }
        }
        
        // Prevent balloon from going below the ground and make it bounce (only if game is not over)
        if (transform.position.y < bottomBound)
        {
            // Clamp the position to the bottom bound (ground level)
            Vector3 pos = transform.position;
            pos.y = bottomBound;
            transform.position = pos;
            
            // Only bounce if the game is still active
            if (!gameOver)
            {
                // Apply bounce force upward
                Vector3 velocity = playerRb.velocity;
                velocity.y = Mathf.Abs(velocity.y); // Reverse downward velocity to upward
                velocity.y += bounceForce; // Add extra bounce force
                playerRb.velocity = velocity;
                
                // Play bounce sound effect
                if (bounceSound != null)
                {
                    playerAudio.PlayOneShot(bounceSound, 0.8f);
                }
                
                Debug.Log("Balloon bounced off the ground!");
            }
            else
            {
                // Game is over - stop all movement and stay on ground
                Vector3 velocity = playerRb.velocity;
                velocity.y = 0; // Stop any vertical movement
                playerRb.velocity = velocity;
                
                Debug.Log("Balloon crashed and stays on ground");
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            // Move explosion to the balloon's current position before playing
            explosionParticle.transform.position = transform.position;
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        }
        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            // Move fireworks to the balloon's current position before playing
            fireworksParticle.transform.position = transform.position;
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }
    }
}