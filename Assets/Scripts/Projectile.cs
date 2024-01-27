using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody projectileRB;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private float explosionDelay = 0.1f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private Vector3 explosionParticleOffset = new Vector3(0,1,0);
    [SerializeField] private GameObject audioSourcePrefab;
    [SerializeField] private AudioClip impactSound;

    private bool hasExploded = false;

    public void Explode()
    {
        var effect = Instantiate(hitEffect, transform.position + explosionParticleOffset, Quaternion.identity);
        Destroy(effect, 1f);
        PlaySoundAtPosition(impactSound);
        Destroy(this.gameObject);
    }

    public void PlaySoundAtPosition(AudioClip clip)
    {
        var audioSource = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
        var instantiatedAudiosource = audioSource.GetComponent<AudioSource>();
        instantiatedAudiosource.clip = clip;
        instantiatedAudiosource.spatialBlend = 1;
        instantiatedAudiosource.Play();

        Destroy(audioSource, instantiatedAudiosource.clip.length);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag != "Player")
        {
            if (!hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }
}
