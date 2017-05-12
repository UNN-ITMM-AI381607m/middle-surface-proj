using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidSurfaceNameSpace.IO
{
   public class Html
    {
        string text, ideal;

        public Html(string pideal)
        {
            text = "<html>" +
                "<head>" +
                "<title>Результаты тестов</title>" +
                "</head>" +
                "<body>" +
                "<table border = \"1\">" +
                "<caption>Результаты тестов</caption>" +
                "<tr>" +
                "<th>Получилось</th>" +
                "<th>Должно было получиться</th>" +
                "</tr> ";
            ideal = pideal.Substring(0, pideal.Length - 4) + "ideal.png";
        }

        public void Add(string path)
        {
            text += "<tr><td><img src = \"" + path +
                    "\" width = \"500\" height = \"200\"></td>" +
                    "<td><img src = \"" + ideal +
                    "\" width = \"500\" height = \"200\"></td>" +
                    "</tr>";
        }

        public string Save()
        {
            text += "</table></body></html>";
            return text;
        }
    }
}
