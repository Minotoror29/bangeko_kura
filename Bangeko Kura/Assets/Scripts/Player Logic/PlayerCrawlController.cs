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

    public override void RotateMesh()
    {
        base.RotateMesh();

        Quaternion meshRotation = Quaternion.LookRotation(new Vector3(LookDirection.x, 0f, LookDirection.y), Mesh.up);
        HealthSystem.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, -meshRotation.eulerAngles.y));
    }

    public override void RotateMeshSmooth(Vector3 direction, float speed)
    {
        base.RotateMeshSmooth(direction, speed);

        Quaternion meshRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.y), Mesh.up);
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, -meshRotation.eulerAngles.y));
        HealthSystem.transform.localRotation = Quaternion.RotateTowards(HealthSystem.transform.localRotation, targetRotation, speed * Time.deltaTime);
    }
}
