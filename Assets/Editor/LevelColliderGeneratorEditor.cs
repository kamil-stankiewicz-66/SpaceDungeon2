using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelColliderGeneratorEditor : EditorWindow
{
    [SerializeField] Tilemap floorTilemap, colliderTilemap;
    [SerializeField] TileBase barrier;

    [MenuItem("CustomEditors/Collider Generator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<LevelColliderGeneratorEditor>("CollGen");
    }

    private Vector2 scrollPosition;

    private void OnGUI()
    {
        //scrollbar start
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        //vars
        floorTilemap = EditorGUILayout.ObjectField("FloorTilemap:", floorTilemap, typeof(Tilemap), true) as Tilemap;
        
        //other
        colliderTilemap = EditorGUILayout.ObjectField("ColliderTilemap:", colliderTilemap, typeof(Tilemap), true) as Tilemap;
        barrier = (TileBase)EditorGUILayout.ObjectField("barrier", barrier, typeof(TileBase), false);

        //scroolbar end
        EditorGUILayout.EndScrollView();

        //button style
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fixedHeight = 30;

        //button
        if (GUILayout.Button("Generate", buttonStyle))
        {
            Undo.RecordObject(colliderTilemap, "Clear previous Collider Generator: Generate");
            GenerateCollider();
        }

        if (GUILayout.Button("Clear", buttonStyle))
        {
            Undo.RecordObject(colliderTilemap, "Clear previous Collider Generator: Clear");
            ClearColliderTilemap();
        }
    }

    void ClearColliderTilemap()
    {
        if (colliderTilemap == null)
        {
            return;
        }

        colliderTilemap.ClearAllTiles();
    }

    void GenerateCollider()
    {
        if (floorTilemap == null && colliderTilemap == null)
        {
            return;
        }

        HashSet<Vector3Int> tiles = GetTilemapTiles(floorTilemap);

        foreach (var cell in tiles)
        {
            //vectors
            Vector3Int[] LoopTab = new Vector3Int[]
            {
                cell,
                cell + Vector3Int.up,
                cell + Vector3Int.down,
                cell + Vector3Int.left,
                cell + Vector3Int.right,
                cell + Vector3Int.right + Vector3Int.up,
                cell + Vector3Int.right + Vector3Int.down,
                cell + Vector3Int.left + Vector3Int.up,
                cell + Vector3Int.left + Vector3Int.down
            };

            //draw
            foreach (Vector3Int vec in LoopTab)
            {
                IfNullDraw(vec, tiles);
            }
        }

    }

    void IfNullDraw(Vector3Int position, HashSet<Vector3Int> temp)
    {
        if (temp.Contains(position))
        {
            return;
        }

        if (colliderTilemap.GetTile(position) != null)
        {
            return;
        }

        colliderTilemap.SetTile(position, barrier);
    }



    HashSet<Vector3Int> GetTilemapTiles(Tilemap tilemap)
    {
        //vars
        HashSet<Vector3Int> temp = new HashSet<Vector3Int>();
        Vector3Int bottomLeft = tilemap.cellBounds.min;
        Vector3Int topRight = tilemap.cellBounds.max;

        //filter
        for (int _y = bottomLeft.y; _y <= topRight.y; _y++)
        {
            for (int _x = bottomLeft.x; _x <= topRight.x; _x++)
            {
                var position = new Vector3Int(_x, _y, 0);
                TileBase currentTile = floorTilemap.GetTile(position);

                if (currentTile != null)
                {
                    temp.Add(position);
                }
            }
        }

        return temp;
    }
}
