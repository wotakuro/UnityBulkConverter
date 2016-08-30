# UnityBulkConverter
[Unity]library and samples for converting each assets

Unityで全てのprefabやシーン中のオブジェクトに対して処理をしたいという時のためのUtilityです。<br />
・UI.Textのフォントを一括で修正したい<br />
・全てのMeshColliderを削除して、BoxColliderに置き換えたい<br />
などと言った処理を楽に書けるようにするモノです。

全てのprefabに対する例としては以下のようになります。

    // boxColliderを追加する処理
    private static bool AddBoxCollider(GameObject gmo,string prefabPath)
    {
       if (gmo.GetComponent<BoxCollider>() != null)
       {
         return false;
       }
       gmo.AddComponent<BoxCollider>();
       return true;
    }
    // 全てのprefabに対して AddBoxColliderを実行
    BulkConvertBatch.BulkConvertUtility.DoAllPrefab(AddBoxCollider, "AddCollider");

全てのシーン中のオブジェクトに対して処理をしたいときは下記です。

    private static bool RectToSmall(RectTransform rectTransform)
    {
        if (rectTransform == null) { return false; }
        rectTransform.localScale = rectTransform.localScale * 0.5f;

        return true;
    }
    // 全てのシーン中のRecttransformに対して処理をします
    BulkConvertBatch.BulkConvertUtility.DoAllComponentsInAllScene<RectTransform>(RectToSmall);
