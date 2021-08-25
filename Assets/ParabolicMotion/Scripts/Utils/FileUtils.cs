using System;
using System.IO;
using Debug = UnityEngine.Debug;

public static class FileUtils
{

	public static void WriteToFile(string path, string data) {
		Debug.Log($"Writing data at {path}:\n {data}");
		StreamWriter writer = new StreamWriter(path);
		writer.Write(data);
		writer.Close();
	}

	public static bool LoadFromFile(string path, out string data) {
		Debug.Log($"Reading data at {path}");
		data = "";

		try {
			StreamReader reader = new StreamReader(path);
			data = reader.ReadToEnd();
			reader.Close();
		}
		catch (Exception e) {
			Debug.Log($"Failed reading the file: {e}");
			return false;
		}

		return true;
	}
}
