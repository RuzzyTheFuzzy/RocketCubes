using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathMessages : MonoBehaviour
{

    [SerializeField] private GameObject message;
    [SerializeField] private float fadeTime;
    private List<GameObject> currentMessages = new List<GameObject>();
    private float height;

    private void Awake()
    {
        RectTransform messageTransform = message.GetComponent<RectTransform>();

        // Height of the message prefab
        height = messageTransform.anchorMax.y - messageTransform.anchorMin.y;
    }

    public void NewMessage(Player player)
    {
        GameObject newMessage = Instantiate(message, transform);
        TMP_Text textObj = newMessage.GetComponent<TMP_Text>();
        string text = player.gameObject.name;

        if (player.Health <= 0)
        {
            text += " Died";
        }
        else
        {
            text += " Hit";
        }

        textObj.text = text;
        textObj.color = player.Ball.gameObject.GetComponent<MeshRenderer>().material.color;
        currentMessages.Insert(0, newMessage);

        StartCoroutine(IFade(textObj));
        ReorderMessages();
    }
    private void ReorderMessages()
    {
        for (int i = 0; i < currentMessages.Count; i++)
        {
            RectTransform currentTransform = currentMessages[i].GetComponent<RectTransform>();
            // anchor.y cant be directly modified, so add a vector 2 with only a y component to get around
            currentTransform.anchorMax += height * i * Vector2.up;
            currentTransform.anchorMin += height * i * Vector2.up;
        }
    }

    private IEnumerator IFade(TMP_Text textObj)
    {
        Color color = textObj.color;

        // Multiply deltatime by fadespeed to make it fully dissapear by fadeTime
        float fadeSpeed = color.a / fadeTime;

        while (color.a > 0f)
        {
            color.a -= Time.deltaTime * fadeSpeed;
            textObj.color = color;
            yield return null;
        }

        Destroy(textObj.gameObject);
        currentMessages.Remove(textObj.gameObject);
        ReorderMessages();
    }
}
