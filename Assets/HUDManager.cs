using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private Slider sliderPlayer;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Slider sliderEnemy;
    [SerializeField]
    private GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        sliderPlayer.value = 100;
        sliderEnemy.value = 100;
    }

    // Update is called once per frame
    void Update()
    {
        sliderPlayer.value = player.GetComponent<MovementBehaviours>().GetHealth();
        //sliderEnemy.value = enemy.GetComponent<EnemmyBehaviours>().GetHealth();
    }
}
