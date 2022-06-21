using Newtonsoft.Json;
using StarWarsCharacters.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace StarWarsCharacters.Helpers
{
    public class APICalls
    {
        private class PeopleRootObject
        {
            public string Count { get; set; }
            public string Next { get; set; }
            public string Previous { get; set; }
            public List<Person> Results { get; set; }
        }

        public static List<Person> GetAllPeople()
        {
            //Api endpoint to get all people (first page only)
            string endPoint = "https://swapi.dev/api/people";

            //create output list to add elements to
            List<Person> people = new List<Person>();

            while(endPoint != null)
            {
                //Request the data from the endpoint
                HttpWebRequest request = WebRequest.Create(endPoint) as HttpWebRequest;

                //Put the response in a json string
                string jsonString = "";
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    jsonString = reader.ReadToEnd();
                }

                //Map the JSON string to the PeopleRootObject
                PeopleRootObject data = JsonConvert.DeserializeObject<PeopleRootObject>(jsonString);

                //Add first page of people to output list
                people.AddRange(data.Results);

                //Set the next end point
                endPoint = data.Next;
            }

            //Set the Id for each person
            for (int i = 0; i < people.Count; i++)
            {
                int id = i + 1;
                people[i].Id = id.ToString();
            }

            return people; //Return the List of people
        }
    }
}
