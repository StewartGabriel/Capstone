using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyStartMenu : MonoBehaviour
{
    public static DontDestroyStartMenu Instance;

    [SerializeField] public GameObject[] objects;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        if (DontDestroyStartMenu.Instance != null){
            foreach (var obj in objects){
                SetObj(obj);
            }
        }
    }

    private void SetObj(GameObject obj) 
    {
        if (obj != null)
        {
            obj.SetActive(true); // Example: Enable the object
        }
    }
}