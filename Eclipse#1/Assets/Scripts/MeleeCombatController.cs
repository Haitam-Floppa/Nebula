using UnityEngine;
using System.Collections;

public class MeleeCombatController : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public float attackCooldown = 1f;
    public float attackDelay = 0.2f;

    private bool canAttack = true;
    private Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            StartCoroutine(AttackCooldown());
            StartCoroutine(AttackWithDelay());
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private IEnumerator AttackWithDelay()
    {
        yield return new WaitForSeconds(attackDelay);

        // Play attack animation
        playerAnimator.SetTrigger("IsAttacking");

        // Wait for the attack animation to complete
        float animationLength = playerAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        // Reset the trigger after the animation completes
        playerAnimator.ResetTrigger("IsAttacking");

        // Rest of the attack logic
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
