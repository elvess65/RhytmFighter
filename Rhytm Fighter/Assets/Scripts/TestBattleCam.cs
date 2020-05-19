using FrameworkPackage.Utils;
using RhytmFighter.CameraSystem;
using RhytmFighter.Core;
using RhytmFighter.Core.Enums;
using UnityEngine;

public class TestBattleCam : MonoBehaviour
{
    public Transform Obj1;
    public Transform Obj2;

    private bool m_IsFocused = false;
    private CameraController m_CameraController;

    public void Init(CameraController cameraController)
    {
        m_CameraController = cameraController;
    }


    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 50), "Focus 1"))
            Focus(Obj1);

        if (GUI.Button(new Rect(10, 70, 150, 50), "Focus 2"))
            Focus(Obj2);

        if (GUI.Button(new Rect(10, 150, 150, 50), "Unfocus"))
        {
            m_IsFocused = false;
            m_CameraController.ActivateCamera(CameraTypes.Main);
        }
    }

    void Focus(Transform target)
    {
        m_CameraController.PeekMemberFromTargetGroup();
        m_CameraController.PushMemberToTargetGroup(target, 1.25f);

        Quaternion targetCameraRotation = Quaternion.LookRotation(target.position - GameManager.Instance.PlayerModel.ViewPosition);
        Vector3 cameraEuler = GameManager.Instance.CamerasHolder.VCamBattle.transform.localEulerAngles;
        cameraEuler.y = targetCameraRotation.eulerAngles.y;
        targetCameraRotation.eulerAngles = cameraEuler;

        if (!m_IsFocused)
            GameManager.Instance.CamerasHolder.VCamBattle.transform.localEulerAngles = targetCameraRotation.eulerAngles;
        else
            m_CameraController.StartSmoothRotation(CameraTypes.Battle, targetCameraRotation);

        m_CameraController.ActivateCamera(CameraTypes.Battle);

        m_IsFocused = true;
    }
}
