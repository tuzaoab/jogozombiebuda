using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    public Character Character;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    public int maxAmmo = 16;
    public int currentAmmo;

    public float recoilAngle = 15f;
    public float recoilDistance = 0.1f;
    public float recoilSpeed = 10f;

    public float spinDuration = 0.166f;
    public int spinTurns = 3;
    public TrailRenderer trail;

    public TMP_Text ammoText;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private int shotCount = 0;
    private bool spinning = false;
    private float spinElapsed = 0f;

    private bool gunVisible = true;

    public Animator animator;
    private SpriteRenderer[] renderers;

    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;

        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        if (trail != null)
        {
            trail.emitting = false;
            trail.Clear();
        }

        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (Character != null && Character.isGameOver)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleGunVisibility();
        }

        if (!gunVisible)
            return;

        if (currentAmmo <= 0 && !spinning)
        {
            Reload();
        }

        if (Input.GetMouseButtonDown(0) && !spinning && currentAmmo > 0)
        {
            Shoot();
            ApplyRecoil();
            currentAmmo--;
            UpdateAmmoUI();

            shotCount++;
            if (shotCount >= 20)
            {
                StartSpin();
            }
        }

        if (Input.GetKey(KeyCode.R) && !spinning)
        {
            Reload();
        }

        if (spinning)
        {
            spinElapsed += Time.deltaTime;
            float t = spinElapsed / spinDuration;
            float zRotation = Mathf.Lerp(0, 360f * spinTurns, t);
            transform.localRotation = originalRotation * Quaternion.Euler(0, 0, zRotation);

            if (spinElapsed >= spinDuration)
            {
                EndSpin();
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * recoilSpeed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, Time.deltaTime * recoilSpeed);
        }
    }

    void Shoot()
    {
        if (animator != null)
            animator.SetTrigger("Shoot");

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.transform.localScale = bulletPrefab.transform.localScale;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = new Vector2(Mathf.Sign(transform.parent.localScale.x), 0);
            rb.velocity = direction * bulletSpeed;
        }

        Destroy(bullet, 3f);
    }

    void ApplyRecoil()
    {
        transform.localRotation = Quaternion.Euler(0, 0, recoilAngle);
        transform.localPosition -= new Vector3(0, 0, recoilDistance);
    }

    void StartSpin()
    {
        spinning = true;
        spinElapsed = 0f;
        shotCount = 0;

        if (trail != null)
        {
            trail.Clear();
            trail.emitting = true;
        }
    }

    void EndSpin()
    {
        spinning = false;
        transform.localRotation = originalRotation;

        if (trail != null)
        {
            trail.emitting = false;
            trail.Clear();
        }
    }

    void Reload()
    {
        if (!spinning)
        {
            StartSpin();
            currentAmmo = maxAmmo;
            UpdateAmmoUI();
        }
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + " / " + maxAmmo;
        }
    }

    void ToggleGunVisibility()
    {
        gunVisible = !gunVisible;

        foreach (SpriteRenderer r in renderers)
        {
            r.enabled = gunVisible;
        }

        if (trail != null)
        {
            trail.emitting = gunVisible;
            if (!gunVisible)
                trail.Clear();
        }

        if (ammoText != null)
            ammoText.enabled = gunVisible;
    }
}
