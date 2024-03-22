using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

public class NewPlayerController : Controller
{
    private PlayerControls _controls;

    private PlayerState _currentState;

    //[SerializeField] private Transform mesh;

    [SerializeField] private HealthDisplay healthDisplay;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 500f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 1000f;
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private GameObject dashEffect;
    private float _dashCooldownTimer;

    [Header("Laser")]
    [SerializeField] private Transform laserFirePoint;
    [SerializeField] private float laserMaxDistance = 10f;
    [SerializeField] private float laserWidth;
    [SerializeField] private Laser laserPrefab;
    [SerializeField] private LayerMask healthSystemLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private int laserDamage = 15;
    [SerializeField] private float laserCooldown = 3f;
    [SerializeField] private Transform aim;
    [SerializeField] private GameObject laserOriginEffect;
    private float _laserCooldownTimer;
    private Vector2 _lookDirection;
    private Vector2 _mousePosition;

    [Header("Weapons")]
    [SerializeField] private List<Weapon> weapons;

    [Header("Fall")]
    [SerializeField] private GameObject fallSpritePrefab;
    [SerializeField] private GameObject fallDownSpritePrefab;
    [SerializeField] private GameObject landMeshPrefab;
    [SerializeField] private GameObject landEffect;
    [SerializeField] private float landEffectLifetime = 0.3f;
    [SerializeField] private int landDamage = 3;
    [SerializeField] private float landDamageRadius = 2.25f;
    [SerializeField] private float landCameraShakeGain;
    [SerializeField] private float landCameraShakeTime;
    private GameObject _fallSprite;
    private GameObject _fallDownSprite;
    private GameObject _landMesh;

    public event Action OnDash;
    public event Action OnTakeDamage;

    public PlayerControls Controls { get { return _controls; } }
    //public Transform Mesh { get { return mesh; } }
    public float MovementSpeed { get { return movementSpeed; } }
    public float DashSpeed { get { return dashSpeed; } }
    public float DashDistance { get { return dashDistance; } }
    public GameObject DashEffect { get { return dashEffect; } }
    public LayerMask HealthSystemLayer { get { return healthSystemLayer; } }
    public Vector2 LookDirection { get { return _lookDirection; } }
    public List<Weapon> Weapons { get { return weapons; } }
    public GameObject FallSprite { get { return _fallSprite; } }
    public GameObject FallDownSprite { get { return _fallDownSprite; } }
    public GameObject LandMesh { get { return _landMesh; } }
    public GameObject LandEffect { get { return landEffect; } }
    public float LandEffectLifetime { get { return landEffectLifetime; } }
    public int LandDamage { get { return landDamage; } }
    public float LandDamageRadius { get { return landDamageRadius; } }
    public float LandCameraShakeGain { get { return landCameraShakeGain; } }
    public float LandCameraShakeTime { get { return landCameraShakeTime; } }

    private void Update()
    {
        UpdateLogic();
    }

    private void FixedUpdate()
    {
        UpdatePhysics();
    }

    public override void Initialize(ScreenManager screenManager)
    {
        base.Initialize(screenManager);

        _controls = new PlayerControls();
        _controls.InGame.Enable();
        _controls.InGame.Dash.performed += ctx => Dash();
        _controls.InGame.Laser.performed += ctx => FireLaser();

        HealthSystem.OnHit += TakeHit;
        //HealthSystem.OnDamage += TakeDamage;
        HealthSystem.OnDeath += Die;
        healthDisplay.Initialize(HealthSystem);

        _dashCooldownTimer = 0f;
        _laserCooldownTimer = 0f;

        foreach (Weapon weapon in weapons)
        {
            weapon.Initialize(this, HealthSystem);
        }

        _fallSprite = Instantiate(fallSpritePrefab);
        _fallSprite.SetActive(false);
        _fallDownSprite = Instantiate(fallDownSpritePrefab);
        _fallDownSprite.SetActive(false);
        _landMesh = Instantiate(landMeshPrefab);
        _landMesh.SetActive(false);
    }

    public void ChangeState(PlayerState nextState)
    {
        _currentState?.Exit();
        _currentState = nextState;
        _currentState.Enter();
    }

    private void Dash()
    {
        if (_dashCooldownTimer > 0f) return;

        OnDash?.Invoke();
        _dashCooldownTimer = dashCooldown;
    }

    private void FireLaser()
    {
        if (_laserCooldownTimer > 0f) return;

        //Physics
        Ray2D ray = new(laserFirePoint.position, _mousePosition - (Vector2)laserFirePoint.position);
        float rayDistance = laserMaxDistance;
        //Checks obstacles
        RaycastHit2D obstacleHit = Physics2D.Raycast(ray.origin, ray.direction, rayDistance, obstacleLayer);
        if (obstacleHit.collider != null)
        {
            rayDistance = obstacleHit.distance;
            if (obstacleHit.collider.TryGetComponent(out Switch s))
            {
                s.Activate();
            }
        }
        //Creates the damaging ray
        RaycastHit2D[] hits = Physics2D.BoxCastAll(ray.origin, new Vector2(laserWidth, 1), aim.rotation.eulerAngles.y, ray.direction, rayDistance, healthSystemLayer);
        foreach (RaycastHit2D enemyHit in hits)
        {
            if (enemyHit.collider.TryGetComponent(out HealthSystem healthSystem))
            {
                if (healthSystem.Source != transform)
                {
                    healthSystem.TakeDamage(laserDamage, transform);
                }
            }
        }

        ////Visuals
        Laser newLaser = Instantiate(laserPrefab);
        newLaser.Initialize((Vector2)laserFirePoint.position + _lookDirection.normalized * 0.75f, (Vector2)laserFirePoint.position + (_mousePosition - (Vector2)laserFirePoint.position).normalized * rayDistance, laserWidth);

        InstantiateEffect(laserOriginEffect, laserFirePoint.position, Quaternion.LookRotation(Vector3.forward, _lookDirection), 0.367f);

        CameraManager.Instance.ShakeCamera(2f, 0.1f);

        _laserCooldownTimer = laserCooldown;
    }

    private void TakeHit(Transform damageSource)
    {
        OnTakeDamage?.Invoke();
    }

    public void Die(HealthSystem healthSystem, Transform deathSource)
    {
        _controls.InGame.Disable();
        Mesh.gameObject.SetActive(false);

        ScreenManager.PlayerDied();
    }

    public void InstantiateEffect(GameObject effect, Vector2 position, Quaternion rotation, float time)
    {
        GameObject newEffect = Instantiate(effect, position, rotation);
        Destroy(newEffect, time);
    }

    //private void TakeDamage(int amount)
    //{
    //    GeneralAnimator.SetTrigger("Squish");
    //    ChangeColor();
    //}

    //private void ChangeColor()
    //{
    //    foreach (SkinnedMeshRenderer renderer in MeshRenderers)
    //    {
    //        renderer.material.SetColor("_Dark_Color", damageColor);
    //    }

    //    _changeColorTimer = changeColorTime;
    //}

    public override bool SwordAttack()
    {
        return _currentState.CanAttackSword();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        _currentState.UpdateLogic();

        HandleRotationInput();

        if (_dashCooldownTimer > 0f)
        {
            _dashCooldownTimer -= Time.deltaTime;
        }

        if (_laserCooldownTimer > 0f)
        {
            _laserCooldownTimer -= Time.deltaTime;
        }
    }

    private void HandleRotationInput()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(_controls.InGame.MousePosition.ReadValue<Vector2>());
        _lookDirection = _mousePosition - (Vector2)laserFirePoint.localPosition - (Vector2)transform.position;
    }

    public void RotateAim()
    {
        Quaternion aimRotation = Quaternion.LookRotation(new Vector3(_lookDirection.x, 0f, _lookDirection.y), aim.up);
        aim.localRotation = Quaternion.Euler(new Vector3(0f, aimRotation.eulerAngles.y, 0f));
    }

    public void RotateMesh()
    {
        Quaternion meshRotation = Quaternion.LookRotation(new Vector3(_lookDirection.x, 0f, _lookDirection.y), Mesh.up);
        Mesh.localRotation = Quaternion.Euler(new Vector3(0f, meshRotation.eulerAngles.y, 0f));
    }

    public void RotateMeshSmooth(Vector3 direction, float speed)
    {
        Quaternion meshRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.y), Mesh.up);
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, meshRotation.eulerAngles.y, 0f));
        Mesh.localRotation = Quaternion.RotateTowards(Mesh.localRotation, targetRotation, speed * Time.deltaTime);
    }

    public override void UpdatePhysics()
    {
        _currentState.UpdatePhysics();

        foreach (Weapon weapon in weapons)
        {
            weapon.UpdatePhysics();
        }
    }

    public void Move(Vector2 direction, float speed)
    {
        Rb.velocity = speed * Time.fixedDeltaTime * direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _currentState.OnCollisionEnter(collision);
    }
}
