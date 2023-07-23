using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;

public class grandPianoScript : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable ModuleSelectable;
    public KMSelectable[] Keys;
    public GameObject EntireThing;
    public TextMesh Number;
    public Material[] KeyMats;
    public GameObject[] KeyObjs;

    KeyCode[] Board =
	{
        KeyCode.Space, KeyCode.W, KeyCode.S, KeyCode.R,
		KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0,
		KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9, KeyCode.Keypad0,
	};
    bool Focused = false;
    bool Grown = false;
    bool Stretching = false;
    float Timer = 0f;
    float Height = 0f;
    int Inputs = 0;

    string[] Piano = { "A0", "A#0", "B0", "C1", "C#1", "D1", "D#1", "E1", "F1", "F#1", "G1", "G#1", "A1", "A#1", "B1", "C2", "C#2", "D2", "D#2", "E2", "F2", "F#2", "G2", "G#2", "A2", "A#2", "B2", "C3", "C#3", "D3", "D#3", "E3", "F3", "F#3", "G3", "G#3", "A3", "A#3", "B3", "C4", "C#4", "D4", "D#4", "E4", "F4", "F#4", "G4", "G#4", "A4", "A#4", "B4", "C5", "C#5", "D5", "D#5", "E5", "F5", "F#5", "G5", "G#5", "A5", "A#5", "B5", "C6", "C#6", "D6", "D#6", "E6", "F6", "F#6", "G6", "G#6", "A6", "A#6", "B6", "C7", "C#7", "D7", "D#7", "E7", "F7", "F#7", "G7", "G#7", "A7", "A#7", "B7", "C8" };
    string[] White = { "A0", "B0", "C1", "D1", "E1", "F1", "G1", "A1", "B1", "C2", "D2", "E2", "F2", "G2", "A2", "B2", "C3", "D3", "E3", "F3", "G3", "A3", "B3", "C4", "D4", "E4", "F4", "G4", "A4", "B4", "C5", "D5", "E5", "F5", "G5", "A5", "B5", "C6", "D6", "E6", "F6", "G6", "A6", "B6", "C7", "D7", "E7", "F7", "G7", "A7", "B7", "C8" };
    string[] Black = { "A#0", "C#1", "D#1", "F#1", "G#1", "A#1", "C#2", "D#2", "F#2", "G#2", "A#2", "C#3", "D#3", "F#3", "G#3", "A#3", "C#4", "D#4", "F#4", "G#4", "A#4", "C#5", "D#5", "F#5", "G#5", "A#5", "C#6", "D#6", "F#6", "G#6", "A#6", "C#7", "D#7", "F#7", "G#7", "A#7" };
    string[] Cards = { "As", "4h", "7c", "Xd", "Ks", "3h", "6c", "9d", "Qs", "2h", "5c", "8d", "Js", "Ah", "4c", "7d", "Xs", "Kh", "3c", "6d", "9s", "Qh", "2c", "5d", "8s", "Jh", "Ac", "4d", "7s", "Xh", "Kc", "3d", "6s", "9h", "Qc", "2d", "5s", "8h", "Jc", "Ad", "4s", "7h", "Xc", "Kd", "3s", "6h", "9c", "Qd", "2s", "5h", "8c", "Jd" };
    string[] Hands = { "4K", "2P;s", "FH;h", "P;s", "FH;d", "3K;c", "F;2", "P;d", "F;3", "P;h", "F;4", "2P;h", "F;A", "P;c", "F;J", "3K;d", "F;5", "RF", "F;X", "SF", "F;6", "2P;c", "F;Q", "S;d", "F;K", "3K;s", "F;7", "S;s", "F;8", "S;c", "F;9", "2P;d", "FH;c", "S;h", "FH;s", "3K;h" };
    string[] Modif = { "234;b", "123;#b", "1234;#b", "234;#b", "2345;#b", "345;#b", "12;b", "12;#b", "123;b", "12;b#", "23;b", "23;#b", "12;#", "123;#", "23;#", "23;b#", "1234;b", "1234;#", "234;#", "2345;#", "2345;b", "34;#b", "34;#", "345;#", "45;#", "34;b#", "34;b", "45;#b", "345;b", "45;b#", "45;b", "123;b#", "1234;b#", "234;b#", "2345;b#", "345;b#" };
    
    string[][] Deck = {
        new string[] { "", "", "", "", "" },
        new string[] { "", "", "", "", "" },
        new string[] { "", "", "", "", "" },
        new string[] { "", "", "", "", "" },
        new string[] { "", "", "", "", "" }
    };
    string[] Used = { "", "", "", "", "" };
    int[] SolutionSeq = { -1, -1, -1, -1, -1 };
    int handType = -1;
    string segment = "";

    int[][] Duck = {
        new int[] { -1, -1, -1, -1, -1 },
        new int[] { -1, -1, -1, -1, -1 },
        new int[] { -1, -1, -1, -1, -1 },
        new int[] { -1, -1, -1, -1, -1 },
        new int[] { -1, -1, -1, -1, -1 }
    };

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake () {
        moduleId = moduleIdCounter++;

        if (Application.isEditor) { Focused = true; }
        ModuleSelectable.OnFocus += delegate () { Focused = true; };
        ModuleSelectable.OnDefocus += delegate () { Focused = false; };

        foreach (KMSelectable key in Keys) {
            key.OnInteract += delegate () { keyPress(key); return false; };
        }
    }

    // Use this for initialization
    void Start () {
        Deck[4][4] = Cards.PickRandom();
        CreateHand(4);
        for (int m = 0; m < 4; m++) {
            CreateHand(m);
        }
        string temp = "";
        string[] hset = {"", "", "", ""};
        for (int x = 0; x < 5; x++) {
            if (x != 4) {
                for (int y = 0; y < 4; y++) {
                    temp = White[Array.IndexOf(Cards, Deck[x][y])];
                    hset[y] = temp;
                    Duck[x][y] = Array.IndexOf(Piano, temp);
                }
            }
            temp = White[Array.IndexOf(Cards, Deck[x][4])];
            SolutionSeq[x] = Array.IndexOf(Piano, temp);
            temp = Black[Array.IndexOf(Hands, Used[x])];
            Duck[x][4] = Array.IndexOf(Piano, temp);
            Debug.LogFormat("[Grand Piano #{0}] Set {1}: {2}", moduleId, x + 1, (x == 4 ? temp : hset.Join(", ") + ", " + temp));
            Debug.LogFormat("[Grand Piano #{0}] {1}Rule: {2}", moduleId, (x == 4 ? "" : LogDeck(x)), Used[x].Replace(";", "(") + ((Used[x].Contains(";")) ? ")" : ""));
            Debug.LogFormat("[Grand Piano #{0}] {1} completes the hand", moduleId, Deck[x][4]);
        }
        Debug.LogFormat("[Grand Piano #{0}] Full Hand: {1}", moduleId, Deck[4].Join(", "));
        string[] vset = {"", "", "", "", ""};
        for (int v = 0; v < 5; v++) {
            vset[v] = Piano[SolutionSeq[v]];
        }
        Debug.LogFormat("[Grand Piano #{0}] Initial Notes: {1}", moduleId, vset.Join(", "));
        temp = Black[Array.IndexOf(Hands, Used[4])];
        int tempi = Array.IndexOf(Black, temp);
        temp = Modif[tempi];
        Debug.LogFormat("[Grand Piano #{0}] Finish Code: {1}", moduleId, temp);
        Modify(temp);
        for (int w = 0; w < 5; w++) {
            vset[w] = Piano[SolutionSeq[w]];
        }
        Debug.LogFormat("[Grand Piano #{0}] Final Notes: {1}", moduleId, vset.Join(", "));
	}

    void CreateHand (int h) {
        string finalCard = Deck[h][4];
        string aCard = " ";
        string[] otherCards = {"", "", "", ""};
        string straightLine = "A23456789XJQK";
        TryAgain:
        handType = UnityEngine.Random.Range(0,27);
        switch (handType) {
            case 0: 
                if (!"AXJQK".Contains(finalCard[0])) { goto TryAgain; }
                /*Royal Flush*/
                for (int c = 0; c < 4; c++) {
                    ROYAL_FLUSH_retry:
                    aCard = "" + "AXJQK".PickRandom() + finalCard[1];
                    if (otherCards.Contains(aCard) || finalCard == aCard) {
                        goto ROYAL_FLUSH_retry;
                    }
                    otherCards[c] = aCard;
                }
                Used[h] = "RF";
            break;
            case 1: 
                if ("AK".Contains(finalCard[0])) { goto TryAgain; }
                /*Straight Flush*/
                segment = "#####";
                while (!(segment[1] == finalCard[0] || segment[2] == finalCard[0] || segment[3] == finalCard[0])) {
                    segment = straightLine.Substring(UnityEngine.Random.Range(0,9), 5);
                }
                segment = segment.Replace(finalCard[0].ToString(), "".ToString());
                for (int c = 0; c < 4; c++) {
                    aCard = "" + segment[c] + finalCard[1];
                    otherCards[c] = aCard;
                }
                Used[h] = "SF";
            break;
            case 2: 
                /*Four of a Kind*/
                for (int c = 0; c < 3; c++) {
                    FOUR_KIND_retry:
                    aCard = "" + finalCard[0] + "shcd".PickRandom();
                    if (otherCards.Contains(aCard) || finalCard == aCard) {
                        goto FOUR_KIND_retry;
                    }
                    otherCards[c] = aCard;
                }
                while (aCard[0] == finalCard[0]) {
                    aCard = Cards.PickRandom();
                }
                otherCards[3] = aCard;
                Used[h] = "4K";
            break;
            case 3: case 4: case 5: case 6:
                /*Full House*/
                while (!(aCard[0] == finalCard[0]) || aCard == finalCard) {
                    aCard = Cards.PickRandom();
                }
                otherCards[0] = aCard;
                while ((aCard[0] == finalCard[0])) {
                    aCard = Cards.PickRandom();
                }
                otherCards[1] = aCard;
                for (int c = 2; c < 4; c++) {
                    FULL_HOUSE_retry:
                    aCard = "" + otherCards[1][0] + "shcd".PickRandom();
                    if (otherCards.Contains(aCard)) {
                        goto FULL_HOUSE_retry;
                    }
                    otherCards[c] = aCard;
                }
                Used[h] = "FH;" + finalCard[1];
            break;
            case 7: case 8: case 9: case 10:
                /*Flush*/
                for (int c = 0; c < 4; c++) {
                    FLUSH_retry:
                    aCard = "" + straightLine.PickRandom() + finalCard[1];
                    if (otherCards.Contains(aCard) || finalCard == aCard) {
                        goto FLUSH_retry;
                    }
                    otherCards[c] = aCard;
                }
                Used[h] = "F;" + finalCard[0];
            break;
            case 11: case 12: case 13: case 14:
                /*Straight*/
                if ("AK".Contains(finalCard[0])) { goto TryAgain; }
                segment = "#####";
                while (!(segment[1] == finalCard[0] || segment[2] == finalCard[0] || segment[3] == finalCard[0])) {
                    segment = straightLine.Substring(UnityEngine.Random.Range(0,9), 5);
                }
                segment = segment.Replace(finalCard[0].ToString(), "".ToString());
                for (int c = 0; c < 4; c++) {
                    aCard = "" + segment[c]  + "shcd".Replace(finalCard[1].ToString(), "".ToString()).PickRandom();
                    otherCards[c] = aCard;
                }
                Used[h] = "S;" + finalCard[1];
            break;
            case 15: case 16: case 17: case 18:
                /*Three of a Kind*/
                for (int c = 0; c < 2; c++) {
                    THREE_KIND_retry:
                    aCard = "" + finalCard[0] + "shcd".PickRandom();
                    if (otherCards.Contains(aCard) || finalCard == aCard) {
                        goto THREE_KIND_retry;
                    }
                    otherCards[c] = aCard;
                }
                while (aCard[0] == finalCard[0]) {
                    aCard = Cards.PickRandom();
                }
                otherCards[2] = aCard;
                while (aCard[0] == finalCard[0] || aCard[0] == otherCards[2][0]) {
                    aCard = Cards.PickRandom();
                }
                otherCards[3] = aCard;
                Used[h] = "3K;" + finalCard[1];
            break;
            case 19: case 20: case 21: case 22:
                /*Two Pairs*/
                while (aCard[0] != finalCard[0] || finalCard == aCard) {
                    aCard = Cards.PickRandom();
                }
                otherCards[0] = aCard;
                while (aCard[0] == finalCard[0]) {
                    aCard = Cards.PickRandom();
                }
                otherCards[1] = aCard;
                while (aCard[0] == finalCard[0] || aCard[0] != otherCards[1][0] || aCard == otherCards[1]) {
                    aCard = Cards.PickRandom();
                }
                otherCards[2] = aCard;
                while (aCard[1] != finalCard[1] || aCard[0] == finalCard[0] || aCard[0] == otherCards[1][0]) {
                    aCard = Cards.PickRandom();
                }
                otherCards[3] = aCard;
                Used[h] = "2P;" + finalCard[1];
            break;
            case 23: case 24: case 25: case 26:
                /*Pair*/
                aCard = finalCard;
                switch (aCard[1]) {
                    case 's': otherCards[0] = aCard.Replace('s', 'c'); break;
                    case 'h': otherCards[0] = aCard.Replace('h', 'd'); break;
                    case 'c': otherCards[0] = aCard.Replace('c', 's'); break;
                    case 'd': otherCards[0] = aCard.Replace('d', 'h'); break;
                }
                for (int c = 1; c < 4; c++) {
                    PAIR_retry:
                    aCard = Cards.PickRandom();
                    for (int d = 0; d < c; d++) {
                        if (aCard[0] == otherCards[d][0] || aCard[1] == otherCards[d][1]) {
                            goto PAIR_retry;
                        }
                    }
                    otherCards[c] = aCard;
                }
                Used[h] = "P;" + finalCard[1];
            break;
        }
        otherCards = otherCards.Shuffle();
        for (int k = 0; k < 4; k++) {
            Deck[h][k] = otherCards[k];
            if (h == 4) {
                Deck[k][4] = Deck[4][k];
            }
        }
    }

    void Update () {
        if (!Focused) { return; }
        for (int j = 0; j < Board.Count(); j++) {
            if (Input.GetKeyDown(Board[j])) {
                switch (j) {
                    case 0: ToggleSize(); break;
                    case 1: Height += 0.02f; break;
                    case 2: Height -= 0.02f; break;
                    case 3: Height = 0f; break;
                    case 4: case 14: UpdateVisuals(1); break;
                    case 5: case 15: UpdateVisuals(2); break;
                    case 6: case 16: UpdateVisuals(3); break;
                    case 7: case 17: UpdateVisuals(4); break;
                    case 8: case 18: UpdateVisuals(5); break;
                    default: UpdateVisuals(0); break;
                }
                EntireThing.transform.localPosition = new Vector3(0f, Height, 0f);
            }
        }
	}

    void ToggleSize () {
        if (Stretching) { return; }
        Stretching = true;
        Grown = !Grown;
        if (Grown) {
            Audio.PlaySoundAtTransform("SLIDEUP", transform);
            StartCoroutine(Grow());
        } else {
            Audio.PlaySoundAtTransform("SLIDEDOWN", transform);
            StartCoroutine(Shrink());
        }
    }

    private IEnumerator Grow () {
        Timer = 0f;
        while(Timer < 0.32f) {
            EntireThing.transform.localScale = new Vector3((float)18.75f * Timer + 1f, 1f, 1f);
            yield return null;
            Timer += Time.deltaTime;
        }
        EntireThing.transform.localScale = new Vector3(7f, 1f, 1f);
        Stretching = false;
    }

    private IEnumerator Shrink () {
        Timer = 0.32f;
        while(Timer > 0f) {
            EntireThing.transform.localScale = new Vector3((float)18.75f * Timer + 1f, 1f, 1f);
            yield return null;
            Timer -= Time.deltaTime;
        }
        EntireThing.transform.localScale = new Vector3(1f, 1f, 1f);
        Stretching = false;
    }

    void UpdateVisuals (int d) {
        if (moduleSolved) { return; }
        Number.text = d.ToString();
        for (int p = 0; p < Piano.Length; p++) {
            KeyObjs[p].GetComponent<Renderer>().material = KeyMats[((Piano[p][1].ToString() == "#".ToString()) ? 1 : 0)];
        }
        if (d != 0) {
            for (int q = 0; q < 5; q++) {
                if (d == 5 && q != 4) { continue; } 
                KeyObjs[Duck[d-1][q]].GetComponent<Renderer>().material = KeyMats[(Piano[Duck[d-1][q]][1].ToString() == "#".ToString() ? 3 : 2)];
            }
        }
    }

    string LogDeck (int z) {
        return ("Hand: " + Deck[z][0] + ", " + Deck[z][1] + ", " + Deck[z][2] + ", " + Deck[z][3] + " / ");
    }

    void Modify (string rule) {
        string[] halves = { "", "" };
        int numpty = 0;
        halves[0] = rule.Split(';')[0];
        halves[1] = rule.Split(';')[1];
        halves[1] += (halves[1].Length == 1 ? halves[1] : "");
        for (int d = 0; d < halves[0].Length; d++) {
            numpty = "12345".IndexOf(halves[0][d]);
            if (halves[1][d%2] == 'b') {
                if (SolutionSeq[numpty] == 0) { continue; }
                if (Piano[SolutionSeq[numpty]-1].Contains("#")) {
                    SolutionSeq[numpty] -= 1;
                }
            } else {
                if (SolutionSeq[numpty] == 87) { continue; }
                if (Piano[SolutionSeq[numpty]+1].Contains("#")) {
                    SolutionSeq[numpty] += 1;
                }
            }
        }
    }

    void keyPress (KMSelectable key) {
        for (int i = 0; i < 88; i++) {
            if (key == Keys[i]) {
                Audio.PlaySoundAtTransform(Piano[i], transform);
                if (moduleSolved) { return; }
                if (SolutionSeq[Inputs] == i) {
                    Debug.LogFormat("[Grand Piano #{0}] Pressed {1}, correct.", moduleId, Piano[i]);
                    Inputs += 1;
                    if (Inputs == 5) {
                        Debug.LogFormat("[Grand Piano #{0}] Pressed all 5 keys. Module solved.", moduleId);
                        GetComponent<KMBombModule>().HandlePass();
                        UpdateVisuals(0);
                        moduleSolved = true;
                        StartCoroutine(TextGen());
                    }
                } else {
                    Debug.LogFormat("[Grand Piano #{0}] Pressed {1}, that's not correct. Strike!", moduleId, Piano[i]);
                    GetComponent<KMBombModule>().HandleStrike();
                    Inputs = 0;
                }
            }
        }
    }

    private IEnumerator TextGen () {
        for (int t = 1; t < 7; t++) {
            yield return new WaitForSeconds(0.05f);
            Number.text = "Solved".Substring(0, t);
        }
    }

    //twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} cycle [Cycles through all sets of notes] | !{0} set 2 [Views a specific set of notes] | !{0} press B0 G#6 F4 [Presses the specified keys (note that flats are not real)]";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        if (Regex.IsMatch(command, @"^\s*cycle\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            for (int i = 0; i < 5; i++)
            {
                UpdateVisuals(i + 1);
                yield return "trywaitcancel 3";
            }
            yield break;
        }
        string[] parameters = command.Split(' ');
        if (Regex.IsMatch(parameters[0], @"^\s*set\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            if (parameters.Length == 1)
                yield return "sendtochaterror Please specify a set of notes to view!";
            else if (parameters.Length > 2)
                yield return "sendtochaterror Too many parameters!";
            else
            {
                if (!parameters[1].EqualsAny("1", "2", "3", "4", "5"))
                {
                    yield return "sendtochaterror!f The specified set of notes '" + parameters[1] + "' is invalid!";
                    yield break;
                }
                yield return null;
                UpdateVisuals(int.Parse(parameters[1]));
            }
            yield break;
        }
        if (Regex.IsMatch(parameters[0], @"^\s*press\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            if (parameters.Length == 1)
                yield return "sendtochaterror Please specify at least 1 key to press!";
            else
            {
                for (int i = 1; i < parameters.Length; i++)
                {
                    if (!Piano.Contains(parameters[i].ToUpperInvariant()))
                    {
                        yield return "sendtochaterror!f The specified key '" + parameters[i] + "' is invalid!";
                        yield break;
                    }
                }
                yield return null;
                for (int i = 1; i < parameters.Length; i++)
                {
                    Keys[Array.IndexOf(Piano, parameters[i].ToUpperInvariant())].OnInteract();
                    yield return new WaitForSeconds(.2f);
                }
            }
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        int start = Inputs;
        for (int i = start; i < 5; i++)
        {
            Keys[SolutionSeq[i]].OnInteract();
            yield return new WaitForSeconds(.2f);
        }
    }
}
