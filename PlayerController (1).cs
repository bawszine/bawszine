using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Dash Settings")]
    public bool canDash = false; // لا يمكنه الداش حتى يأخذ الماسك
    public float dashForce = 24f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing;
    private bool dashAvailable = true;

    [Header("Combat Settings")]
    public float baseDamage = 10f;
    public float damageMultiplier = 1f;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    private Rigidbody2D rb;
    private float moveInput;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isDashing) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = Vector2.up * jumpForce;
        }

        // تفعيل الداش بحرف Shift
        if (canDash && Input.GetKeyDown(KeyCode.LeftShift) && dashAvailable)
        {
            StartCoroutine(PerformDash());
        }

        // تجربة الضرب (مثلاً بالضغط على زر الفأرة الأيسر)
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private IEnumerator PerformDash()
    {
        dashAvailable = false;
        isDashing = true;
        
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        
        // تحديد اتجاه الداش
        float dashDir = moveInput != 0 ? moveInput : transform.localScale.x;
        rb.velocity = new Vector2(dashDir * dashForce, 0f);

        // تأثير بصري بسيط (تغيير الشفافية أثناء الداش)
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);

        yield return new WaitForSeconds(dashDuration);

        spriteRenderer.color = originalColor;
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        dashAvailable = true;
    }

    void Attack()
    {
        // حساب الضرر الحالي
        float finalDamage = baseDamage * damageMultiplier;
        Debug.Log("Attacking! Damage dealt: " + finalDamage);

        // منطق ضرب الأعداء (اختياري إذا كان عندك أعداء)
        /*
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            // enemy.GetComponent<Enemy>().TakeDamage(finalDamage);
        }
        */
    }

    public void EnableDash()
    {
        canDash = true;
        Debug.Log("DASH UNLOCKED! Press Left Shift to use.");
    }

    public void IncreaseDamage(float amount)
    {
        damageMultiplier += amount;
        Debug.Log("DAMAGE INCREASED! Current Multiplier: " + damageMultiplier);
        
        // تأثير بصري عند أخذ ماسك الضرر (تغيير اللون للأحمر قليلاً)
        StartCoroutine(FlashColor(Color.red));
    }

    private IEnumerator FlashColor(Color color)
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = color;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
