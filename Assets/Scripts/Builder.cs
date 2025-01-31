using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField] private GameObject preBuildObject;
    [SerializeField] private GameObject postBuildObject;
    
    public void Build()
    {
        preBuildObject.SetActive(false);
        postBuildObject.SetActive(true);
    }

    public void Unbuild()
    {
        preBuildObject.SetActive(true);
        postBuildObject.SetActive(false);
    }
}
