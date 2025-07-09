using System;

public static class SimpleJsonParser
{
    public static int ExtractEventTypeFromJson(string json)
    {
        string fieldValue = ExtractFieldFromJson(json, "eventType");
        if (string.IsNullOrEmpty(fieldValue))
        {
            return -1;
        }
        return int.Parse(fieldValue);
    }

    public static string ExtractFieldFromJson(string json, string fieldName)
    {
        string target = $"\"{fieldName}\":";
        int startIndex = json.IndexOf(target, StringComparison.OrdinalIgnoreCase);
        if (startIndex >= 0)
        {
            startIndex += target.Length;
            // Skip whitespace
            while (char.IsWhiteSpace(json[startIndex])) startIndex++;

            // Determine if the value is a string or number
            if (json[startIndex] == '"') // string value
            {
                startIndex++;
                int endIndex = json.IndexOf('"', startIndex);
                if (endIndex > startIndex)
                {
                    return json.Substring(startIndex, endIndex - startIndex);
                }
            }
            else // numeric value
            {
                int endIndex = json.IndexOfAny(new[] { ',', '}', ' ' }, startIndex);
                if (endIndex > startIndex)
                {
                    return json.Substring(startIndex, endIndex - startIndex).Trim();
                }
            }
        }
        return "";
    }
}