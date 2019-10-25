using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;
using System.Reflection;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            string choice;
            var db = new BloggingContext();
            Console.WriteLine("Would you like to\n1. Display all blogs\n2. Add a blog\n3. Create a post");
            choice = Console.ReadLine();
            try
            {
                switch (choice)
                {
                    case ("2"):
                        
                // Create and save a new Blog
                Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();
               

                var blog = new Blog {Name = name};
                db.AddBlog(blog);
                logger.Info("Blog added - {name}", name);
                break;

                    case ("1"):

                        // Display all Blogs from the database
                        var query = db.Blogs.OrderBy(b => b.Name);

                Console.WriteLine("All blogs in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                }

                        break;
                    case ("3"):
                        string blogChoice;
                        // Display all Blogs from the database
                        var blogQuery = db.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in blogQuery)
                        {
                            Console.WriteLine(item.Name);
                        }
                        
                        Console.WriteLine("Which blog would you like to post to?");
                        blogChoice = Console.ReadLine();
                        db.Blogs.Where(b => b.Name == blogChoice);
                        Console.WriteLine($"Adding post to {blogChoice}");
                        //add a post
                        Console.WriteLine("Title:");
                        var title = Console.ReadLine();
                        Console.WriteLine("Add your post!");
                        var content = Console.ReadLine();
                        var post = new Post { Title = title, Content = content};
                        db.AddPost(post);
                        logger.Info($"Post added {title}");
                        break;
                    default:
                        Console.WriteLine("Please enter a valid choice.");
                        logger.Error("Invalid choice");
                        break;
            }//switch
            }//try
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}
