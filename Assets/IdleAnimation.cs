using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IdleAnimation : MonoBehaviour
{
    public GameObject[] prefabs; // Array of prefabs children of the empty GameObject
    public int minSimultaneousJumpAmount = 10; // Minimum number of prefabs jumping simultaneously
    public int maxSimultaneousJumpAmount = 20; // Maximum number of prefabs jumping simultaneously
    public float minIntervalBetweenJumps = 0.5f; // Minimum interval between each group of jumps
    public float maxIntervalBetweenJumps = 2.0f; // Maximum interval between each group of jumps
    private Animator waveAnimator; // Animator of the wave animation
    public float minHeight = 1.0f; // Minimum jump height
    public float maxHeight = 3.0f; // Maximum jump height

    private List<Transform> jumpingPrefabs = new List<Transform>(); // List of prefabs currently jumping

    void Start()
    {
        waveAnimator = GetComponent<Animator>();
        // Disable the wave Animator at the start
        //  waveAnimator.enabled = false;

        // Start the jump animation in groups after the wave animation finishes
        StartCoroutine(StartJumpAnimationAfterWave());
    }

    IEnumerator StartJumpAnimationAfterWave()
    {
        // Wait until the wave animation has finished
        yield return new WaitForSeconds(waveAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Disable the wave Animator
        waveAnimator.enabled = false;

        // Start the jump animation in groups
        StartCoroutine(JumpInGroups());
    }

    IEnumerator JumpInGroups()
    {
        while (true)
        {
            // Get a random number of prefabs to jump simultaneously
            int simultaneousJumpAmount = Random.Range(minSimultaneousJumpAmount, maxSimultaneousJumpAmount + 1);

            // Get a random interval between jumps
            float intervalBetweenJumps = Random.Range(minIntervalBetweenJumps, maxIntervalBetweenJumps);

            // Get a random list of prefabs to jump simultaneously
            List<GameObject> prefabsToJump = GetRandomPrefabs(simultaneousJumpAmount);

            // Start the jump animation on the selected prefabs
            foreach (GameObject prefab in prefabsToJump)
            {
                // Use minHeight and maxHeight for random jump height
                StartCoroutine(Jump(prefab.transform, Random.Range(minHeight, maxHeight), Random.Range(0.1f, 1.0f)));
            }

            // Wait for the interval between jumps
            yield return new WaitForSeconds(intervalBetweenJumps);
        }
    }

    List<GameObject> GetRandomPrefabs(int amount)
    {
        List<GameObject> randomPrefabs = new List<GameObject>();

        // Shuffle the array of prefabs
        List<GameObject> shuffledPrefabs = new List<GameObject>(prefabs);
        for (int i = 0; i < shuffledPrefabs.Count; i++)
        {
            GameObject temp = shuffledPrefabs[i];
            int randomIndex = Random.Range(i, shuffledPrefabs.Count);
            shuffledPrefabs[i] = shuffledPrefabs[randomIndex];
            shuffledPrefabs[randomIndex] = temp;
        }

        // Select the specified amount of random prefabs
        for (int i = 0; i < Mathf.Min(amount, shuffledPrefabs.Count); i++)
        {
            randomPrefabs.Add(shuffledPrefabs[i]);
        }

        return randomPrefabs;
    }

    IEnumerator Jump(Transform prefabTransform, float height, float duration)
    {
        Vector3 initialPosition = prefabTransform.position;
        Vector3 targetPosition = initialPosition + Vector3.up * height;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            prefabTransform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            yield return null;
        }

        // Ensure the prefab is exactly at the target position
        prefabTransform.position = targetPosition;

        // Wait for a frame before returning the prefab to its initial position
        yield return null;

        // Return the prefab to its initial position
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            prefabTransform.position = Vector3.Lerp(targetPosition, initialPosition, t);
            yield return null;
        }

        // Ensure the prefab is exactly at its initial position
        prefabTransform.position = initialPosition;
    }

    // Method to activate the wave animation
    public void ActivateWaveAnimation()
    {
        // Activate the wave Animator
        waveAnimator.enabled = true;
    }
}
