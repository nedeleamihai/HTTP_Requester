using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Requester
{
    class Test
    {
        //Pastrati ierarhia clasei!!
        //Scrieti corpul claselor, metodele, etc.
        public class ParsedResponse
        {
            //class for the User
            public class User
            {
                private String nume;
                private int score;
                private String raspuns;
                private String descriere;

                public User(String nume, int score, String raspuns, String descriere)
                {
                    this.nume = nume;
                    this.score = score;
                    this.raspuns = raspuns;
                    this.descriere = descriere;
                }
            }
            
            //class for the Question
            public class Question
            {
                private String intrebare;
                private int voturi;
                private String data;

                public Question(String intrebare, int voturi, String data)
                {
                    this.intrebare = intrebare;
                    this.voturi = voturi;
                    this.data = data;
                }
            }
        }





        /*Nu modificati semmatura functiilor -------------------------------------------------------*/
        /*Modificati doar corpul functiilor in functie de cerinte...................................*/

        public static Response queryServer()
        {
            //TODO (1): sa se contruiasca cele 2 URL-uri analizand documentatia si folosind functii specializate pe stringuri
            //Bonus: sa se defineasca la nivel global  in clasa constante de tip string comune celor 2 URL-uri
            String QuestionsURL = "https:" +
                                    "//" +
                                    "api.randomapi.com" +
                                    "/" +
                                    "16.4" +
                                    "/" +
                                    "questions?private_token=" +
                                    "Ty151J88M&page=1";

            String UsersURL = "https:" +
                                    "//" +
                                    "api.randomapi.com" +
                                    "/" +
                                    "16.4" +
                                    "/" +
                                    "users?private_token=" +
                                    "Ty151J88M&page=1";

            //TODO (2): sa se apeleze functia makeHttpRequest() cu unul dintre URL-uri
            Response raspuns = NetworkUtils.makeHttpRequest(UsersURL);

            //TODO (3): apelul la makeHttpRequest() poate intoarce diferite un raspuns cu diferite coduri
            //pentru fiecare cod, sa se afiseze un mesaj corespunzator in consola: E.g. "Codul primit e ...."
            

            //TODO (4): sa se trateze, specific, exceptia primita in cazul unui URL cu format necorespunzator 
            //Hint: exceptia este declarata la nivel global in clasa
            try
            {
                Console.WriteLine("Codul primit e " + raspuns.getResponseCode());
            }
            catch (NetworkUtils.InvalidUrlFormatException e)
            {
                Console.WriteLine("No response, HTTP code is: " + raspuns.getResponseCode());
            }

            //TODO (5): sa se returneze raspunsul de la server

            return raspuns;
        }




        public static void parseResponse(Response response)
        {

            //TODO (6): analizati tipul de raspuns primit de la server si construiti clasele de tip
            //User si Question, definind metode de acces si de modificare pentru fiecare atribut in parte
            ParsedResponse.User u;
            ParsedResponse.Question q;

            if (response.getResponseDetails().getResponseType() == 0)
            {
                String[] parts = response.getResponseDetails().getQuestionsResponse().Split('\n');

                for (int i = 0; i < Int32.Parse(response.getResponseDetails().getUsersCount()); i++)
                {
                    String[] userparts = parts[i].Split('|');

                }
            }
            else
            {
                String[] parts = response.getResponseDetails().getQuestionsResponse().Split('\n');

                for (int i = 0; i < Int32.Parse(response.getResponseDetails().getUsersCount()); i++)
                {
                    String[] questionparts = parts[i].Split('|');

                }
            }

            //TODO (8): in cazul unui apel de succes, sa se interpreteze/converteasca mesajul obiectului
            //de tip Response si sa se creeze obiectele necesare care sa contina informatiile de la server

            //TODO (9): folosind supraincarcarea, sa se creeze doua functii care iau ori un obiect de tip User
            // sau Question si ii afiseaza continutul intr-un format aranjat

            //TODO (10): realizati comunicarea intre toate functiile create din functiile Main
            
        }
    }
}
