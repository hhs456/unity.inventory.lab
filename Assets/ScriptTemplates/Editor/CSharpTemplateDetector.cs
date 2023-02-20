using System.IO;
using UnityEditor;
using UnityEngine;

public class CSharpTemplateDetector : AssetModificationProcessor {

    // 非使用者操作而產生資源時將會呼叫此函式，eg. ".meta" 檔案
    // 需使用 static 修飾詞
    static void OnWillCreateAsset(string path) {
        // 移除.meta extention字串
        path = path.Replace(".meta", "");

        // 檢測路徑的副檔名
        if (Path.GetExtension(path) != ".cs") {
            return;
        }

        // 取得腳本路徑
        var index = Application.dataPath.LastIndexOf("Assets");
        path = Application.dataPath.Substring(0, index) + path;

        // 取得腳本名稱
        var fileName = Path.GetFileNameWithoutExtension(path);

        // 轉換關鍵字
        var file = File.ReadAllText(path);
        ReplaceKeyword(fileName, ref file);

        // 覆寫檔案並重新整理資源
        File.WriteAllText(path, file);
        AssetDatabase.Refresh();
    }


    // 轉換關鍵字
    static void ReplaceKeyword(string fileName, ref string script) {
        // 將原檔名的 "Editor" 置換為 "" ，再將此字串取代 "#TargetScriptName#"
        script = script.Replace("#TargetScriptName#", fileName.Replace("Editor", ""));
    }

}