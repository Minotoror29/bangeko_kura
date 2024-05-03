using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerCrawlController : PlayerController
{
    [Header("Weapons")]
    [SerializeField] private TurretController turret;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        turret.Initialize(this, HealthSystem);
    }

    public override void UpdateWeapons()
    {
        base.UpdateWeapons();

        turret.UpdateLogic();
    }
}
