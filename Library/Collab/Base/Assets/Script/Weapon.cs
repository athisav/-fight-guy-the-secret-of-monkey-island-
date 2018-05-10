using UnityEngine;
using UnityEngine.Networking;

public class Weapon : NetworkBehaviour {
    public GameObject bulletPrefab;

	// Where the bullet spawns with respect to the weapon
	public Vector3 bulletSpawnOffset;

    // In seconds
    public float reloadRate;
    public float attackRate;
    public float initialAttackDelay;
    // Set clip size to -1 for infinite size
    public int maxClipSize;

    private bool isAttacking;
    private bool isReloading;

    private float timeUntilNextAttack;
    private float timeUntilReloadFinishes;
    private int currentClipSize;

	private NetworkBehaviour parentPlayerNetworkBehaviour;

    void Start() 
	{
		// TODO+ attach transform to local player gameobject
    }

	[Command]
    void CmdAttack()
    {
        // Reset next attack time
        timeUntilNextAttack = attackRate;

        currentClipSize--;


		Vector3 playerPos = transform.position;
		Vector3 playerDirection = transform.forward;
		Quaternion playerRotation = transform.rotation;
		float spawnDistance = 50;

		Vector3 spawnPos = playerPos + playerDirection*spawnDistance;

		GameObject bullet = Instantiate(bulletPrefab, spawnPos, playerRotation);        
		NetworkServer.Spawn(bullet);
	}

	[Command]
    void CmdBeginReload()
    {
        timeUntilReloadFinishes = reloadRate;
        isReloading = true;
    }

	[Command]
    void CmdFinishReload()
    {
        currentClipSize = maxClipSize;

        // Stop reloading when reload finishes
        isReloading = false;
    }

    void Update()
    {
		/*
        if (Input.GetMouseButtonUp(0))
        {
            isAttacking = false;
            return;
        }*/


        if (Input.GetMouseButtonDown(0))
        {
            //timeUntilNextAttack = initialAttackDelay + attackRate;
            //isAttacking = true;
			CmdAttack();
        }

		/*
        if (Input.GetButtonDown("Reload1"))
        {
            CmdBeginReload();
        }

        float deltaTime = Time.deltaTime;
        if (isReloading)
        {
            timeUntilReloadFinishes -= deltaTime;
            if (timeUntilReloadFinishes <= 0)
            {
                CmdFinishReload();
            }
        }
        else if (isAttacking)
        {
            if (currentClipSize == 0)
            {
                CmdBeginReload();
            }
            else
            {
                timeUntilNextAttack -= deltaTime;
                if (timeUntilNextAttack <= 0)
                {
                    CmdAttack();
                }
            }
        }*/

    }
}
