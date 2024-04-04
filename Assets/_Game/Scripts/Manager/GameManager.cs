using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerController playerController;
    public CameraController cameraController;
    public ParticleSystem deathEffect;
    public ParticleSystem respawnEffect;
    public GameObject respawnShield;
    public Animator batControl;
    public Animator tunnelControl;
    public Animator swimControl;
    public Animator gemCollected;
    public Animator keyCollected;
    public AudioManager audioManager;
    public UIManager UIManager;
    public Health health;
    public int numOfHearts = 5;
    public int numOfGems = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        health.health = numOfHearts;
        UIManager.numOfGems.text = "X " + numOfGems;
    }

    public void RespawnPlayer(Vector3 respawnLocation)
    {
        numOfHearts -= 1;
        audioManager.Dead();
        playerController.CancelAbility();
        playerController.DropKey();
        playerController.DropSilverSword();
        playerController.DropGoldSword();
        StartCoroutine(RespawnCooldown(respawnLocation));
    }

    IEnumerator RespawnCooldown(Vector3 playerRespawn)
    {
        batControl.SetBool("Left", false);
        tunnelControl.SetBool("Left", false);
        swimControl.SetBool("Left", false);
        gemCollected.SetBool("Collected", false);
        keyCollected.SetBool("Collected", false);
        playerController.GetComponent<Animator>().SetBool("Death", true);
        deathEffect.transform.position = playerController.transform.position;
        playerController.gameObject.SetActive(false);
        deathEffect.Play();
        yield return new WaitForSeconds(1f);

        if (numOfHearts <= 0)
        {
            Time.timeScale = 0;
            UIManager.DeathMessage();
        }

        yield return new WaitForSeconds(1f);               
        playerController.GetComponent<Animator>().SetBool("Death", false);
        playerController.ResetPosition(playerRespawn);
        Camera.main.transform.position = new Vector3(playerRespawn.x, playerRespawn.y, -10f);
        respawnEffect.transform.position = playerRespawn;
        respawnEffect.Play();
        yield return new WaitForSeconds(1f);
        playerController.gameObject.SetActive(true);
        Physics2D.gravity = new Vector2(0f, -9.81f);
        playerController.isVulnerable = false;
        EnableShield();
        playerController.GetComponent<Rigidbody2DHorizontalMove>().Flip(true);
        yield return new WaitForSeconds(3f);
        playerController.isVulnerable = true;
        DisableShield();
    }

    public void EnableShield()
    {
        respawnShield.SetActive(true);
    }

    public void DisableShield()
    {
        respawnShield.SetActive(false);
    }
}
