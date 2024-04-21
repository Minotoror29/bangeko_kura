using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArenaManager : ScreenManager
{
    [SerializeField] private Transform spawnCursorPrefab;
    [SerializeField] private LayerMask groundMask;

    private Transform _spawnCursor;

    public override void DetermineSpawnPoint()
    {
        CurrentState = ScreenState.Spawn;
        Controls.Spawn.Enable();
        _spawnCursor = Instantiate(spawnCursorPrefab, DefaultSpawnPoint.position, Quaternion.identity);
    }

    public override void EnterScreen()
    {
        base.EnterScreen();

        Controls.Spawn.Spawn.performed += ctx => SpawnPlayer();
    }

    public override void ExitScreen(ScreenExit lastExit)
    {
        base.ExitScreen(lastExit);

        Controls.Spawn.Spawn.performed -= ctx => SpawnPlayer();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (CurrentState == ScreenState.Spawn)
        {
            Ray ray = Camera.main.ScreenPointToRay(Controls.Spawn.MousePosition.ReadValue<Vector2>());
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, groundMask);
            if (hit)
            {
                _spawnCursor.position = hit.point;
                LandPosition = hit.point;
                Ground = hit.collider.gameObject;
            }
        }
    }

    private void SpawnPlayer()
    {
        CurrentState = ScreenState.Play;
        Controls.Spawn.Disable();
        Destroy(_spawnCursor.gameObject);

        Player.ChangeState(new PlayerLandState(Player, LandPosition, Ground));
    }
}
