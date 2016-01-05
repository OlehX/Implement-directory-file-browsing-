using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApiPagingAngularClient.Models
{
    public class Dyrectory
    {
        private List<Elem> elements = new List<Elem>();
        private string path="";
        public IEnumerable<Elem> GetAll { get { return this.elements; } }
        public IQueryable<Elem> Elements
        {
            get { return this.GetAll.AsQueryable(); }
        }
        public Dyrectory()
        { 
         
        }
        public Dyrectory(string path)
        {
            this.path = path;
            this.elements = GetFilesFolders();
        }
       
        public List<Elem> GetFilesFolders()
        {
  
            foreach (string f in GetFolders())
            {
                elements.Add(new Elem() { Name = f.Substring(f.LastIndexOf("\\")+1), Type = "Folder" });
            }
            
            foreach (File f in GetFiles())
            {
                elements.Add(new Elem(){ Name = f.Name, Type="File"});
            }
            return elements;
        }
        public List<string> GetFolders()
        {
            List<string> folders = new List<string>();
            try
            {
                // LINQ query.
                var dirs = from dir in
                         Directory.EnumerateDirectories(this.path)
                           select dir;

                // Show results.
                foreach (var dir in dirs)
                {
                    // Remove path information from string.
                        folders.Add(dir.Substring(dir.LastIndexOf("//")+2));

                }

               
            }
            catch (UnauthorizedAccessException UAEx)
            {
                Console.WriteLine(UAEx.Message);
            }
            catch (PathTooLongException PathEx)
            {
                Console.WriteLine(PathEx.Message);
            }
            return folders;
        }
      
        public List<File> GetFiles()
        {
             List<File> files = new List<File>();
            try
            {
               
                DirectoryInfo diTop = new DirectoryInfo(this.path);
                // Enumerate the files just in the top directory.
                foreach (var fi in diTop.EnumerateFiles())
                {

                    try
                    {
                        files.Add(new File { Name = fi.Name, Size = fi.Length });
                    }
                    // Catch unauthorized access to a file.
                    catch (UnauthorizedAccessException UnAuthTop)
                    {
                        Console.WriteLine("{0}", UnAuthTop.Message);
                    }

                }

            }
            // Catch error in directory path.
            catch (DirectoryNotFoundException DirNotFound)
            {
                Console.WriteLine("{0}", DirNotFound.Message);
            }
            // Catch unauthorized access to a first tier directory. 
            catch (UnauthorizedAccessException UnAuthDir)
            {
                Console.WriteLine("UnAuthDir: {0}", UnAuthDir.Message);
            }
            return files;
        }
        public int CountFiles( long from, long to)
        {
            // Create a DirectoryInfo object of the starting directory.
            DirectoryInfo diTop = new DirectoryInfo(this.path);
            int count = 0;
            try
            {
                // Enumerate the files just in the top directory.t[[
                foreach (var fi in diTop.EnumerateFiles())
                {
                    try
                    {
                        if (from == -1)
                        {
                            if (fi.Length <= to)
                            {
                                count++;
                            }
                        }
                        else
                        if (to == -1)
                        {
                            if (fi.Length >= from)
                            {
                                count++;
                            }
                        }
                        else
                        {
                            if (fi.Length > from && fi.Length <= to)
                            {
                                count++;
                            }
                        }
                        // Display each file over 10 MB;

                    }
                    // Catch unauthorized access to a file.
                    catch (UnauthorizedAccessException UnAuthTop)
                    {
                        Console.WriteLine("{0}", UnAuthTop.Message);
                    }
                }
                // Enumerate all subdirectories.
                foreach (var di in diTop.EnumerateDirectories("*"))
                {
                    try
                    {
                        // Enumerate each file in each subdirectory.
                        foreach (var fi in di.EnumerateFiles("*",
                                        SearchOption.AllDirectories))
                        {
                            try
                            {
                                if (from == -1)
                                {
                                    if (fi.Length <= from)
                                    {
                                        count++;
                                    }
                                }
                                else
                      if (to == -1)
                                {
                                    if (fi.Length >= to)
                                    {
                                        count++;
                                    }
                                }
                                else
                                {
                                    if (fi.Length > from && fi.Length <= to)
                                    {
                                        count++;
                                    }
                                }

                            }
                            // Catch unauthorized access to a file.
                            catch (UnauthorizedAccessException UnAuthFile)
                            {
                                Console.WriteLine("UnAuthFile: {0}",
                                                UnAuthFile.Message);
                                Console.WriteLine("UnAuthFile: {0}",
                                                 UnAuthFile.Message);
                            }
                        }
                    }
                    // Catch unauthorized access to a subdirectory.
                    catch (UnauthorizedAccessException UnAuthSubDir)
                    {
                        Console.WriteLine("UnAuthSubDir: {0}",
                                                UnAuthSubDir.Message);
                    }
                }
            }
            // Catch error in directory path.
            catch (DirectoryNotFoundException DirNotFound)
            {
                Console.WriteLine("{0}", DirNotFound.Message);
            }
            // Catch unauthorized access to a first tier directory. 
            catch (UnauthorizedAccessException UnAuthDir)
            {
                Console.WriteLine("UnAuthDir: {0}", UnAuthDir.Message);
            }
            // Catch paths that are too long. 
            catch (PathTooLongException LongPath)
            {
                Console.WriteLine("{0}", LongPath.Message);
            }
            return count;
        }

    }
   

}