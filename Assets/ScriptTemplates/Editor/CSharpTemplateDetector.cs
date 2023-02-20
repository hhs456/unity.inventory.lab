using System.IO;
using UnityEditor;
using UnityEngine;

public class CSharpTemplateDetector : AssetModificationProcessor {

    // �D�ϥΪ̾ާ@�Ӳ��͸귽�ɱN�|�I�s���禡�Aeg. ".meta" �ɮ�
    // �ݨϥ� static �׹���
    static void OnWillCreateAsset(string path) {
        // ����.meta extention�r��
        path = path.Replace(".meta", "");

        // �˴����|�����ɦW
        if (Path.GetExtension(path) != ".cs") {
            return;
        }

        // ���o�}�����|
        var index = Application.dataPath.LastIndexOf("Assets");
        path = Application.dataPath.Substring(0, index) + path;

        // ���o�}���W��
        var fileName = Path.GetFileNameWithoutExtension(path);

        // �ഫ����r
        var file = File.ReadAllText(path);
        ReplaceKeyword(fileName, ref file);

        // �мg�ɮרí��s��z�귽
        File.WriteAllText(path, file);
        AssetDatabase.Refresh();
    }


    // �ഫ����r
    static void ReplaceKeyword(string fileName, ref string script) {
        // �N���ɦW�� "Editor" �m���� "" �A�A�N���r����N "#TargetScriptName#"
        script = script.Replace("#TargetScriptName#", fileName.Replace("Editor", ""));
    }

}