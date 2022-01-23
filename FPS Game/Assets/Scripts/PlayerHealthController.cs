using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;
    [SerializeField] float invincibleLength = 1f;
    private float invincCounter;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            
            if(UIController.instance != null)
            {
                UIController.instance.healthSlider.maxValue = maxHealth;
                UIController.instance.healthSlider.value = currentHealth;
                UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
            }
            
            }

        else
        {
            Destroy(transform.gameObject); 
            DontDestroyOnLoad(transform.gameObject);
        }

    }

    void Start()
    {
        if (UIController.instance != null)
        {
            UIController.instance.healthSlider.maxValue = maxHealth;
            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
        }
    }

    void Update()
    {
        
        if(invincCounter > 0)
        {
            invincCounter -= Time.deltaTime;
        }

    }

    public void TakeDamage(int damage)
    {
        if(invincCounter <= 0)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                gameObject.SetActive(false);
                currentHealth = 0;
                GameManager.instance.PlayerDied();
            }

            invincCounter = invincibleLength;

            RefreshUIHeal();
            
        }
    }


    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;

        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

        RefreshUIHeal();
    }

    private void RefreshUIHeal()
    {
        if (UIController.instance != null)
        {
            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
        }
    }

}
