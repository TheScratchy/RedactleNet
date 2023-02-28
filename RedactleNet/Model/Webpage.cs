using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RedactleNet.Model
{
    public class WebPage
    {
        public string WebData { get; private set; }
        
        public WebPage(string webAddress)
        {
            using var client = new HttpClient();

            var content = client.GetStringAsync(webAddress);
            WebData = content.Result;
        }
         
        public string GetArticleName()
        {
            string artName = string.Empty;
            using (StringReader reader = new StringReader(WebData))
            {
                do
                {
                    artName = reader.ReadLine();
                    if (artName != null)
                    {
                        if (artName.StartsWith("<title>"))
                        {
                            artName = artName.Replace("<title>", string.Empty);
                            artName = artName.Replace(" - Wikipedia</title>", string.Empty);
                            break;
                        }
                    }
                } while (artName != null);
            }
            return artName;
        }
        public string GetArticleContent()
        {
            string artContent = string.Empty;
            using (StringReader reader = new StringReader(WebData))
            {
                string tempArtCont;
                do
                {
                    tempArtCont = reader.ReadLine();
                    if (tempArtCont == null)
                        break;

                    if (tempArtCont.StartsWith("<p>") == false)
                        continue;

                    bool isHTMLStyleMarked = false;
                    string ASCIINumbers = string.Empty;
                    foreach (char c in tempArtCont)
                    {
                    # region getting rid of HTML style formating.
                        // getting rid of HTML style formating.
                        if (c == '<')
                            isHTMLStyleMarked = true;
                        else if (c == '>')
                        {
                            isHTMLStyleMarked = false;
                            continue; // necessary, since it would add '>' into the text.
                        }

                        if (isHTMLStyleMarked)
                            continue;
                        #endregion

                        #region deleting "&#32;" and so on.
                        if (c == '&')
                        {
                            ASCIINumbers += c;
                            continue;
                        }
                        else if (c == '#' && ASCIINumbers == "&")
                        {
                            ASCIINumbers += c;
                            continue;
                        }
                        if (ASCIINumbers.Contains("&#"))
                        {
                            ASCIINumbers += c;
                            if (ASCIINumbers.Contains("160;")) // non-brekable space
                            {
                                ASCIINumbers = string.Empty;
                                artContent += " ";
                                continue;
                            }
                            if (ASCIINumbers.Contains("8211;")) // sign "-"
                            {
                                ASCIINumbers = string.Empty;
                                artContent += "- ";
                                continue;
                            }
                            else if (ASCIINumbers.Contains("&#91;"))
                            {
                                if (ASCIINumbers.Contains("93;"))
                                    ASCIINumbers = string.Empty;
                                continue;
                            }
                            else if (c == ';' && ASCIINumbers.Contains("&#"))
                            {
                                ASCIINumbers= string.Empty;
                                continue;
                            }    
                            continue;
                        }
                        #endregion

                        artContent += c;
                    }
                    artContent += "\n";
                } while (tempArtCont != null);
            }
            return artContent;

        }

    }
}
