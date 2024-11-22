using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    private TextMeshProUGUI blockText;  // Text inside the block
    private Image blockImage;          // Background color of the block

    public float TimeUpdated;          // Last time this block was updated
    public bool isEmpty = true;        // Whether this block is empty
    

    void Awake()
    {
        // Get references to components
        blockText = GetComponentInChildren<TextMeshProUGUI>();
        if (blockText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found in children!");
        }

        blockImage = GetComponent<Image>();
        if (blockImage == null)
        {
            Debug.LogError("Image component not found on this GameObject!");
        }
    }
    
    public void UpdateBlock(string number, Color textColor, Color bgColor)
    {
        blockText.text = number;
        blockText.color = textColor;
        blockImage.color = bgColor;
        isEmpty = false;
        TimeUpdated = Time.time;
    }

    public void HighlightNextBlock(Color textColor, Color bgColor)
    {
        blockText.color = textColor;
        blockImage.color = bgColor;
    }

    public void ResetBlockFormatting()
    {
        blockText.color = Color.black;
        blockImage.color = Color.white;
    }

    public void ResetBlock()
    {
        blockText.text = "";
        blockText.color = Color.black;
        blockImage.color = Color.white;
        isEmpty = true;
    }

    public string GetText()
    {
        return blockText.text;
    }
}
