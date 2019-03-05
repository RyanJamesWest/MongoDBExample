using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//These need to be included
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoDbExample
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }

        static async Task MainAsync()
        {
            //Connects to a MongoDb instance.
            var client = new MongoClient();

            //Retrieves a database named "school", if it doesn't exist, it creates it.
            IMongoDatabase db = client.GetDatabase("school");

            //Retrieves a collection in the database named "students", if the collection doesn't exist, it creates it.
            var collection = db.GetCollection<Student>("students");
            
            //Calls a function to create a list of new students
            var newStudents = CreateNewStudents(); 

            //Inserts the list of new students to the collection
            await collection.InsertManyAsync(newStudents);
            Console.WriteLine("Student records created");
            Console.ReadKey();

            //Displays the record defined by the filter
            var filter = Builders<Student>.Filter.Eq("_id", 2);
            var result = await collection.Find(filter).ToListAsync();
            Console.WriteLine(result.ToJson());
            Console.ReadKey();

            //Updates the record defined by the filter
            filter = Builders<Student>.Filter.Eq("_id", 2);
            var update = Builders<Student>.Update.Set("Age", 24);
            await collection.UpdateOneAsync(filter, update);
            Console.WriteLine("Student record updated");
            Console.ReadKey();

            //deletes the record defined by the filter
            filter = Builders<Student>.Filter.Eq("_id", 3);
            await collection.DeleteOneAsync(filter);
            Console.WriteLine("Student record deleted");
            Console.ReadKey();
        }

        private static IEnumerable<Student> CreateNewStudents()
        {
            var student1 = new Student
            {
                Id = 1,
                FirstName = "Gregor",
                LastName = "Felix",
                Subjects = new List<string>() { "English", "Mathematics", "Physics", "Biology" },
                Class = "JSS 3",
                Age = 23
            };

            var student2 = new Student
            {
                Id = 2,
                FirstName = "Machiko",
                LastName = "Elkberg",
                Subjects = new List<string> { "English", "Mathematics", "Spanish" },
                Class = "JSS 3",
                Age = 23
            };

            var student3 = new Student
            {
                Id = 3,
                FirstName = "Julie",
                LastName = "Sandal",
                Subjects = new List<string> { "English", "Mathematics", "Physics", "Chemistry" },
                Class = "JSS 1",
                Age = 25
            };

            var newStudents = new List<Student> { student1, student2, student3 };

            return newStudents;
        }
    }

    //This could be in a separate class cs file
    internal class Student
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Class { get; set; }
            public int Age { get; set; }
            public IEnumerable<string> Subjects { get; set; }
        }
}
