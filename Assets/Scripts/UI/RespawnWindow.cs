using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnWindow : MonoBehaviour
{
    public Text remainingLives;

    public Character PC;

    // Start is called before the first frame update
    void Start()
    {
        PC = FindObjectOfType<Character>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        remainingLives.text = PC.health.ToString();
    }
}
