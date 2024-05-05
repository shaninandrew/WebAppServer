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
            
            StreamWriter writer = new StreamWriter(response.OutputStream);
            writer.WriteLine("<html>");

            writer.WriteLine("<head>");
            writer.WriteLine("<meta charset=\"utf-8\">");
            writer.WriteLine("<title>Тестовое</title>");
            writer.WriteLine("</head>");

            writer.WriteLine("<body>");
            writer.WriteLine("Работает!");

            writer.WriteLine("</body>");

            writer.WriteLine("</html>");

            writer.Close();
            stopwatch.Stop();
            return ($"Обработано {request.Url} за {(double)stopwatch.ElapsedTicks / 10000} мс");


        }

    }
}
