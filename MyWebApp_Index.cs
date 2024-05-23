using System.Diagnostics;
using System.Net;

namespace WebAppServer
{
    internal class MyWebApp_Index : AServerBase
    {

        public MyWebApp_Index(string server = "localhost", string route="/",int port = 5080) :base( server,route , port)
        {
            Console.WriteLine("Главная страница запущена...");
        }

        public override string ProcessRequest(HttpListenerRequest request, HttpListenerResponse response)
        {

            Stopwatch stopwatch = Stopwatch.StartNew();

            response.StatusCode = 200;
            response.ContentType = "text/html";

            string title = "Тестовое приложение";

            StreamWriter writer = new StreamWriter(response.OutputStream);
            writer.WriteLine("<!DOCTYPE html> <html lang=ru>");
            writer.WriteLine("<head>");
            writer.WriteLine("<meta charset=\"utf-8\">");
            writer.WriteLine("<link rel=\"stylesheet\" href=\"/css/application.css\" type=\"text/css\">");
            writer.WriteLine($"<title> {title}</title>");
            writer.WriteLine("<link rel=\"icon\" href = \"/favicon.ico\"/>");
            writer.WriteLine("<script defer async type=\"text/javascript\" src= \"/lib/boot.js\"></script>");
            writer.WriteLine("</head>");
            writer.WriteLine("<body>");
            writer.WriteLine("<div class=application id=application>");
            
            writer.WriteLine("<div class=application_header id=application_header>");
            writer.WriteLine($"{title}"); 
            writer.WriteLine("<div class=application_button_section> <button class=application_button onclick=\"collapse_fullscreen();\">❖</button>");
            writer.WriteLine("<button class=application_button onclick=\"toggle_to_fullscreen();\">⌬</button> </div>");
            writer.WriteLine("</div>");
            
            writer.WriteLine("<div class=application_body id=application_body>");
            writer.WriteLine("Работает!");
            writer.WriteLine("<button>Test </button>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body>");
            writer.WriteLine("</html>");

            writer.Close();
            stopwatch.Stop();
            return ($"Обработано {request.Url} за {(double)stopwatch.ElapsedTicks / 10000} мс");


        }

    }
}
