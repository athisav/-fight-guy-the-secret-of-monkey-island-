using System;

public class Weapon : MonoBehavior {
    public GameObject bullet;

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

    void Start()
    {

    }

    void Attack()
    {
        // Reset next attack time
        timeUntilNextAttack = attackRate;

        currentClipSize--;

        // TODO: Spawn bullet
        // Instantiate bullet
        // Set bullet component such that it is travelling
        // in same direction as parent transform
    }

    void BeginReload()
    {
        timeUntilReloadFinishes = reloadRate;
        isReloading = true;
    }

    void FinishReload()
    {
        currentClipSize = maxClipSize;

        // Stop reloading when reload finishes
        isReloading = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isAttacking = false;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            timeUntilNextAttack = initialAttackDelay + attackRate;
            isAttacking = true;
        }

        if (Input.GetButtonDown("Reload1"))
        {
            BeginReload();
        }

        float deltaTime = Time.deltaTime;
        if (isReloading)
        {
            timeUntilReloadFinishes -= deltaTime;
            if (timeUntilReloadFinishes <= 0)
            {
                FinishReload();
            }
        }
        else if (isAttacking)
        {
            if (currentClipSize == 0)
            {
                BeginReload();
            }
            else
            {
                timeUntilNextAttack -= deltaTime;
                if (timeUntilNextAttack <= 0)
                {
                    Attack();
                }
            }
        }

    }
}
