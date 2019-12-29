using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//? 에셋 번들 스크립트
public class BundleBuilder : Editor
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles(){
        //! 에셋 번들 빌드 후 정리
        BuildPipeline.BuildAssetBundles("Assets/AssetBundle",BuildAssetBundleOptions.None, BuildTarget.Android);
    }

}
