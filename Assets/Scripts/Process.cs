using UnityEngine;
using System.Collections;

public class Process : MonoBehaviour
{
    public int processId; // Unique ID for each process
    public static int N = 3; // Number of processes
    public static bool[] flags; // Flags to indicate intent
    public static int[] turn; // Turn array for Peterson's N algorithm
    public static int currentTurn = 0; // Global turn variable to control access

    private Vector3 startPosition;
    private Vector3 resourcePosition;
    private bool isInCriticalSection = false;
    private float moveSpeed = 2f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;

        GameObject resourceObj = GameObject.FindGameObjectWithTag("Resource");
        if (resourceObj != null)
        {
            resourcePosition = resourceObj.transform.position;
        }

        if (flags == null)
        {
            flags = new bool[N];
            turn = new int[N - 1];

            for (int i = 0; i < N; i++)
            {
                flags[i] = false;
            }
        }
    }

    public void RequestResource()
    {
        if (!isInCriticalSection && processId == currentTurn)
        {
            StartCoroutine(EnterCriticalSection());
        }
        else if (processId != currentTurn)
        {
            spriteRenderer.color = Color.yellow; // Waiting indicator
        }
    }

    IEnumerator EnterCriticalSection()
    {
        for (int level = 0; level < N - 1; level++)
        {
            flags[processId] = true; // Indicate intent to enter critical section
            turn[level] = processId; // Set the turn for this level

            while (ExistsHigherPriorityProcess(level))
            {
                spriteRenderer.color = Color.yellow; // Waiting indicator
                yield return null; // Wait and recheck condition
            }
        }

        // Enter critical section
        isInCriticalSection = true;
        spriteRenderer.color = Color.green; // In critical section indicator

        while (Vector3.Distance(transform.position, resourcePosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, resourcePosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(2f); // Simulate work in the critical section

        while (Vector3.Distance(transform.position, startPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        ExitCriticalSection();
    }

    private bool ExistsHigherPriorityProcess(int level)
    {
        for (int j = 0; j < N; j++)
        {
            if (j != processId && flags[j] && turn[level] == processId)
            {
                return true;
            }
        }
        return false;
    }

    private void ExitCriticalSection()
    {
        flags[processId] = false; // Clear the intent to enter critical section
        isInCriticalSection = false;
        spriteRenderer.color = Color.white; // Reset to default color

        // Update the turn to the next process
        currentTurn = (currentTurn + 1) % N;
    }
}
