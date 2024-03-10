using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] Slider progressBar;
    public float fillSpeed;
    public bool isAbility;

    // Update is called once per frame
    void Update()
    {
        if (!isAbility)
        {
            if (progressBar.value < progressBar.maxValue)
            {
                progressBar.value += fillSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (progressBar.value > progressBar.minValue)
            {
                progressBar.value -= fillSpeed * Time.deltaTime;
            }

            if (progressBar.value == progressBar.minValue)
            {
                progressBar.value = progressBar.maxValue;
                gameObject.SetActive(false);
            }
        }
    }
}
