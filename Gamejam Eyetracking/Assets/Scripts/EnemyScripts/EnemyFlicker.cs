using UnityEngine;

public class EnemyFlicker : MonoBehaviour
{
    [Header("Flicker Settings")]
    public float visibleDuration = 2f;
    public float invisibleDuration = 2f;

    [Header("References")]
    public GameObject enemyVisual; // Assign your enemy GameObject here (must have Collider2D set as trigger)
    public Transform player;       // Assign your player GameObject here

    private void Start()
    {
        if (enemyVisual == null)
        {
            Debug.LogError("EnemyFlickerRespawn: enemyVisual not assigned!");
            enabled = false;
            return;
        }
        StartCoroutine(FlickerLoop());
    }

    private System.Collections.IEnumerator FlickerLoop()
    {
        while (true)
        {
            // Enable enemy (visible)
            enemyVisual.SetActive(true);
            yield return new WaitForSeconds(visibleDuration);

            // Disable enemy (invisible)
            enemyVisual.SetActive(false);
            yield return new WaitForSeconds(invisibleDuration);
        }
    }
}