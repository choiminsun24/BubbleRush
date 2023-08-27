using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class ExelReader : MonoBehaviour
{
	//정규식, 문자열 처리를 위함.
	static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
	static string LINE_SPLIT_RE = @"[\r\n|\n\r|\n|\r](?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
	static char[] TRIM_CHARS = {'\"', '\\'};

	public static List<Dictionary<string, string>> Read(string path)
    {
		List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();

		TextAsset data = Resources.Load(path) as TextAsset;
		string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);

		if (lines.Length <= 1) return list;

		Debug.Log(lines[0][0]);
		Debug.Log(lines[2][0]);
		Debug.Log(lines[3][0]);
		Debug.Log(lines[1][0]);

		string[] header = Regex.Split(lines[2], SPLIT_RE);

		for(int i = 3; i < lines.Length; i++)
		{
			string[] line = Regex.Split(lines[i], SPLIT_RE);
			if (line.Length == 0 || line[0] == "") continue; 

			Dictionary<string, string> column = new Dictionary<string, string>();

			for (int j = 4; j < line.Length; j++)
            {
				string value = line[j];
				value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\n", "\n");

				//string realvalue = value;

				//int n;
				//float f;

				//if (int.TryParse(value, out n))
				//            {
				//	realvalue = n;
				//            }
				//else if (float.TryParse(value, out f))
				//            {
				//	realvalue = f;
				//            }
				for (int k = 0; k < header.Length; k++)
                {
					Debug.Log(header[k]);
                }
                try
                {
					column[value] = value;
                }
				catch (Exception ex)
                {
					Debug.Log("j: " + j + " value: " + value);
                }
				
				//column[header[j]] = value;
            }

			list.Add(column);
        }

		return list;
    }
}
