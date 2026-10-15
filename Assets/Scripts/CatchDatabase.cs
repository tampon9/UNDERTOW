using UnityEngine;

public enum CatchTone
{
    Neutral,      // «И вот так ты представлял свою жизнь?»
    Ironic,       // «Поздравляю — ты поймал понедельник»
    Melancholic   // «Вода не помнит, сколько раз ты приходил»
}

[System.Serializable]
public class CatchEntry
{
    public string id;
    [TextArea] public string phrase;
    public Sprite silhouette;
    public CatchTone tone;
}

/// <summary>
/// Записи идут строго по порядку — никакого рандома. Располагай их в инспекторе
/// в том порядке, в котором должны попадаться игроку (используй это, чтобы
/// осознанно чередовать тон: нейтрально → иронично → меланхолично и т.д.).
/// </summary>
[CreateAssetMenu(fileName = "CatchDatabase", menuName = "Undertow/Catch Database")]
public class CatchDatabase : ScriptableObject
{
    public CatchEntry[] entries;

    public int Count => entries != null ? entries.Length : 0;

    public CatchEntry GetEntry(int index)
    {
        if (entries == null || index < 0 || index >= entries.Length) return null;
        return entries[index];
    }
}