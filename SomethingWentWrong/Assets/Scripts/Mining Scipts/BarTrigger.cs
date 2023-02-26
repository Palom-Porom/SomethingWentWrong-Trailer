using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarTrigger : MonoBehaviour
{
    [SerializeField] Slider slider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == SpawnSystemScript.instance.player)
        {
            slider.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == SpawnSystemScript.instance.player)
        {
            slider.gameObject.SetActive(false);
        }
    }
}
