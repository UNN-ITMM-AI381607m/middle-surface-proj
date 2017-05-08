using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidSurfaceNameSpace.IO
{
    class Html
    {
        string text;

        public Html()
        {
            text = "<html>" +
                "< head >" +
                "< title > Результаты тестов </ title >" +
                "</ head >" +
                "< body >" +
                "< table border = \"1\" >" +
                "< caption > Результаты тестов </ caption >" +
                "< tr >" +
                "< th > Получилось </ th >" +
                "< th > Должно было получиться</ th >" +
                "</ tr > ";
        }

        public void Add(string path,string folder)
        {
            text += "< tr >< td >< img src = " + path +
                    " width = \"500\" height = \"200\" ></ td >" +
                    "< td >< img src = " + folder + "\\ideal.png" +
                    "width = \"500\" height = \"200\" ></ td >" +
                    "</ tr >";
        }

        public string Save()
        {
            text += "</ table ></ body ></ html >";
            return text;
        }
    }
}
