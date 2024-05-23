using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;

namespace WebAppServer
{
    /// <summary>
    /// Базовый класс от которого необходимо наследовать потомка,
    /// чтобы написать приложение.
    /// Создав потомка, переопределите метод ProcessRequest в котором
    /// обработайте входящий запрос и верните данные в ответ.
    /// </summary>
    public abstract class AServerBase:IDisposable    
    {
        public string RouteApp {  get; set; }="/";

        private HttpListener http;
        private int _port;
        private string _server;

        private AsyncCallback _callback;

        private bool stop = false;

        ///
        public void StopServer()
        { 
            stop = true;
        }

        
        /// <summary>
        /// Создает серверное приложение
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        public AServerBase(string server = "localhost", string route="/", int port = 5080) 
        {
            _port = port;
            _server = server;
            RouteApp = route;

            http = new HttpListener();
            http.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            http.Prefixes.Add("http://"+_server+":"+_port.ToString() +""+ RouteApp);

            try
            {

                http.Start();
                Console.WriteLine($"Базовый класс создал сервер... Порт {_port}");

                int i = 0;
                while (!stop)
                {

                    i=(i+1)%10;
                    //поток запрос-ответ
                    if (stop) break;
                    HttpListenerContext context  = http.GetContext();
                    if (context == null) continue;
                    if (stop) break;


                    Task<string> t = new Task<string>(() =>
                    {
                        Uri url = context.Request.Url;

                        //Не соответствует пути
                        if (url.LocalPath != route)
                            return ProcessFile(context.Request, context.Response);


                        return ProcessRequest(context.Request, context.Response);
                    })
                    {
                        
                    };
                    t.ContinueWith(t => 
                    {

                        //if (i==0) GC.Collect();
                        Console.WriteLine($" {i} {(t.IsCompletedSuccessfully==true?"OK":"ERROR")} {t.Result}");  } 
                    );
                    
                    t.Start();
                }
                
            }
            catch (Exception ex) 
            { 
                Debug.WriteLine(ex.ToString());
                Debug.WriteLine(ex.StackTrace.ToString());

            }  
            
            GC.Collect();

        }

        /// <summary>
        /// Метод обработки запроса надо написать самостоятельно.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        abstract public string  ProcessRequest (HttpListenerRequest request, HttpListenerResponse response);

        /// <summary>
        /// Обработка файлов
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public string ProcessFile(HttpListenerRequest request, HttpListenerResponse response)
        {
            
            string file = System.IO.Path.GetFileName(request.Url.LocalPath);
            string dir = System.IO.Path.GetDirectoryName(request.Url.LocalPath).Replace("..", "").Replace("\\.", ""); //ломаем каталоги ..

            string file_name = "." + System.IO.Path.Combine(dir, file);        //текущий каталог

            if (!System.IO.File.Exists(file_name))
            {
                response.StatusCode = 404;
                // response.Close();
                return $"File {file_name} not found.";
            }


            try
            {
                FileStream fs = new FileStream(file_name, FileMode.Open, FileAccess.Read);
                response.StatusCode = 200;
                response.ContentLength64 = fs.Length;
                fs.Position = 0;
                fs.CopyTo(response.OutputStream);
                fs.Close();
                fs.DisposeAsync();
                //response.Close();

                return $"File {file_name} was sended.";
            }
            catch (Exception ex)
            {
                return $"Exception: "+ex.StackTrace+"\r\n"+ex.Message;
            }

        }
        
        /// <summary>
        /// Уничтожает экземпляр класса
        /// </summary>
        void IDisposable.Dispose()
        {
            Console.WriteLine($"Базовый класс очистил сервер.");
            try { http.Stop(); } catch { }  
            
            GC.Collect();

        }
    }
}
