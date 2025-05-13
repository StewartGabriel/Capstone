using UnityEngine;

public class Handle1PositionLoader : MonoBehaviour
{
    public bool spawnWhenReady = true;

    void Start()
    {
        if (spawnWhenReady && PlayerPrefs.GetInt("Handle1_HasTransform", 0) == 1)
        {
            Vector3 pos = new Vector3(
                PlayerPrefs.GetFloat("Handle1_Pos_X"),
                PlayerPrefs.GetFloat("Handle1_Pos_Y"),
                PlayerPrefs.GetFloat("Handle1_Pos_Z")
            );

            Vector3 rot = new Vector3(
                PlayerPrefs.GetFloat("Handle1_Rot_X"),
                PlayerPrefs.GetFloat("Handle1_Rot_Y"),
                PlayerPrefs.GetFloat("Handle1_Rot_Z")
            );

            transform.position = pos;
            transform.eulerAngles = rot;

            Debug.Log("Loaded Handle1 position and rotation.");
        }
    }
}
