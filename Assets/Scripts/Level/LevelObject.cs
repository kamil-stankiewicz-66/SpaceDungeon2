using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    public string ID;

#if UNITY_EDITOR
    private void OnValidate()
    {
        GenerateID();
    }
#endif


    [ContextMenu("Generate ID")]
    public void GenerateID()
    {
        if (this == null) return;

        bool needsNewID = string.IsNullOrEmpty(ID);
        
        if (!needsNewID)
        {
            LevelObject[] allInHierarchy = transform.root.GetComponentsInChildren<LevelObject>(true);

            foreach (LevelObject obj in allInHierarchy)
            {
                if (obj != this && obj.ID == ID)
                {
                    needsNewID = true;
                    break;
                }
            }
        }

        if (needsNewID)
        {
            Undo.RecordObject(this, "Generate Unique ID");
            ID = System.Guid.NewGuid().ToString();

            EditorUtility.SetDirty(this);

            Debug.LogWarning($"{gameObject.name} updated id: {ID}");
        }
    }
}