using UnityEditor;

namespace FPSepController
{
    [InitializeOnLoad]
    public class LayerUtils
    {
        //Layer to replace with the new name
        static LayerUtils()
        {
            CreateLayer(3, "Player");
        }


        static void CreateLayer(int _layerIndex, string _layerName)
        {
            //Get the layer property from the TagManager asset.
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layers = tagManager.FindProperty("layers");


            //Check if we can find the layer by going through the array.
            bool ExistLayer = false;
            for (int i = _layerIndex; i < layers.arraySize; i++)
            {
                SerializedProperty layerSP = layers.GetArrayElementAtIndex(i);
                if (layerSP.stringValue == _layerName)
                {
                    ExistLayer = true;
                    break;
                }
            }


            //Check if the layer is empty and doesn't exist -- then change it.
            for (int j = _layerIndex; j < layers.arraySize; j++)
            {
                SerializedProperty layerSP = layers.GetArrayElementAtIndex(j);
                if (layerSP.stringValue == "" && !ExistLayer)
                {
                    layerSP.stringValue = _layerName;
                    tagManager.ApplyModifiedProperties();

                    break;
                }
            }
        }
    }
}