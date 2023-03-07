using System.Collections;
using System.Collections.Generic;
using Game;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestDictionary
{
  [Test]
  public void TestValidateWord()
  {
    string[] words = { "BANANA", "BANDANA", "APPLE", "TABLE", "COMPUTER", "SCIENCE"};

    foreach (string w in words)
    {
      Assert.That(GameDictionary.ValidateWord(w) == true, $"Dictionary failed to accept {w}");
    }

    string[] badWords = { "", "A", "[", ".12.3.124.1", "0023496340698329065823069845690" };
    foreach (string w in badWords)
    {
      Assert.That(GameDictionary.ValidateWord(w) == false, $"Dictionary failed to deny {w}");
    }
  }

  [Test]
  public void TestGenerateWord()
  {
    for (int i = GameDictionary.MinWordLength; i < 7; i++)
    {
      for (int j = 0; j < 10; j++)
      {
        Assert.That(GameDictionary.GenerateWord(i) != "", $"Dictionary failed to create word of length {i}");
      }
    }
  }

  [Test]
  public void TestGenerateWordFromList()
  {
    // Good words
    char[] apple = "applkeq".ToCharArray();
    char[] banana = "baonana".ToCharArray();
    char[] bandana = "bandana".ToCharArray();
    char[] table = "tablebb".ToCharArray();
    char[] science = "science".ToCharArray();

    Assert.That(GameDictionary.GenerateWordFromList(apple) != "");
    Assert.That(GameDictionary.GenerateWordFromList(banana) != "");
    Assert.That(GameDictionary.GenerateWordFromList(bandana) != "");
    Assert.That(GameDictionary.GenerateWordFromList(table) != "");
    Assert.That(GameDictionary.GenerateWordFromList(science) != "");

    // Bad words
    char[] sad = "sad!?!?".ToCharArray();
    Assert.That(GameDictionary.GenerateWordFromList(sad) == "");

    // No words
    char[] zy = "zyzyzyz".ToCharArray();
    Assert.That(GameDictionary.GenerateWordFromList(zy) == "");
  }
}
