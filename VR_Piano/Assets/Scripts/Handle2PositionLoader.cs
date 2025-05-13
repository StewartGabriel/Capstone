using UnityEngine;

public class Handle2PositionLoader : MonoBehaviour
{
    public bool spawnWhenReady = true;

    void Start()
    {
        if (spawnWhenReady && PlayerPrefs.GetInt("Handle2_HasTransform", 0) == 1)
        {
            Vector3 pos = new Vector3(
                PlayerPrefs.GetFloat("Handle2_Pos_X"),
                PlayerPrefs.GetFloat("Handle2_Pos_Y"),
                PlayerPrefs.GetFloat("Handle2_Pos_Z")
            );

            Vector3 rot = new Vector3(
                PlayerPrefs.GetFloat("Handle2_Rot_X"),
                PlayerPrefs.GetFloat("Handle2_Rot_Y"),
                PlayerPrefs.GetFloat("Handle2_Rot_Z")
            );

            transform.position = pos;
            transform.eulerAngles = rot;

            Debug.Log("Loaded Handle2 position and rotation.");
        }
    }
}
