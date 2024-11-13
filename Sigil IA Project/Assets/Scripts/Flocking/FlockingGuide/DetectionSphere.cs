using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionSphere : MonoBehaviour
{
    //posicionar esfera en el centro de los monks

    [SerializeField] GameObject[] monks;

    private void Update()
    {
        Vector3 midPoint = CalculateMidPoint();
        transform.position = midPoint;
    }

    private Vector3 CalculateMidPoint()
    {
        Vector3 finalPos = Vector3.zero;

        foreach (var item in monks)
        {
            finalPos += item.transform.position;
        }

        return finalPos / monks.Length;
    }

    public void AlertMonks()
    {
        foreach (var monk in monks)
        {
            monk.GetComponent<Monk>().IsAlerted = true;
        }
    }
}
