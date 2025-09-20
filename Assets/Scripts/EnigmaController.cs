using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using System;

public class EnigmaController : MonoBehaviour
{
    public AudioTest radio;
    public ControlPanel cp;
    [ContextMenu("Edit Station")]
    private void EditStation()
    {
        //Test
        CreateEnigma(187.7f, GenerateCodeList(true,true,5));
        CreateEnigma(89.0f, "f", "u", "c", "k");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cp.OnValid.AddListener(OnValid);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateEnigma(float frequency, params string[] codes)
    {
        if (codes == null) return;
        if (frequency < 50 || frequency > 250)
        {
            Debug.LogError("Frequenz ist außerhalb des Gültigen bereichs beim erstellen einer Enigma");
            return; 
        }
        List<string> sequence = new List<string>();
        // neuen Sender erstellen und die Frequenz zuweisen. 
        RadioFrequencyAudio channel = new RadioFrequencyAudio();
        channel.Frequency = frequency;

        Debug.Log("Frequenz:" + frequency + " Params:" + codes.ToString() + " Radio name: " + radio.name);

        // String Arr in List umwandeln
        sequence.AddRange(codes
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim()));

        //Controlpanel mit der List ausstatten
        cp.SetTargetSequence(sequence);


        //String liste in Char liste umwandeln und an das RadioFrequencyAudio / Sender übergeben.
        channel.Numbers = ToCharList(sequence);

        Debug.Log("Neue Station soll hinzugefügt werden");
        radio.AddFrequency(channel); // Sender hinzufügen todo: nach beenden der aufgab ggf entfernen
        Debug.Log("Neue station wurde hinzugefügt?");

    }


    // Hier einfügen was passieren soll wenn die Challenge gelößt ist
    private void OnValid()
    {
        Debug.Log("Enigma wurde gelöst"); // todo: was damit machen
    }

    public static List<char> ToCharList(List<string> src)
    {
        return src
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim()[0])   // erster Buchstabe nach Trim
            .ToList();
    }

    private static readonly System.Random _rng = new System.Random(); // eigener RNG (beeinflusst UnityEngine.Random nicht)


    // numbers: Ziffern 0–9 zulassen
    // chars:   Buchstaben a–z zulassen
    // length:  Anzahl der Einträge im Ergebnis-Array
    public static string[] GenerateCodeList(bool numbers, bool chars, int length)
    {
        if (!numbers && !chars || length <= 0)
            return Array.Empty<string>();

        const string digits = "0123456789";
        const string letters = "abcdefghijklmnopqrstuvwxyz";

        string pool = (numbers ? digits : string.Empty) + (chars ? letters : string.Empty);
        if (pool.Length == 0)
            return Array.Empty<string>();

        var result = new string[length];
        for (int i = 0; i < length; i++)
        {
            int idx = _rng.Next(pool.Length); // 0..pool.Length-1
            result[i] = pool[idx].ToString(); // ein Zeichen als String
        }
        return result;
    }

}


