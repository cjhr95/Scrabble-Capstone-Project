using System;
using System.Collections.Generic;

namespace Assets
{
  public enum PoolStyle
  {
    Scrabble,             // Standard Scrabble style
    WeightedDistribution, // Random distribution based on letter frequency
    TrueRandom            // Completely random selection.
  }
  public static class LetterPoolManager
  {
    public const int POOL_SIZE = 100; // Letters to start the game with.
    public static List<char> letters; // The list of letters.

    // A helper variable.
    public static char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    
    // Description: Fills the letter pool with three different styles to choose from.
    // Parameter:   The pool style (from enum).
    public static void FillPool(PoolStyle style)
    {
      letters = new List<char>(POOL_SIZE);
      Random rand = new Random();
      switch (style)
      {
        case PoolStyle.Scrabble:
          for (int i = 0; i < 12; i++)
          {
            letters.Add('E');
            if (i < 9) letters.AddRange(new char[] {'A', 'I'});
            if (i < 8) letters.Add('O');
            if (i < 6) letters.AddRange(new char[] { 'N', 'R', 'T' });
            if (i < 4) letters.AddRange(new char[] { 'L', 'S', 'U' });
          }
          break;
        case PoolStyle.WeightedDistribution:
          char[] distribution = GetAlphabetDistributionArray();
          for (int i = 0; i < POOL_SIZE; i++)
          {
            letters.Add(distribution[rand.Next(0, distribution.Length)]);
          }
          break;
        case PoolStyle.TrueRandom:
          for (int i = 0; i < POOL_SIZE; i++){
            letters.Add((char)rand.Next(65, 91));
          }
          break;
      }
    }


    // Description: Gets a random letter from the pool of letters.
    // Returns:     A string containing the letter from the pool.
    public static string RetrieveLetterFromPool()
    {
      Random rand = new Random();
      int rand_loc = rand.Next(letters.Count);
      string rand_char = letters[rand_loc].ToString();
      letters.RemoveAt(rand_loc);
      return rand_char;
    }

    public static int GetCurrentPoolSize()
    {
      return letters.Count;
    }

    // Description: Obtains the number of a particular letter
    //              left in the pool.
    // Returns:     The letter count.
    public static int GetLetterCount(char letter)
    {
      int count = 0;
      for (int i = 0; i < letters.Count; i++) if (letters[i] == letter) count++;
      return count;
    }

    private static char[] GetAlphabetDistributionArray()
    {
      const int ArrSize = 509;
      char[] distr = new char[ArrSize];

      // I'm sorry.
      // Don't know how to better do this.
      int index = 0;
      for (int i = 0; i < 57; i++) distr[index++] = 'E';
      for (int i = 0; i < 43; i++) distr[index++] = 'A';
      for (int i = 0; i < 39; i++) distr[index++] = 'R';
      for (int i = 0; i < 38; i++) distr[index++] = 'I';
      for (int i = 0; i < 37; i++) distr[index++] = 'O';
      for (int i = 0; i < 35; i++) distr[index++] = 'T';
      for (int i = 0; i < 34; i++) distr[index++] = 'N';
      for (int i = 0; i < 29; i++) distr[index++] = 'S';
      for (int i = 0; i < 28; i++) distr[index++] = 'L';
      for (int i = 0; i < 23; i++) distr[index++] = 'C';
      for (int i = 0; i < 19; i++) distr[index++] = 'U';
      for (int i = 0; i < 17; i++) distr[index++] = 'D';
      for (int i = 0; i < 16; i++) distr[index++] = 'P';
      for (int i = 0; i < 15; i++) distr[index++] = 'M';
      for (int i = 0; i < 15; i++) distr[index++] = 'H';
      for (int i = 0; i < 13; i++) distr[index++] = 'G';
      for (int i = 0; i < 11; i++) distr[index++] = 'B';
      for (int i = 0; i < 9; i++) distr[index++] = 'F';
      for (int i = 0; i < 9; i++) distr[index++] = 'Y';
      for (int i = 0; i < 7; i++) distr[index++] = 'W';
      for (int i = 0; i < 6; i++) distr[index++] = 'K';
      for (int i = 0; i < 5; i++) distr[index++] = 'V';
      for (int i = 0; i < 1; i++) distr[index++] = 'X';
      for (int i = 0; i < 1; i++) distr[index++] = 'Z';
      for (int i = 0; i < 1; i++) distr[index++] = 'J';
      for (int i = 0; i < 1; i++) distr[index++] = 'Q';

      return distr;
    }
  }
}
