using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public float attackCooldown = 0.5f;

    private Animator playerAnimator;
    private bool canAttack = true;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && canAttack)
        {
            playerAnimator.SetBool("IsAttacking", true);
            canAttack = false;
            Invoke(nameof(ResetAttackCooldown), attackCooldown);
        }
        else if (!Input.GetMouseButton(0))
        {
            playerAnimator.SetBool("IsAttacking", false);
        }
    }

    private void ResetAttackCooldown()
    {
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
