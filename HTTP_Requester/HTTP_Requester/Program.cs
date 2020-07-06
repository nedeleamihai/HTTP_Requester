using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace HTTP_Requester
{
    class Program
    {   
        static void Main(string[] args)
        {
            //comment this and replace it with SetDefaultConfiguration for development purposes
            //use the public enums ROUTER_STATUS_CODE and SERVER_STATUS_CODE for giving params 
            NetworkUtils.AutomaticServerRouterStatus();



            //TODO(11)... de aici incolo
            Test.parseResponse(Test.queryServer());
           

            Console.WriteLine("S-a terminat programul :)");
            Console.WriteLine("Nedelea Teodor Mihai");
            Console.ReadKey();
        }
    }
}
