//using System;
//using UnityEngine;

//public class PianoKeyInput : MonoBehaviour
//{
//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Q)) { Debug.Log("Key Q pressed"); PlayNextAvailableSound(SoundType.fNOTES, 0); }
//        else if (Input.GetKeyDown(KeyCode.W)) { Debug.Log("Key W pressed"); PlayNextAvailableSound(SoundType.fNOTES, 1); }
//        else if (Input.GetKeyDown(KeyCode.E)) { Debug.Log("Key E pressed"); PlayNextAvailableSound(SoundType.fNOTES, 2); }
//        else if (Input.GetKeyDown(KeyCode.R)) { Debug.Log("Key R pressed"); PlayNextAvailableSound(SoundType.fNOTES, 3); }
//        else if (Input.GetKeyDown(KeyCode.T)) { Debug.Log("Key T pressed"); PlayNextAvailableSound(SoundType.fNOTES, 4); }
//        else if (Input.GetKeyDown(KeyCode.Y)) { Debug.Log("Key Y pressed"); PlayNextAvailableSound(SoundType.fNOTES, 5); }
//        else if (Input.GetKeyDown(KeyCode.U)) { Debug.Log("Key U pressed"); PlayNextAvailableSound(SoundType.fNOTES, 6); }
//        else if (Input.GetKeyDown(KeyCode.I)) { Debug.Log("Key I pressed"); PlayNextAvailableSound(SoundType.fNOTES, 7); }
//        else if (Input.GetKeyDown(KeyCode.O)) { Debug.Log("Key O pressed"); PlayNextAvailableSound(SoundType.fNOTES, 8); }
//        else if (Input.GetKeyDown(KeyCode.P)) { Debug.Log("Key P pressed"); PlayNextAvailableSound(SoundType.fNOTES, 9); }
//        else if (Input.GetKeyDown(KeyCode.A)) { Debug.Log("Key A pressed"); PlayNextAvailableSound(SoundType.fNOTES, 10); }
//        else if (Input.GetKeyDown(KeyCode.S)) { Debug.Log("Key S pressed"); PlayNextAvailableSound(SoundType.fNOTES, 11); }
//        else if (Input.GetKeyDown(KeyCode.D)) { Debug.Log("Key D pressed"); PlayNextAvailableSound(SoundType.fNOTES, 12); }
//        else if (Input.GetKeyDown(KeyCode.F)) { Debug.Log("Key F pressed"); PlayNextAvailableSound(SoundType.fNOTES, 13); }
//        else if (Input.GetKeyDown(KeyCode.G)) { Debug.Log("Key G pressed"); PlayNextAvailableSound(SoundType.fNOTES, 14); }
//        else if (Input.GetKeyDown(KeyCode.H)) { Debug.Log("Key H pressed"); PlayNextAvailableSound(SoundType.fNOTES, 15); }
//        else if (Input.GetKeyDown(KeyCode.J)) { Debug.Log("Key J pressed"); PlayNextAvailableSound(SoundType.fNOTES, 16); }
//        else if (Input.GetKeyDown(KeyCode.K)) { Debug.Log("Key K pressed"); PlayNextAvailableSound(SoundType.fNOTES, 17); }
//        else if (Input.GetKeyDown(KeyCode.L)) { Debug.Log("Key L pressed"); PlayNextAvailableSound(SoundType.fNOTES, 18); }
//        else if (Input.GetKeyDown(KeyCode.Z)) { Debug.Log("Key Z pressed"); PlayNextAvailableSound(SoundType.fNOTES, 19); }
//        else if (Input.GetKeyDown(KeyCode.X)) { Debug.Log("Key X pressed"); PlayNextAvailableSound(SoundType.fNOTES, 20); }
//        else if (Input.GetKeyDown(KeyCode.C)) { Debug.Log("Key C pressed"); PlayNextAvailableSound(SoundType.fNOTES, 21); }
//        else if (Input.GetKeyDown(KeyCode.V)) { Debug.Log("Key V pressed"); PlayNextAvailableSound(SoundType.fNOTES, 22); }
//        else if (Input.GetKeyDown(KeyCode.B)) { Debug.Log("Key B pressed"); PlayNextAvailableSound(SoundType.fNOTES, 23); }
//    }

//    private void PlayNextAvailableSound(SoundType startSoundType, int keyIndex)
//    {
//        SoundType soundType = startSoundType;
//        int index = keyIndex;
//        for (int i = 0; i < Enum.GetValues(typeof(SoundType)).Length; i++)
//        {
//            AudioClip clip = SoundManager.GetInstance().GetSpecificClip(soundType, index);
//            if (clip != null)
//            {
//                Debug.Log("Playing clip from " + soundType + " at index " + index + ": " + clip.name);
//                SoundManager.GetInstance().audioSource.PlayOneShot(clip);
//                return;
//            }
//            index = index - SoundManager.GetInstance().GetSoundList()[(int)soundType].Sounds.Length;
//            soundType = (SoundType)(((int)soundType + 1) % Enum.GetValues(typeof(SoundType)).Length);
//        }
//        Debug.LogWarning("No available sounds to play for key: " + keyIndex);
//    }
//}
