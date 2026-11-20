using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

// level 1's baked lightmaps are empty (black). windows doesn't use them so it looks fine there,
// but the web build does and the whole level goes dark.
// so on load: drop the lightmaps and light everything with the sky/ambient light instead
public static class PoolroomsLightingFix
{
    const string SceneName = "Level 1";

    // runs once when the game starts, then checks every scene that loads
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        SceneManager.sceneLoaded += (scene, mode) => Apply(scene);
        Apply(SceneManager.GetActiveScene());
    }

    static void Apply(Scene scene)
    {
        if (scene.name != SceneName) return;

        // make sure the ambient light of this scene is ready
        DynamicGI.UpdateEnvironment();

        // static objects don't get the ambient light on their own, so pass it in directly
        var block = new MaterialPropertyBlock();
        block.CopySHCoefficientArraysFrom(new[] { RenderSettings.ambientProbe });

        // every object that used a lightmap: stop using it and take the ambient light
        foreach (var r in Object.FindObjectsByType<Renderer>())
        {
            if (r.lightmapIndex < 0 || r.lightmapIndex >= 65534) continue; // not lightmapped
            r.lightmapIndex = -1;
            r.lightProbeUsage = LightProbeUsage.CustomProvided;
            r.SetPropertyBlock(block);
        }
        // remove the empty lightmaps
        LightmapSettings.lightmaps = new LightmapData[0];
    }
}
