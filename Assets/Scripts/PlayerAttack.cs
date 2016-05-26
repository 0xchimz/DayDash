using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	public int damagePerShot = 20;                  // The damage inflicted by each bullet.
	public float timeBetweenCasting = 0.15f;        // The time between each shot.
	public float range = 100f;                      // The distance the gun can fire.

	float timer;                                    // A timer to determine when to fire.
	Ray shootRay;                                   // A ray from the gun end forwards.
	RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
	int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
	ParticleSystem gunParticles;                    // Reference to the particle system.
	LineRenderer gunLine;                           // Reference to the line renderer.
	AudioSource gunAudio;                           // Reference to the audio source.
	Light gunLight;                                 // Reference to the light component.
	float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenCasting that the effects will display for.

	public GameObject magic;

	void Awake ()
	{
		// Create a layer mask for the Shootable layer.
		shootableMask = LayerMask.GetMask ("Shootable");

		// Set up the references.
		//gunParticles = GetComponent<ParticleSystem> ();
		//gunLine = GetComponent <LineRenderer> ();
		//gunAudio = GetComponent<AudioSource> ();
		//gunLight = GetComponent<Light> ();
	}

	void Update ()
	{
		// Add the time since Update was last called to the timer.
		timer += Time.deltaTime;

		// If the Fire1 button is being press and it's time to fire...
		if(Input.GetButton ("Fire1") && timer >= timeBetweenCasting)
		{
			// ... shoot the gun.
			Cast ();
		}

		// If the timer has exceeded the proportion of timeBetweenCasting that the effects should be displayed for...
		if(timer >= timeBetweenCasting * effectsDisplayTime)
		{
			// ... disable the effects.
			DisableEffects ();
		}
	}

	public void DisableEffects ()
	{
		// Disable the line renderer and the light.
		//gunLine.enabled = false;
		//gunLight.enabled = false;
	}

	void Cast ()
	{
		// Reset the timer.
		timer = 0f;


	}
}