using UnityEditor;
using UnityEngine;


namespace FPSepController
{
    [CustomEditor(typeof(DisableSiblings))]
    public class DisableSiblings_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This component is used to disable sibling-objects. Remove it to not have it affect others/be affected anymore.", MessageType.Info);
            DisableSiblings();
        }

        void DisableSiblings()
        {
            DisableSiblings selectPlayer = target as DisableSiblings;
            Transform t = selectPlayer.transform;
            int childcount = t.parent.childCount;
            for (int i = 0; i < childcount; i++)
            {
                Transform currentChild = t.parent.GetChild(i);
                if (currentChild == t && currentChild.TryGetComponent<DisableSiblings>(out DisableSiblings other))
                {
                    currentChild.gameObject.SetActive(true);
                    continue;
                }

                currentChild.gameObject.SetActive(false);
            }
        }
    }

}