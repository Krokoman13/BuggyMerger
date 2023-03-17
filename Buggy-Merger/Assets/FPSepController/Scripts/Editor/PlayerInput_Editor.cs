using UnityEditor;

namespace FPSepController
{
    [CustomEditor(typeof(PlayerInput), false)]
    public class PlayerInput_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            PlayerInput pInput = target as PlayerInput;
            if(pInput.allAxis.Count == 0 || pInput.allButtons.Count == 0)
            {
                EditorGUILayout.HelpBox("The FPSepController package has presets available to use. Click the second button on the topright of this component.", MessageType.Info);                
            }

#if !ENABLE_LEGACY_INPUT_MANAGER
            EditorGUILayout.HelpBox("UNITY INPUT MANAGER (LEGACY) NOT ENABLED. GO TO PLAYER SETTINGS -> OTHER -> ACTIVE INPUT HANDLING.", MessageType.Error);
#endif


            base.OnInspectorGUI();
        }
    }
}