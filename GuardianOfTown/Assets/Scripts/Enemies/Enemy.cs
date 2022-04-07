using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class Enemy : Character
{
    private FillEnemyHealthBar fillEnemyHealthBar;
    private ParticleSystem criticalHitParticleSystem;
    protected DataPersistantManager DataPersistentManager { get; set; }
    protected int Exp { get; set; }
    protected PlayerController Player { get; set; }

    protected virtual void Start()
    {
        Player = FindObjectOfType<PlayerController>();
        DataPersistentManager = FindObjectOfType<DataPersistantManager>();
        fillEnemyHealthBar = GetComponentInChildren<FillEnemyHealthBar>();
        criticalHitParticleSystem = GetComponentInChildren<ParticleSystem>();
        fillEnemyHealthBar.slider.gameObject.SetActive(false);
    }
    protected void Trigger (Collider other, GameObject floatingTextPrefab, GameObject criticalHitPrefab)
    {
        if (other.CompareTag(Tags.Bullet))
        {
            var damage = 0;
            var critical = false;

            if(Player.IsCritical())
            {
                criticalHitParticleSystem.Play();
                GameManager.SharedInstance.ShakeCamera();
                critical = true;
            }

            damage = Player.Damage - (Defense / 2);
            other.gameObject.SetActive(false);
            ObjectPooler.ProjectileCount++;
            GameManager.SharedInstance.projectileText.text = "Projectile: " + ObjectPooler.ProjectileCount;
            ShowDamage(critical, damage, floatingTextPrefab, criticalHitPrefab);
            ReceiveDamage(damage);
            fillEnemyHealthBar.slider.gameObject.SetActive(true);
            fillEnemyHealthBar.FillEnemySliderValue();
            Debug.Log("ouch, it hurts" + HP);

            if (HP <= 0)
            {
                Player.Exp += Exp;
                Player.LevelUp();
                Die();
            }
        }
        else if (other.CompareTag(Tags.Wall))
        {
            Die();
            Player.TownReceiveDamage(); 
        }
        else if (other.CompareTag(Tags.Player))
        {
            Player.ReceiveDamage(Attack - (Player.Defense / 2));
            Player.ComprobateLifeRemaining();

            if (!Player.IsDead)
            {
                Player.shieldParticleSystem.Play();
                Player.Exp += Exp;
                if (Player.Exp > 20)
                {
                    Player.LevelUp();
                }
                Die();
            }
        }
    }

    public override void Die()
    {
        if (this.CompareTag(Tags.Boss))
        {
            if (Player.Exp > 20)
            {
                Player.LevelUp();
            }
            DataPersistentManager.ChangeStage();
        }
        Destroy(gameObject);
    }

    public override void Move()
    {
        transform.position += Vector3.back * Time.deltaTime * Speed;
    }

    public override void LevelUp()
    {
        for (int i = 0; i < Level * 2; i++)
        {
            HpMax += 10;
            HP = HpMax;
            var randomUpgrade = Random.Range(0, 2);
            switch (randomUpgrade)
            {
                case 0: Attack += 5; break;
                case 1: Defense += 4; break;
            }
        }
    }

    void ShowDamage(bool isCritical, int damage, GameObject floatingTextPrefab, GameObject criticalHitPrefab)
    {
        GameObject prefab = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        prefab.GetComponentInChildren<TextMesh>().text = damage.ToString();
        if (isCritical)
        {
            Instantiate(criticalHitPrefab, transform.position, Quaternion.identity);
            prefab.GetComponentInChildren<TextMesh>().color = Color.yellow;
        }
    }
}
