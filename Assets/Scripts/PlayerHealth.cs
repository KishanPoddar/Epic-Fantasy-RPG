using StarterAssets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float maxHealth = 100;
    public float currentHealth;
    private float damageMultiplier = 1;
    [SerializeField] private Slider healthSlider;
    private GameObject enemy;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }
    private void OnEnable()
    {
        transform.GetComponent<ThirdPersonController>().enabled = true;
    }

    private void LateUpdate()
    {
        enemy = GameObject.FindWithTag("Enemy");
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage * damageMultiplier;
        healthSlider.value = currentHealth;
        transform.GetComponent<Animator>().Play("GetHit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        transform.GetComponent<Animator>().Play("Death");
        transform.GetComponent<ThirdPersonController>().enabled = false;
        Invoke("ReloadScene", 3f);
    }

    public void DamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }

    void ReloadScene()
    {
        Debug.Log("Scene Reloaded");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Heal"))
        {
            GetComponent<Animator>().Play("Buff");
            currentHealth = maxHealth;
            healthSlider.value = currentHealth;
        }
    }
}
