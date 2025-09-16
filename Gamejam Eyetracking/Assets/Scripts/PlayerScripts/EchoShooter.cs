using UnityEngine;

public class EchoShooter : MonoBehaviour
{
    public GameObject echoProjectilePrefab;
    public MicInputLevel micInput;
    public float minLoudness = 20f;
    public float shootInterval = 1f;
    public float baseSpeed = 5f;
    public float maxSpeed = 20f;
    public float loudnessToSpeedMultiplier = 25f;

    private float shootTimer = 0f;
    private bool isMakingNoise = false;

    void Update()
    {
        if (micInput == null)
        {
            Debug.LogError("MicInputLevel is not assigned in EchoShooter!");
            return;
        }
        if (echoProjectilePrefab == null)
        {
            Debug.LogError("EchoProjectilePrefab is not assigned in EchoShooter!");
            return;
        }
        if (Camera.main == null)
        {
            Debug.LogError("No camera with tag 'MainCamera' found!");
            return;
        }

        if (micInput.loudness >= minLoudness)
        {
            isMakingNoise = true;
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                ShootEcho();
                shootTimer = 0f;
            }
        }
        else
        {
            if (isMakingNoise)
            {
                ShootEcho();
            }
            isMakingNoise = false;
            shootTimer = 0f;
        }
    }

    void ShootEcho()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDir = mouseWorldPos - transform.position;
        float speed = Mathf.Clamp(baseSpeed + micInput.loudness * loudnessToSpeedMultiplier, baseSpeed, maxSpeed);

        GameObject projectile = Instantiate(echoProjectilePrefab, transform.position, Quaternion.identity);
        EchoProjectile echo = projectile.GetComponent<EchoProjectile>();
        if (echo == null)
        {
            Debug.LogError("EchoProjectile component not found on prefab!");
            return;
        }
        echo.Initialize(shootDir, speed);
    }
}