using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject processPrefab;  // Drag your ProcessPrefab here
    public int numberOfProcesses = 3; // Number of processes to create
    public AudioClip backgroundMusic; // Drag your audio clip here
    private AudioSource audioSource;
    public static List<Process> processes = new List<Process>(); // Static list to manage all processes

    void Start()
    {
        // Set up audio source
        audioSource = GetComponent<AudioSource>();
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }

        // Set the static N value in Process class
        Process.N = numberOfProcesses;

        // Create processes in a circle around the resource
        for (int i = 0; i < numberOfProcesses; i++)
        {
            // Calculate position in a circle
            float angle = i * (360f / numberOfProcesses);
            Vector3 position = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * 5f,
                Mathf.Sin(angle * Mathf.Deg2Rad) * 5f,
                0
            );

            // Create the process
            GameObject processObj = Instantiate(processPrefab, position, Quaternion.identity);
            Process process = processObj.GetComponent<Process>();
            process.processId = i;

            // Add to the static list of processes
            processes.Add(process);
        }
    }

    void Update()
    {
        // When space is pressed, all processes will try to access the resource
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Process process in processes)
            {
                process.RequestResource();
            }
        }
    }

    // Method to retrieve all processes
    public static List<Process> GetProcesses()
    {
        return processes;
    }
}
