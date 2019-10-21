using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image healthbar;
    private float originalWidth;

    private void OnEnable()
    {
        originalWidth = healthbar.rectTransform.rect.width;
        //Debug.Log("Width: " + originalWidth);
        EventAnnouncer.OnPlayerDamaged += UpdateHealthbar;
    }

    private void OnDisable()
    {
        EventAnnouncer.OnPlayerDamaged -= UpdateHealthbar;
    }

    private void UpdateHealthbar(float damage, float health)
    {
        int width = (int)Mathf.Round((originalWidth * 0.01f) * health);
        healthbar.rectTransform.sizeDelta = new Vector2(width, healthbar.rectTransform.rect.height);
    }
}
