using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UIElements;

namespace Game
{
  public static class GameDictionary
  {
    const string DictionaryPath = "./Assets/ScrabbleDictionary.txt";
    public const int MinWordLength = 3;
    private static List<string> words;

    public static void InitializeDictionary()
    {
      if (words is null)
      {
        words = new List<string>();
        using (StreamReader sr = new StreamReader(DictionaryPath))
        {
          while (sr.Peek() >= 0) words.Add(sr.ReadLine());
        }
      }
    }

    // Description: Checks the word against the dictionary.
    // Parameters:  word - The word to be checked.
    // Returns:     true if the word exists, false otherwise.
    public static bool ValidateWord(string word)
    {
      if (word.Length < MinWordLength) return false;
      foreach (string w in words)
      {
        if (word == w) return true;
      }
      return false;

      //return File.ReadAllText(DictionaryPath).Contains(word);
    }

    // Description: Finds a word of the given length from the
    //              dictionary.
    // Parameters:  length - the length of the desired word.
    // Returns:     Returns a valid word of the given length or
    //              "" if no word exists of the given length.
    public static string GenerateWord(int length)
    {
      using (StreamReader sr = new StreamReader(DictionaryPath))
      {
        List<string> words = new List<string>();
        while (sr.Peek() >= 0)
        {
          if (sr.ReadLine().Length == length) words.Add(sr.ReadLine());
        }
        if (words.Count > 0)
        {
          return words[new Random().Next(words.Count)];
        }
        else
        {
          return "";
        }
      }
    }


    // Description: Generates a word given a set of letters
    // Parameters:  letters - the char array of letters
    // Returns:     returns a valid word or "" if no word is able to be made.
    public static string GenerateWordFromList(char[] letters)
    {
      return "";
    }
  }
}
