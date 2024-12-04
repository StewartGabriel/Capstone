using UnityEngine;

public class PianoKeyInput : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) { SoundManager.PlaySound(SoundType.fNOTES); }
        else if (Input.GetKeyDown(KeyCode.W)) { SoundManager.PlaySound(SoundType.fNOTES); }
        else if (Input.GetKeyDown(KeyCode.E)) { SoundManager.PlaySound(SoundType.gNOTES); }
        else if (Input.GetKeyDown(KeyCode.R)) { SoundManager.PlaySound(SoundType.gNOTES); }
        else if (Input.GetKeyDown(KeyCode.T)) { SoundManager.PlaySound(SoundType.SHARPS); }
        else if (Input.GetKeyDown(KeyCode.Y)) { SoundManager.PlaySound(SoundType.SHARPS); }
        else if (Input.GetKeyDown(KeyCode.U)) { SoundManager.PlaySound(SoundType.aNOTES); }
        else if (Input.GetKeyDown(KeyCode.I)) { SoundManager.PlaySound(SoundType.aNOTES); }
        else if (Input.GetKeyDown(KeyCode.O)) { SoundManager.PlaySound(SoundType.bNOTES); }
        else if (Input.GetKeyDown(KeyCode.P)) { SoundManager.PlaySound(SoundType.bNOTES); }
        else if (Input.GetKeyDown(KeyCode.A)) { SoundManager.PlaySound(SoundType.cNOTES); }
        else if (Input.GetKeyDown(KeyCode.S)) { SoundManager.PlaySound(SoundType.cNOTES); }
        else if (Input.GetKeyDown(KeyCode.D)) { SoundManager.PlaySound(SoundType.dNOTES); }
        else if (Input.GetKeyDown(KeyCode.F)) { SoundManager.PlaySound(SoundType.dNOTES); }
        else if (Input.GetKeyDown(KeyCode.G)) { SoundManager.PlaySound(SoundType.eNOTES); }
        else if (Input.GetKeyDown(KeyCode.H)) { SoundManager.PlaySound(SoundType.eNOTES); }
        else if (Input.GetKeyDown(KeyCode.J)) { SoundManager.PlaySound(SoundType.SHARPS); }
        else if (Input.GetKeyDown(KeyCode.K)) { SoundManager.PlaySound(SoundType.SHARPS); }
        else if (Input.GetKeyDown(KeyCode.L)) { SoundManager.PlaySound(SoundType.SHARPS); }
        else if (Input.GetKeyDown(KeyCode.Z)) { SoundManager.PlaySound(SoundType.SHARPS); }
        else if (Input.GetKeyDown(KeyCode.X)) { SoundManager.PlaySound(SoundType.SHARPS); }
        else if (Input.GetKeyDown(KeyCode.C)) { SoundManager.PlaySound(SoundType.SHARPS); }
        else if (Input.GetKeyDown(KeyCode.V)) { SoundManager.PlaySound(SoundType.SHARPS); }
        else if (Input.GetKeyDown(KeyCode.B)) { SoundManager.PlaySound(SoundType.SHARPS); }
    }
}
