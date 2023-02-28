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
            _article = _article.Replace("(listen);", "");
        }

        //private 
    }
}
