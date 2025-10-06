using UnityEngine;

public class EchoShooter : MonoBehaviour
{
    public GameObject echoProjectilePrefab;
    public MicInputLevel micInput;

    [Header("Shooting Settings")]
    public float minLoudness = 10f;
    public float minShootInterval = 0.12f;
    public float maxShootInterval = 0.02f;

    [Header("Projectile Speed")]
    public float baseSpeed = 10f;
    public float maxSpeed = 50f;

    [Header("Smoothing")]
    public float smoothingSpeed = 5f;

    private float shootTimer = 0f;
    private float smoothedLoudness = 0f;

    void Update()
    {
        if (micInput == null || echoProjectilePrefab == null || Camera.main == null)
            return;

        smoothedLoudness = Mathf.Lerp(smoothedLoudness, micInput.loudness, Time.deltaTime * smoothingSpeed);

        if (smoothedLoudness >= minLoudness)
        {
            float t = Mathf.InverseLerp(minLoudness, 100f, smoothedLoudness);
            float currentInterval = Mathf.Lerp(minShootInterval, maxShootInterval, t);

            shootTimer += Time.deltaTime;
            if (shootTimer >= currentInterval)
            {
                ShootEcho(t);
                shootTimer = 0f;
            }
        }
        else
        {
            shootTimer = 0f;
        }
    }

    void ShootEcho(float loudnessT)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDir = (mouseWorldPos - transform.position);
        shootDir.Normalize();

        float speed = Mathf.Lerp(baseSpeed, maxSpeed, loudnessT);

        // PITCH -> kleur (laag = blauw, hoog = rood)
        float pitchT = micInput.GetNormalizedPitch();
        Color c = Color.Lerp(Color.blue, Color.red, pitchT);
        // (Alternatief regenboog: float hue = Mathf.Lerp(0.66f, 0f, pitchT); Color c = Color.HSVToRGB(hue,1f,1f);)

        GameObject projectile = Instantiate(echoProjectilePrefab, transform.position, Quaternion.identity);
        EchoProjectile echo = projectile.GetComponent<EchoProjectile>();
        if (echo != null)
        {
            echo.Initialize(shootDir, speed);
            echo.SetColor(c);
        }
    }
}