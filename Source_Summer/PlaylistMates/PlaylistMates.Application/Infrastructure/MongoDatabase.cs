using System.Security.Cryptography.X509Certificates;
using MongoDB.Bson;
using PlaylistMates.Application.Documents;

namespace PlaylistMates.Application.Infrastructure;

using Bogus;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


    public class MongoDatabase
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _db;
        public bool EnableLogging { get; set; }

        public MongoDatabase()
        {
            const string connectionUri = "mongodb+srv://admin:6CiUFfQdjbCt1xHO@cluster0.ltk1drf.mongodb.net/?authSource=admin";
            
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            // Set the ServerApi field of the settings object to Stable API version 1
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            // Create a new client and connect to the server
            _client = new MongoClient(settings);
            _db = _client.GetDatabase("PlaylistMates");
            // Send a ping to confirm a successful connection
            try {
                var result = _client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }
        public PlaylistRepositoryd PlaylistRepository => new PlaylistRepositoryd(_db.GetCollection<Playlistd>(nameof(Playlistd)));
        public Repositoryd<Songd,Guid> SongRepository => new Repositoryd<Songd, Guid>(_db.GetCollection<Songd>(nameof(Songd)));
        
        /*public Repository<Exam, Guid> ExamRepository => new Repository<Exam, Guid>(_db.GetCollection<Exam>(nameof(Exam)));
        public StudentRepository StudentRepository => new StudentRepository(_db.GetCollection<Student>(nameof(Student)));
        public TeacherRepository TeacherRepository => new TeacherRepository(_db.GetCollection<Teacher>(nameof(Teacher)));*/
        
        // move to a test
        /*public void Seed()
        {
            _db.DropCollection(nameof(Student));
            _db.DropCollection(nameof(Exam));
            _db.DropCollection(nameof(Teacher));

            Randomizer.Seed = new Random(1454);
            var rnd = new Randomizer();

            var teachers = new Faker<Teacher>("de")
                .CustomInstantiator(f =>
                {
                    var lastname = f.Name.LastName();
                    return new Teacher(
                        shortname: lastname.Length < 3 ? lastname.ToUpper() : lastname.Substring(0, 3).ToUpper(),
                        firstname: f.Name.FirstName(),
                        lastname: lastname)
                    {
                        Email = $"{lastname.ToLower()}@spengergasse.at".OrDefault(f, 0.25f) // using Bogus.Extensions;
                    };
                })
                .Generate(200)       // Take nimmt nur 20 Elemente des Enumerators
                .GroupBy(g => g.Shortname)
                .Select(g => g.First())
                .Take(20)
                .ToList();
            _db.GetCollection<Teacher>(nameof(Teacher)).InsertMany(teachers);

            int id = 1000;
            var subjects = new string[] { "POS", "DBI", "D", "E", "AM" };
            var classes = new string[] { "4AHIF", "4BHIF", "4CHIF", "4EHIF" };
            var students = new Faker<Student>("de")
                .CustomInstantiator(f =>
                {
                    var s = new Student(
                        id: id++,
                        firstname: f.Name.FirstName(),
                        lastname: f.Name.LastName(),
                        schoolClass: f.Random.ListItem(classes),
                        dateOfBirth: new DateTime(2003, 1, 1).AddDays(f.Random.Int(0, 4 * 365)));

                    var grades = new Faker<Grade>("de")
                        .CustomInstantiator(f =>
                            new Grade(
                                value: f.Random.Int(1, 5),
                                subject: f.Random.ListItem(subjects)))
                        .Generate(5);
                    foreach (var g in grades)
                    {
                        s.UpsertGrade(g);
                    }
                    return s;
                })
                .Generate(100)
                .ToList();
            _db.GetCollection<Student>(nameof(Student)).InsertMany(students);

            var negative = students.SelectMany(s => s.NegativeGrades.Select(n => new { Student = s, Grade = n })).ToList();
            var exams = rnd
                        .ListItems(negative, negative.Count / 2)
                        .Select(n =>
                        {
                            var teacher = rnd.ListItem(teachers);
                            var assistant = rnd.ListItem(teachers);
                            // In 20% der Fälle liefern wir ein GradedExam (schon benotet).
                            var e = new Exam(
                                student: n.Student,
                                teacher: rnd.ListItem(teachers),
                                subject: n.Grade.Subject);
                            return rnd.Bool(0.5f) && teacher.Shortname != assistant.Shortname
                                ? new GradedExam(
                                    exam: e,
                                    assistant: rnd.ListItem(teachers),
                                    grade: new Grade(value: rnd.Int(3, 5), subject: n.Grade.Subject))
                                : e;
                        })
                        .ToList();
            _db.GetCollection<Exam>(nameof(Exam)).InsertMany(exams);
        }*/
    }
