using UnityEngine;
using TMPro;

public class BlockManager : MonoBehaviour
{
    public Block[] blocks;  // Array to hold the blocks
    public int maxBlocks = 5;  // Maximum number of blocks in the queue

    public TextMeshProUGUI lastPressText;  // Displays the last pressed number
    public TextMeshProUGUI emptySlotText;  // Displays empty slot messages
    public TextMeshProUGUI alreadyExistsText;  // Displays already exists messages
    public AudioSource alreadyExistsSound;  // Plays 8-bit sound for already exists
    public AudioSource backgroundSound;
    public AudioSource clickSound;

      private float defaultBackgroundVolume; // Stores the default background volume

    void Start()
    {
        // Ensure all blocks are initialized and empty at the start
        foreach (var block in blocks)
        {
            if (block != null)
            {
                block.ResetBlock();  // Reset each block to its default state
            }
        }

        // Clear initial messages
        emptySlotText.text = "";
        alreadyExistsText.text = "";
        lastPressText.text = "";
  // Set initial volumes
        defaultBackgroundVolume = 0.2f; // Lower background volume
        backgroundSound.volume = defaultBackgroundVolume;

          // Start background sound
        if (!backgroundSound.isPlaying)
        {
            backgroundSound.loop = true;
            backgroundSound.Play();
        }
    }

    void Update()
    {
        // Check for number key presses
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                AddNumberToQueue(i.ToString());
            }
        }
    }

    public void AddNumberToQueue(string number)
    {
        // Clear block formatting
        foreach (var block in blocks)
        {
            block.ResetBlockFormatting();
        }

        // Update the LastPress text
        lastPressText.text = $"{number}";

        // Check if the number already exists in the queue
        foreach (var block in blocks)
        {
            if (!block.isEmpty && block.GetText() == number)
            {
                alreadyExistsText.text = $"Number {number} already exists!";
                PlaySound(alreadyExistsSound);  // Play already exists sound
                return;
            }
        }

        // Check for empty slots
        foreach (var block in blocks)
        {
            if (block.isEmpty)
            {
                block.UpdateBlock(number, Color.green, Color.cyan);  // Add to the first empty block
                PlaySound(clickSound);  // Play click sound
                emptySlotText.text = "There is an empty slot, no replacement expected.";
                return;
            }
        }

        // If no empty slot, replace the oldest block!
        emptySlotText.text = "";  // Clear the empty slot message
        Block oldestBlock = GetOldestBlock();
        Block secondOldestBlock = GetSecondOldestBlock();

        if (oldestBlock != null)
        {
            oldestBlock.UpdateBlock(number, Color.green, Color.cyan);  // Update oldest block with new value
            PlaySound(clickSound);  // Play click sound
        }

        if (secondOldestBlock != null)
        {
            secondOldestBlock.HighlightNextBlock(Color.red, Color.gray);  // Highlight the next block to be replaced
        }
    }

    private Block GetOldestBlock()
    {
        Block oldestBlock = null;
        float oldestTime = float.MaxValue;

        foreach (var block in blocks)
        {
            if (block != null && !block.isEmpty)
            {
                if (block.TimeUpdated < oldestTime)
                {
                    oldestTime = block.TimeUpdated;
                    oldestBlock = block;
                }
            }
        }

        return oldestBlock;
    }

    private Block GetSecondOldestBlock()
    {
        Block oldestBlock = null;
        Block secondOldestBlock = null;
        float oldestTime = float.MaxValue;
        float secondOldestTime = float.MaxValue;

        foreach (var block in blocks)
        {
            if (block != null && !block.isEmpty)
            {
                if (block.TimeUpdated < oldestTime)
                {
                    secondOldestTime = oldestTime;
                    secondOldestBlock = oldestBlock;

                    oldestTime = block.TimeUpdated;
                    oldestBlock = block;
                }
                else if (block.TimeUpdated < secondOldestTime)
                {
                    secondOldestTime = block.TimeUpdated;
                    secondOldestBlock = block;
                }
            }
        }

        return secondOldestBlock;
    }



  private void PlaySound(AudioSource sound)
    {
        // Reduce background sound volume during playback
        backgroundSound.volume = defaultBackgroundVolume * 0.5f;

        // Play sound only if it's not already playing
        if (!sound.isPlaying)
        {
            sound.Play();
        }



    }

}
