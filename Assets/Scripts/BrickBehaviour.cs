
using UnityEngine;

public class BrickBehaviour : MonoBehaviour
{
    public bool isBreakable;
    public BrickSpawner spawner;

    [Header("Sound Effect")]
    public AudioClip hitSound;
    [Range(0f, 1f)]
    public float volume = 1f;
    public float soundMultiplier = 5f;

    [Header("Particle Effect")]
    public GameObject particlePrefab;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball"))
            return;

        // Play the hit sound 5x the original volume setting.
        if (hitSound != null)
        {
            float finalVolume = Mathf.Clamp01(
                volume * soundMultiplier
            );

            Vector3 soundPosition = Camera.main != null
                ? Camera.main.transform.position
                : transform.position;

            AudioSource.PlayClipAtPoint(
                hitSound,
                soundPosition,
                finalVolume
            );
        }

        // Unbreakable bricks play the sound but remain.
        if (!isBreakable)
            return;

        // Spawn particles at the collision point.
        if (particlePrefab != null)
        {
            Vector3 hitPoint = transform.position;

            if (collision.contactCount > 0)
                hitPoint = collision.GetContact(0).point;

            hitPoint.z = transform.position.z;

            GameObject effect = Instantiate(
                particlePrefab,
                hitPoint,
                Quaternion.identity
            );

            ParticleSystem[] systems =
                effect.GetComponentsInChildren<ParticleSystem>(true);

            float destroyAfter = 1f;

            foreach (ParticleSystem ps in systems)
            {
                ps.Stop(
                    true,
                    ParticleSystemStopBehavior.StopEmittingAndClear
                );

                var main = ps.main;
                main.loop = false;
                main.simulationSpace =
                    ParticleSystemSimulationSpace.World;

                ps.Play(true);

                destroyAfter = Mathf.Max(
                    destroyAfter,
                    main.duration +
                    main.startLifetime.constantMax + 0.2f
                );
            }

            Destroy(effect, destroyAfter);
        }

        if (spawner != null)
            spawner.BrickDestroyed();

        Destroy(gameObject);
    }
}
