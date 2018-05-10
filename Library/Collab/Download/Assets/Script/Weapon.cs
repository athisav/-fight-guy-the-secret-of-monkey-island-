using UnityEngine;
using UnityEngine.Networking;

public class Weapon : NetworkBehaviour {
    public GameObject bulletPrefab;

	[Tooltip("Where the bullet spawns with respect to the weapon, assuming the weapon is rotated 0 degrees")]
	public Vector3 bulletSpawnOffset;
	[Tooltip("Where the weapon is with respect to the player, assuming the player is rotated 0 degrees")]
	public Vector3 weaponOffset;

	[Tooltip("In seconds")]
    public float reloadRate;
    public float attackRate;
    public float initialAttackDelay;
	[Tooltip("Set clip size to -1 for infinite size")]
    public int maxClipSize;
	public int initialClipSize;

    private bool isAttacking;
    private bool isReloading;

    private float timeUntilNextAttack;
    private float timeUntilReloadFinishes;
    private int currentClipSize;

	private NetworkBehaviour parentPlayerNetworkBehaviour;

    void Start() 
	{
		currentClipSize = initialClipSize;

		// Infinite ammo
		if (maxClipSize == -1) {
			currentClipSize = -1;
		}

		transform.localPosition = weaponOffset;
    }

	[Command]
    void CmdAttack()
    {
        // Reset next attack time
        timeUntilNextAttack = attackRate;

        currentClipSize--;

		Vector3 gunPos = transform.position;
		Vector3 playerDirection = transform.parent.forward;
		Quaternion playerRotation = transform.parent.rotation;

		Vector3 rotatedBulletSpawnOffset = playerRotation * bulletSpawnOffset;
		Vector3 spawnPos = gunPos + rotatedBulletSpawnOffset;

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
		
        if (Input.GetButtonUp("Fire1"))
        {
            isAttacking = false;
            return;
        }

		if (Input.GetButtonDown("Fire1"))
        {
			timeUntilNextAttack += initialAttackDelay;
            isAttacking = true;
        }
			
        if (Input.GetButtonDown("Reload1"))
        {
            CmdBeginReload();
        }


		float deltaTime = Time.deltaTime;

		// Decrease time until next attack if nothing is held
		if (!Input.GetButton("Fire1") && !Input.GetButton("Reload1")) {
			timeUntilNextAttack -= deltaTime;
			if (timeUntilNextAttack < 0) {
				timeUntilNextAttack = 0;
			}
		}

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
        }
    }
}
