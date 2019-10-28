using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameScene : SceneBase<GameScene>
{
    [SerializeField] private TextMeshProUGUI scoreText = null;

    public bool IsRunning { get; private set; }
    public bool IsInvincible { get; private set; }
    private Coroutine invincibleCoroutine = null;

    [SerializeField] private ShipController ship = null;
    private FlashColor shipColor = null;

    private int health = 0;
    private float score = 0.0f;

    //Could implement a powerup that when collected, the multipler goes up?
    private float multiplier = 1.4f;

    private void OnEnable()
    {
        EventAnnouncer.OnRequestPlayerInvincible += InvincibleRequest;
        shipColor = ship.GetComponent<FlashColor>();
    }

    private void OnDisable()
    {
        EventAnnouncer.OnRequestPlayerInvincible -= InvincibleRequest;
    }

    private void Start()
    {
        InitGame();
    }

    private void InitGame()
    {
        IsRunning = true;
        health = 100;
        score = 0;
    }

    private void Update()
    {
        if (IsRunning)
        {
            IntegrateGame();
        }
    }

    private void IntegrateGame()
    {
        score += Time.deltaTime * multiplier;
        scoreText.text = score.ToString("n2") + "m";

        FishPool.Instance.SetMax((int)Mathf.Clamp(score * 0.1f, 0, 30));
    }

    private void StopIntegrating()
    {
        IsRunning = false;
    }

    public void DamagePlayer(int damage)
    {
        if (IsInvincible)
        {
            return;
        }

        health -= damage;

        EventAnnouncer.OnPlayerDamaged?.Invoke(damage, health);

        float? f = EventAnnouncer.OnRequestPlayerInvincible?.Invoke();

        if (f.HasValue)
        {
            HandleInvincibleRequest(f.Value);
        }

        if (health <= 0)
        {
            StopIntegrating();
            EventAnnouncer.OnPlayerDied?.Invoke(score);
            EventAnnouncer.OnRequestSceneChange?.Invoke(EnumScene.RESULTS, new TransitionEffect(5.0f, Color.black));
        }
    }

    private void HandleInvincibleRequest(float duration)
    {
        if (!IsInvincible && invincibleCoroutine == null)
        {
            invincibleCoroutine = StartCoroutine(MakeInvincible(duration));
        }
    }

    private IEnumerator MakeInvincible(float duration)
    {
        IsInvincible = true;
        yield return new WaitForSeconds(duration);
        IsInvincible = false;
        invincibleCoroutine = null;
    }

    private float InvincibleRequest()
    {
        return shipColor.Activate();
    }
}
