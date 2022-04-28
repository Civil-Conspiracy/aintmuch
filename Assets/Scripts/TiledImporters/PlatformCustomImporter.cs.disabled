using UnityEngine;
using SuperTiled2Unity;
using SuperTiled2Unity.Editor;

[AutoCustomTmxImporter()]
public class PlatformCustomImporter : CustomTmxImporter
{
    public override void TmxAssetImported(TmxAssetImportedArgs args)
    {
        var all = args.ImportedSuperMap.GetComponentsInChildren<SuperTileLayer>();

        foreach (SuperTileLayer t in all)
        {
            if (t.CompareTag("Platform"))
            {
                t.transform.GetChild(0).GetComponent<Collider2D>().usedByEffector = true;
                t.transform.GetChild(0).gameObject.AddComponent<PlatformEffector2D>();
            }
        }
    }
}
