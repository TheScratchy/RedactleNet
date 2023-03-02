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
        private Dictionary<List<string>, int> _guessedWords;
        /// <summary>
        /// Dictonary representing the guessed words. Key is the word, value is the number of hits.
        /// </summary>
        public Dictionary<List<string>, int> GuessedWords { get { return _guessedWords; } }
        

        public Game()
        {
            _wp = new("https://en.wikipedia.org/wiki/United_States");
            _nameOfArticle = _wp.GetArticleName();
            _article = _wp.GetArticleContent();
            #region replacements
            _article = _article.Replace("(listen);", "");
            _article = _article.Replace("%", " percent");
            _article = _article.Replace("$", "");
            #endregion
            _encryptedArticle = EncryptArticle(_article);
        }

        private string EncryptArticle(string article)
        {
            string encryptedArticle = string.Empty;

            string word = string.Empty;
            foreach(char c in article)
            {
                if ((Char.IsLetter(c) || Char.IsDigit(c)) == false) 
                {
                    word = string.Empty;
                    encryptedArticle += c;
                }
                else
                {
                    encryptedArticle += "█";
                }
            }

            return encryptedArticle;
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
