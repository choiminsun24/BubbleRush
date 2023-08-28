using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class ExelReader : MonoBehaviour
{
    //���Խ�, ���ڿ� ó���� ����.
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    //�о���� �޼ҵ�
    public static List<Dictionary<string, string>> Read(string path)
    {
        List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
        TextAsset data = Resources.Load(path) as TextAsset;

        //������ ������
        string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);
        if (lines.Length <= 1) return list;

        //header
        string[] header = Regex.Split(lines[2], SPLIT_RE);

        //�� ó��
        for (int i = 3; i < lines.Length; i++)
        {
            //���� ������
            string[] line = Regex.Split(lines[i], SPLIT_RE);
            if (line.Length == 0 || line[0] == "") continue;

            //�� ó�� - �ٵ� Dictionary��
            Dictionary<string, string> column = new Dictionary<string, string>();

            for (int j = 0; j < line.Length; j++)
            {
                string value = line[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\n", "\n");
                column[header[j]] = value;
            }

            list.Add(column);
        }

        return list;
    }
}
