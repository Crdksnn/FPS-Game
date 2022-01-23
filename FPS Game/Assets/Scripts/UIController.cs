using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider healthSlider;
    public Text healthText;
    public Text ammoText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(transform.gameObject);
            DontDestroyOnLoad(transform.gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
