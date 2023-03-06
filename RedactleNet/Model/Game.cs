using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RedactleNet.Model
{
    internal class Game
    {
        private string _article;
        private string _encryptedArticle;
        public string EncryptedArticle { get { return _encryptedArticle; } }
        private string _nameOfArticle;
        private WebPage _wp;
        /// <summary>
        /// Dictonary representing the guessed words. Key is the word, value is the number of hits.
        /// </summary>
        private Dictionary<string, int> _guessedWords;
        /// <summary>
        /// Dictonary representing the guessed words. Key is the word, value is the number of hits.
        /// </summary>
        public Dictionary<string, int> GuessedWords { get { return _guessedWords; } }
        private List<string> _startingWords = new()
        {
                    "the", "at", "there", 
                    "of", "be", "use", "her", "than",
                    "and", "this", "an", "would", "until",
                    "a", "have", "each", "were",
                    "to", "from", "which", "been",
                    "in", "or", "do", "into", "who", "how",
                    "that", "by", "if", "but", "will", "not",
                    "what",  "for", "on",
                    "all", "about", "go", "out", "as", "with",
                    "then", "no", "may", "so", "such",
                    "now", "during", "after", "was",
                    "because", "unlike", "unless", "through",
                    "onto", "when", "unto", "beyond", "off",
                    "since", "along", "while"
         };

        public Game()
        {
            _guessedWords = new();
            _wp = new("https://en.wikipedia.org/wiki/United_States");
            _nameOfArticle = _wp.GetArticleName();
            _article = _wp.GetArticleContent();
            #region replacements
            _article = _article.Replace("(listen);", "");
            _article = _article.Replace("%", " percent");
            _article = _article.Replace("$", "");
            #endregion
            _article = _nameOfArticle + "\n\n" + _article;
            _encryptedArticle = EncryptArticle(_article);
        }

        /// <summary>
        /// Encrypts the article for the first time, excludes words stored in "_startingWords".
        /// </summary>
        /// <param name="article"></param>
        /// <returns>Returns encrypted article, with black squares instead of chars.</returns>
        private string EncryptArticle(string article)
        {
            string encryptedArticle = string.Empty;

            string word = string.Empty;
            foreach (char c in article)
            {
                if (Char.IsLetter(c) || Char.IsDigit(c))
                {
                    word += c;
                    continue;
                }

                if (word == "A")
                {
                    encryptedArticle += "█";
                }
                else if (_startingWords.Contains(word.ToLower()))
                    encryptedArticle += word;
                else
                    foreach (char c2 in word)
                        encryptedArticle += "█";
                word = string.Empty;
                encryptedArticle += c;

            }

            return encryptedArticle;
        }

        public void GuessWord(string word)
        {
            // the word was already guessed or it is included in the starting words
            if (_guessedWords.ContainsKey(word) || _startingWords.Contains(word))
                return;

            SearchForWordInArticle(word);
        }

        private void SearchForWordInArticle(string guessedWord)
        {
            string word = string.Empty;
            int indexToReplace = 0;
            int guessedTimes = 0;

            foreach (char c in _article)
            {
                indexToReplace++;
                if (Char.IsLetter(c) || Char.IsDigit(c))
                {
                    word += c;
                    continue;
                }
                if (guessedWord.Equals(word.ToLower()))
                {
                    _encryptedArticle = _encryptedArticle.Remove(indexToReplace - 1 - word.Length, word.Length).Insert(indexToReplace - 1 - word.Length, word);
                    guessedTimes++;
                }
                word = string.Empty;
                
            }
            _guessedWords[guessedWord] = guessedTimes;
        }

        //private void ReplaceDollarSign(ref string article)
        //{
        //    bool flag = true;
        //    string number = string.Empty, word = string.Empty;
        //    foreach(char c in article)
        //    {
        //        if (c != '$' && flag)
        //            continue;

        //        flag = false;
        //        if (Char.IsLetter(c))
        //            word += c;
        //        if (Char.IsNumber(c))
        //            number += c;

        //        if (word == "bilion" || word == "milion" || word == "trilion")

        //    }
        //}
    }
}
