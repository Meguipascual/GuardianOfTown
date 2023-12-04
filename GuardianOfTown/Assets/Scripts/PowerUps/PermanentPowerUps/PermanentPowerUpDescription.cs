using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentPowerUpDescription : MonoBehaviour
{
    public GameObject descriptionPanel;
    // Start is called before the first frame update
    public void ShowDescriptionPanel()
    {
        float zRotation = Random.Range(-7, 7);
        gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1);
        descriptionPanel.SetActive(true);
        descriptionPanel.transform.Rotate(new Vector3(0, 0, zRotation));
    }

    public void HideDescriptionPanel()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        descriptionPanel.SetActive(false);
        descriptionPanel.transform.rotation = Quaternion.identity;
    }
}
