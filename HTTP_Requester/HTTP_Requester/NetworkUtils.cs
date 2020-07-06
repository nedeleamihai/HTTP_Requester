using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace HTTP_Requester
{   
    //A NU SE MODIFICA VREO FUNCTIE DIN ACEST FISIER!!!!!!!

    //class for the Response object, makeHttpRequest() always returns a Response object
    //regardless of the success or unsuccess of the HTTP query
    public class Response
    {
        //the response code of the HTTP request is stored here (e.g. 200,404, etc)
        private int responseCode;
        //the reference to a ResponseDetails object - non-null if the response code is 200-OK
        private ResponseDetails responseDetails;

        public int getResponseCode() { return this.responseCode; }
        public ResponseDetails getResponseDetails() { return this.responseDetails; }

        //Constructor...
        public Response(int responseCode,
                        ResponseDetails responseDetails)
        {
            this.responseDetails = responseDetails;
            this.responseCode = responseCode;
        }

        //in case of a succesful call (200), the ResponseDetails object is not null
        //HINT: see the implementation of the makeHttpRequest() function
        public class ResponseDetails
        {
            //responseType is either USERS or QUESTIONS, depending on the URL used for querying the server
            //HINT: see the RESPONSE_TYPE public enum
            private int responseType;
            //in case the response type is USERS, this variable is non-null
            private string users_count;
            //in case the response type is QUESTIONS, this variable is non-null 
            private string questions_count;
            //in case the response type is USERS, this variable is non-null
            private string usersResponse;
            //in case the response types is QUESTIONS, this variable is non-null
            private string questionsResponse;

            public int getResponseType() { return this.responseType; }
            public string getQuestionsCount() { return this.questions_count; }
            public string getUsersCount() { return this.users_count; }
            public string getUsersResponse() { return this.usersResponse; }
            public string getQuestionsResponse() { return this.questionsResponse; }

            //Constructor...
            public ResponseDetails(
                        int responseType,
                        string users_count,
                        string questions_count,
                        string usersResponse,
                        string questionsResponse)
            {
                this.responseType = responseType;
                this.users_count = users_count;
                this.questions_count = questions_count;
                this.usersResponse = usersResponse;
                this.questionsResponse = questionsResponse;
            }
        }
    }







    //enum for setting up the server for different scenarios
    //best case-scenario - the status code: UP
    public enum SERVER_STATUS_CODE { UP,
                                     SERVICE_UNAVAILABLE,
                                     REQUEST_TIMEOUT };
    
    //enum for setting up the router for different scenarios
    //best case-scenario - the status code: TURNED ON
    public enum ROUTER_STATUS_CODE { TURNED_ON,
                                     TURNED_OFF };

    //enum for setting the response type in a ResponseType object
    public enum RESPONSE_TYPE { USERS,
                                QUESTIONS };

    //200 - OK  (TURNED ON- UP)
    //504 - ROUTER- TURNED OFF
    //503 - SERVICE UNAVAILABLE
    //408 - REQUEST_TIMEOUT
    //406 - PROTOCOL_MISUSED





    class NetworkUtils
    {
        //URL for getting the infos
        /*NU FOLOSITI URL-urile de aici, creati-le voi in clasa Test------------------------------------------*/
        public static string GET_ALL_QUESTIONS = @"https://api.randomapi.com/16.4/questions?private_token=Ty151J88M&page=1";
        public static string GET_ALL_USERS= @"https://api.randomapi.com/16.4/users?private_token=Ty151J88M&page=1";
           


        public static int SERVER_STATUS;
        public static int ROUTER_STATUS;

        //Custom Exception...
        [Serializable]
        public class InvalidUrlFormatException : Exception
        {
            public InvalidUrlFormatException()
            {

            }

            public InvalidUrlFormatException(string name)
                : base()
            {

            }

        }

        //function that automatically sets the server and router status
        //Hint: for development purposes, comment the call and call instead SetDefaultConfiguration()
        //use the aforementioned enums for custom setting
        public static void AutomaticServerRouterStatus()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            int random = (unixTimestamp * 147) % 100 + 1; //get random number between 1 and 100

            if (random >= 30 && random <= 100)
            {
                NetworkUtils.ROUTER_STATUS = (int)ROUTER_STATUS_CODE.TURNED_ON;
            }
            else
            {
                NetworkUtils.ROUTER_STATUS = (int)ROUTER_STATUS_CODE.TURNED_OFF;
            }

            if (random >= 25 && random <= 75)
            {
                NetworkUtils.SERVER_STATUS = (int)SERVER_STATUS_CODE.UP;
            }
            else if (random >= 0 && random <= 25)
            {
                NetworkUtils.SERVER_STATUS = (int)SERVER_STATUS_CODE.SERVICE_UNAVAILABLE;
            }
            else
            {
                NetworkUtils.SERVER_STATUS = (int)SERVER_STATUS_CODE.REQUEST_TIMEOUT;
            }
            Console.WriteLine("ROUTER STATUS: " + Enum.GetName(typeof(ROUTER_STATUS_CODE), ROUTER_STATUS));
            Console.WriteLine("SERVER STATUS: " + Enum.GetName(typeof(SERVER_STATUS_CODE), SERVER_STATUS));
            Console.WriteLine("\n\n\n");
        }

        //use this function for setting a custom configuration for Router-Server
        //
        public static void SetDefaultConfiguration(ROUTER_STATUS_CODE router,
                                                   SERVER_STATUS_CODE server)
        {
            Console.WriteLine("Default configuration:\n[ROUTER]: " + router + "\n[SERVER]: " + server);
            Console.WriteLine("\n\n\n");
            NetworkUtils.ROUTER_STATUS = (int)router;
            NetworkUtils.SERVER_STATUS = (int)server;
        }

        public static Response makeHttpRequest(string url)
        {
            if(NetworkUtils.ROUTER_STATUS == (int)ROUTER_STATUS_CODE.TURNED_OFF)
            {
                return new Response(504, null);
            }
            if(NetworkUtils.SERVER_STATUS == (int)SERVER_STATUS_CODE.REQUEST_TIMEOUT)
            {
                return new Response(408, null);
            }
            if(NetworkUtils.SERVER_STATUS == (int)SERVER_STATUS_CODE.SERVICE_UNAVAILABLE)
            {
                return new Response(503, null);
            }
            if(NetworkUtils.SERVER_STATUS == (int)SERVER_STATUS_CODE.UP
                && NetworkUtils.ROUTER_STATUS == (int)ROUTER_STATUS_CODE.TURNED_ON)
            {
                return EvaluateRequest(url);
            }
            else
            {
                return new Response(406, null);
            }
        }
        private static string[] readUsersFile(ref int usersCount)
        {
            try
            {
                string path = @"D:\Curs-predare\Partial\Model practic\HTTP_Requester\users.txt";
                StreamReader sr = new StreamReader(path);
                string line = String.Empty;
                int how_many = Int32.Parse(sr.ReadLine());
                string[] users = new string[how_many];
                //Console.WriteLine("There are: " + how_many);
                usersCount = how_many;
                for(int i=0; i<how_many; i++)
                {
                    line = sr.ReadLine();
                    users[i] = line;
                }
                return users;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Could not read from users.txt file?");
                Console.WriteLine(ex);
            }
            return null;  
        }
        private static string[] readQuestionsFile(ref int questionsCount)
        {
            try
            {
                string path = @"D:\Curs-predare\Partial\Model practic\HTTP_Requester\questions.txt";
                StreamReader sr = new StreamReader(path);
                string line = String.Empty;
                int how_many = Int32.Parse(sr.ReadLine());
                string[] questions = new string[how_many];
                //Console.WriteLine("There are: " + how_many);
                questionsCount = how_many;
                for (int i = 0; i < how_many; i++)
                {
                    line = sr.ReadLine();
                    questions[i] = line;
                }
                return questions;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not read from questions.txt file ?");
                Console.WriteLine(ex);
            }
            return null;
        }

        private static string[] SplitUrl(string url)
        {
            string[] parts = url.Split('?','&','/');
            /*foreach(string part in parts)
            {
                Console.WriteLine(part);
            }*/
            return parts;
        }

        private static Response EvaluateRequest(string url) {

            string[] parts = SplitUrl(url);
            InvalidUrlFormatException ex = new InvalidUrlFormatException("You GAVE ME A BAD URL :>(");
        
            if(parts.Length != 7)
            {
                throw ex;
            }
            //Console.WriteLine("The URL had enough bodies");
            
            if(!(parts[0]=="https:" 
                && parts[1]==""
                && parts[2] == "api.randomapi.com"
                && parts[3] == "16.4"
                && parts[5] == "private_token=Ty151J88M"))
            {
                throw ex;
            }
            if (!(parts[4] == "users" || parts[4] == "questions"))
                throw ex;
            if(!(parts[6].Contains("page=") || parts[6].Contains("first=") || parts[6].Contains("user_index=")))
            {
                throw ex;
            }

            if (String.Equals(parts[4], "users"))
            {
                return getUsersResponse();

            }else if (String.Equals(parts[4], "questions"))
            { 
                return getQuestionsResponse();  
            }
            else
            {
                throw ex;
            }
        }

        private static Response getUsersResponse()
        {
            int usersCount = 0;
            string[] users = readUsersFile(ref usersCount);

            string users_response = "";
            for (int i = 0; i < users.Length; i++)
            {
                users_response += users[i];
            }
            /*Console.WriteLine("[Response Users Count]: " + usersCount);
            Console.WriteLine("[Response]: " + users_response);*/
            Response.ResponseDetails responseDetails =
                new Response.ResponseDetails((int)RESPONSE_TYPE.USERS,
                                             usersCount.ToString(),
                                             0.ToString(),
                                             users_response,
                                             String.Empty);
            return new Response(200, responseDetails);
            
    }
        private static Response getQuestionsResponse()
        {
            int questionsCount = 0;
            string[] questions = readQuestionsFile(ref questionsCount);

            string questions_response = "";
            for (int i = 0; i < questions.Length; i++)
            {
                questions_response += questions[i];
            }
            /*Console.WriteLine("[Response Questions Count]: " + questionsCount);
            Console.WriteLine("[Response]: " + questions_response);*/
            Response.ResponseDetails responseDetails =
                new Response.ResponseDetails((int)RESPONSE_TYPE.QUESTIONS,
                                               0.ToString(),
                                               questionsCount.ToString(),
                                               String.Empty,
                                               questions_response);
                
            return new Response(200, responseDetails);
        }
    }
}
