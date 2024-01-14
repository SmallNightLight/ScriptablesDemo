using System.Collections.Generic;

public class Stacktrace
{
    private List<string> _messages = new List<string>();

    public void Add(string message) => _messages.Add(System.DateTime.Now.ToString("HH:mm:ss") + ": " + message);

    public void Clear() => _messages.Clear();

    public List<string> GetMessages() => _messages;
}