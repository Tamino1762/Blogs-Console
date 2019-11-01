using NLog;
using BlogsConsole.Models;
using System;
using System.ComponentModel;
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
            int count = 0;
            var db = new BloggingContext();
            do
            {
                Console.WriteLine(
                    "Would you like to\n1. Display all blogs\n2. Add a blog\n3. Create a post\n4. Display Posts\nEnter Q to quit");
                choice = Console.ReadLine().ToUpper();
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
                            var query = db.Blogs.OrderBy(b => b.BlogId);

                            Console.WriteLine("All blogs in the database");
                            foreach (var item in query)
                            {
                                count++;
                            }

                            Console.WriteLine(count + " blogs found.");

                            foreach (var item in query)
                            {

                                Console.WriteLine(item.Name);
                            }
                            Console.WriteLine("Press Enter to continue.");
                            Console.ReadLine();

                            break;
                        case ("3"):
                            int blogChoice;
                            // Display all Blogs from the database
                            var blogQuery = db.Blogs.OrderBy(b => b.BlogId);

                            Console.WriteLine("All blogs in the database:");
                            Console.WriteLine();
                            foreach (var item in blogQuery)
                            {
                                count++;
                            }

                            Console.WriteLine(count + " blogs found.");

                            foreach (var item in blogQuery)
                            {
                                Console.WriteLine("Blog ID: {0}\tBlog Name: {1}", item.BlogId, item.Name);
                            }
                           
                            Console.WriteLine("Which blog would you like to post to?");
                            if (int.TryParse(Console.ReadLine(), out blogChoice))
                            {
                                if (db.Blogs.Any(b => b.BlogId == blogChoice))
                                {
                                    Console.WriteLine($"Adding post to {blogChoice}");
                                    //add a post
                                    Console.WriteLine("Title:");
                                    var title = Console.ReadLine();
                                    Console.WriteLine("Add your post!");
                                    var content = Console.ReadLine();

                                    if ((title.Length != 0) && (content.Length != 0))
                                    {
                                        var post = new Post {BlogId = blogChoice, Title = title, Content = content};
                                        db.AddPost(post);
                                        logger.Info($"Post added {title}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Post must include a Title and Content");
                                        logger.Error("Content invalid");
                                    }

                                }
                            }
                            else
                            {
                                Console.WriteLine("Blog must be a number");
                                logger.Error("Invalid input.");
                            }



                            break;
                        case ("4"):
                            // Display all posts from the database
                            int bChoice;
                            var postQuery = db.Posts.OrderBy(p => p.Title).ToList();
                            var blogPosts = db.Blogs.OrderBy(b => b.Name);
                            Console.WriteLine("Select blog posts to display:");
                            Console.WriteLine("0\tAll blogs");
                            foreach (var item in blogPosts)
                            {
                                Console.WriteLine("{0}\t{1}", item.BlogId, item.Name);
                            }
                            if (int.TryParse(Console.ReadLine(), out bChoice)) { 
                                if (bChoice == 0) { 
                                    foreach (var item in blogPosts)
                                    {
                                        count++;
                                    }

                                    Console.WriteLine(count + " blogs found.");

                                    Console.WriteLine("All posts in the database:");
                                    foreach (var blogItem in blogPosts)
                                    {
                                        Console.WriteLine("Blog: " + blogItem.Name);

                                        foreach (var postItem in postQuery.Where(p => p.BlogId == blogItem.BlogId))
                                        {
                                            Console.WriteLine("Title: " + postItem.Title);
                                            Console.WriteLine("Content: " + postItem.Content);
                                            Console.WriteLine("");
                                        }
                                    }
                                    Console.WriteLine("");
                                    Console.WriteLine("Press Enter to continue.");
                                    Console.WriteLine();
                                }
                                if (db.Blogs.Any(b => b.BlogId == bChoice)){ 
                                    Console.WriteLine("All posts in ");
                                    foreach (var blogItem in blogPosts)
                                    {
                                        Console.WriteLine("Blog: " + blogItem.Name);

                                        foreach (var postItem in postQuery.Where(p => p.BlogId == blogItem.BlogId))
                                        {
                                            Console.WriteLine("Title: " + postItem.Title);
                                            Console.WriteLine("Content: " + postItem.Content);
                                            Console.WriteLine("");
                                        }
                                    }
                                    Console.WriteLine("");
                                    Console.WriteLine("Press Enter to continue.");
                                    Console.WriteLine();
                                }
                            }

                            else
                            {
                                Console.WriteLine("Blog must be a number");
                                logger.Error("Invalid input.");
                            }
                            
                            
             
                            Console.ReadLine();
                            break;
                        case ("Q"):
                            Console.WriteLine("Have a great day!");
                            break;
                            default:
                            Console.WriteLine("Please enter a valid choice.");
                            logger.Error("Invalid choice");
                            break;
                            
                    } //switch
                } //try
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }
            } while (choice != "Q");
            logger.Info("Program ended");
        }
    }
}
