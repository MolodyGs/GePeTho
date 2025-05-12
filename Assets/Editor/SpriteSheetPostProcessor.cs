using UnityEngine;
using UnityEditor;

public class DefaultSpriteImporter : AssetPostprocessor
{
  void OnPreprocessTexture()
  {
    // Es una imagen
    if (assetPath.ToLower().EndsWith(".png") || assetPath.ToLower().EndsWith(".jpg"))
    {
      TextureImporter importer = (TextureImporter)assetImporter;

      // Solo modifica si aún no está configurado como Sprite
      if (importer.textureType != TextureImporterType.Sprite)
      {
        importer.textureType = TextureImporterType.Sprite;
      }

      // // Establecer como sprite múltiple
      // importer.spriteImportMode = SpriteImportMode.Multiple;

      // Desactivar mipmaps
      importer.mipmapEnabled = false;

      // Establecer filtros
      importer.filterMode = FilterMode.Point;
      importer.textureCompression = TextureImporterCompression.Uncompressed;

      // Establecer Pixels Per Unit
      importer.spritePixelsPerUnit = 1;

      // Ajuste de wrapping
      importer.wrapMode = TextureWrapMode.Clamp;
    }
  }
}
