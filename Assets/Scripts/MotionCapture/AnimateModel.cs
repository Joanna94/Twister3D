using LZWPlib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateModel : MonoBehaviour
{
    [Header("Bones")]

    [Tooltip("Assign based on avatar")]
    public bool autoAssign = true;
    [Tooltip("Transform component of each bone")]
    public Transform[] modelBoneTransforms;

    // for GUI label
    int trackedBonesCount = 0;
    int totalBonesCount = 0;

    private Transform transform;

    void Start()
    {
        transform = gameObject.GetComponent<Transform>();
        if (autoAssign)
        {
            Animator animator = GetComponent<Animator>();

            if (animator == null)
            {
                Debug.LogError("Animator component missing!");
                return;
            }

            modelBoneTransforms = new Transform[21];
            modelBoneTransforms[0] = animator.GetBoneTransform(HumanBodyBones.Hips);
            modelBoneTransforms[1] = animator.GetBoneTransform(HumanBodyBones.Spine);
            modelBoneTransforms[2] = animator.GetBoneTransform(HumanBodyBones.Chest);
            modelBoneTransforms[3] = null;
            modelBoneTransforms[4] = animator.GetBoneTransform(HumanBodyBones.UpperChest);
            modelBoneTransforms[5] = animator.GetBoneTransform(HumanBodyBones.Neck);
            modelBoneTransforms[6] = animator.GetBoneTransform(HumanBodyBones.Head);
            modelBoneTransforms[7] = animator.GetBoneTransform(HumanBodyBones.LeftShoulder);
            modelBoneTransforms[8] = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            modelBoneTransforms[9] = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            modelBoneTransforms[10] = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            modelBoneTransforms[11] = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
            modelBoneTransforms[12] = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
            modelBoneTransforms[13] = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
            modelBoneTransforms[14] = animator.GetBoneTransform(HumanBodyBones.RightHand);
            modelBoneTransforms[15] = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
            modelBoneTransforms[16] = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
            modelBoneTransforms[17] = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            modelBoneTransforms[18] = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
            modelBoneTransforms[19] = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
            modelBoneTransforms[20] = animator.GetBoneTransform(HumanBodyBones.RightFoot);
        }
    }

    void Update()
    {
        LzwpMocap.UpdateData(transform);
        UpdateModel();
    }

    void UpdateModel()
    {
        trackedBonesCount = 0;

        var fr = LzwpMocap.lastFrame;

        if (fr.humans.Count > 0)
        {
            totalBonesCount = fr.humans[0].numBones;
            for (int i = 0; i < totalBonesCount; i++)
            {
                try
                {
                    if (fr.humans[0].bones[i].tracked)
                    {
                        trackedBonesCount++;

                        if (modelBoneTransforms[i] == null)
                            continue;

                        if (i >= modelBoneTransforms.Length)
                            break;

                        if (i == 0)  // hips
                            modelBoneTransforms[i].position = fr.humans[0].bones[i].position;
                        modelBoneTransforms[i].rotation = fr.humans[0].bones[i].rotation;
                    }
                }
                catch (System.Exception) { }
            }
        }
    }

    void OnGUI()
    {
        GUI.Box(new Rect(5f, 5f, 150f, 25f), "Tracked bones: " + trackedBonesCount + "/" + totalBonesCount);
    }
}
