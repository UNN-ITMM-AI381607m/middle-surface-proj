using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace Parser_Saver
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathToFile = Console.ReadLine();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Shape2D));
            FileStream fs = new FileStream(pathToFile, FileMode.Open);
            Shape2D shape = (Shape2D)xmlSerializer.Deserialize(fs);

            Console.WriteLine("Contours count: " + shape.Contour.Count());
            for (int i = 0; i < shape.Contour.Count(); i++)
            {
                Console.WriteLine(" Contour " + (i + 1) + ":");
                Console.WriteLine("     Points count: " + shape.Contour[i].Points.Count());
                for(int j = 0; j < shape.Contour[i].Points.Count(); j++)
                {
                    Console.WriteLine("         Point " + (j + 1) + ":");
                    Console.WriteLine("             X: " + shape.Contour[i].Points[j].X + "    Y: " + shape.Contour[i].Points[j].Y);
                }

                Console.WriteLine("     Segments count: " + shape.Contour[i].Segments.Count());
            }

            fs.Close();
        }
    }
}
