using UnityEngine;

public class HandlePositionLoader : MonoBehaviour
{
    public bool spawnWhenReady = true;
    private string objectName;

    void Awake()
    {
        objectName = gameObject.name;
    }

    void Start()
    {
        if (spawnWhenReady && PlayerPrefs.GetInt(objectName + "_HasTransform", 0) == 1)
        {
            Vector3 pos = new Vector3(
                PlayerPrefs.GetFloat(objectName + "_Pos_X"),
                PlayerPrefs.GetFloat(objectName + "_Pos_Y"),
                PlayerPrefs.GetFloat(objectName + "_Pos_Z")
            );

            Vector3 rot = new Vector3(
                PlayerPrefs.GetFloat(objectName + "_Rot_X"),
                PlayerPrefs.GetFloat(objectName + "_Rot_Y"),
                PlayerPrefs.GetFloat(objectName + "_Rot_Z")
            );

            transform.position = pos;
            transform.eulerAngles = rot;

            Debug.Log($"Loaded {objectName} position and rotation.");
        }
    }
}
