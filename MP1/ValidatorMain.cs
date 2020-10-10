// MP1: XML Validator
// Instructor-provided code. You should not modify this file!
//
// This program tests a XML validator object from a file. We can also
// add or remove individual tags.
//
// When it prompts for a file name, if a simple string such as 
// "test1.xml" (without the quotes) is typed, it will just look
// for the file on the hard disk in the same directory as the 
// executable file.
//
// First use the provided test files and then consider testing with own
// test cases.


using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace XMLValidatorNS
{
    public class ValidatorMain
    {
        public static void Main()
        {
            XMLValidator validator = new XMLValidator(); //empty validator
            string fileText = "";
            string choice = "s"; //there is no s in if's

            Console.WriteLine("Welcome to our simplified XML validator program!");
            Console.WriteLine("Please start by importing an XML file or adding individual tags.");


            while (true)
            {
                if (choice.StartsWith("f"))
                {
                    // prompt for a filename (accepts either a simple filename or with path)
                    Console.Write("File name: ");
                    string filename = Console.ReadLine().Trim();

                    if (filename.Length > 0)
                    {
                        try
                        {
                            fileText = ReadCompleteFile(filename);
                            Queue<XMLTag> tags = new Queue<XMLTag>(XMLTag.Tokenize(fileText));

                            // create the XML validator
                            validator = new XMLValidator(tags);
                        }
                        catch (FileNotFoundException)
                        {
                            Console.WriteLine("file not found: " + filename);
                        }
                        catch (IOException ioe)
                        {
                            Console.WriteLine("I/O error: " + ioe.Message);
                        }
                    }
                    else
                    {
                        fileText = "No text (starting from empty queue)";
                        validator = new XMLValidator();
                    }
                }
                else if (choice.StartsWith("a"))
                {
                    Console.Write("What tag to add (such as <summary> or </para> or <para /> without attributes)? ");
                    string tagText = Console.ReadLine().Trim();

                    //Some minimal check on the validity of the entered tag
                    bool isNotValid = tagText.Length < 3
                                   || tagText.Substring(1, tagText.Length - 2).Trim().Length < 1 // the element is empty
                                   || tagText[0] != '<' // doesn't start with <
                                   || tagText[tagText.Length - 1] != '>'  // doesn't end with >
                                   || tagText.Substring(1, tagText.Length - 2).Contains("<")  // element has <
                                   || tagText.Substring(1, tagText.Length - 2).Contains(">"); // element has >
                    if (isNotValid)
                    {
                        Console.WriteLine("Not an acceptable tag");
                    }
                    else
                    {
                        try
                        {
                            XMLTag tag = XMLTag.Parse(tagText);
                            validator.AddTag(tag);
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                else if (choice.StartsWith("g"))
                {
                    Console.WriteLine("The current tags are: "); Console.WriteLine();
                    Console.WriteLine(validator.GetTags());
                }
                else if (choice.StartsWith("p"))
                {
                    if (string.IsNullOrEmpty(fileText))
                    {
                        Console.WriteLine("Nothing here to print!");
                    }
                    else
                    {
                        Console.WriteLine("The content of the file last read is: "); Console.WriteLine();
                        Console.WriteLine(fileText);
                    }
                }
                else if (choice.StartsWith("r"))
                {
                    Console.WriteLine("Examples:");
                    Console.WriteLine("  if you type <summary>, then <summary> tags will be removed");
                    Console.WriteLine("  if you type summary, then all tags that include summary will be removed (such as <summary> or </summary>)");
                    Console.WriteLine();
                    Console.Write("What to remove all (such as <summary> or </code> or see)? ");
                    String element = Console.ReadLine().Trim();
                    validator.Remove(element);
                }
                else if (choice.StartsWith("v"))
                {
                    validator.Validate();
                    Console.WriteLine();
                }
                else if (choice.StartsWith("q"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Choose from the menu");
                }
                Console.WriteLine();
                Console.Write("(f)ilename, (p)rint, (g)etTags, (v)alidate, (a)ddTag, (r)emove, (q)uit? ");
                choice = Console.ReadLine().Trim().ToLower();
            }
        }

        /// <summary>
        ///  Returns an input stream to read from the given address.
        ///  Works with normal file names.
        /// </summary>
        public static StreamReader GetInputStream(string address)
        {
            // local file
            return new StreamReader(address);

        }

        /// <summary>
        /// Opens the given address for reading input, and reads it until the end 
        /// of the file, and returns the entire file contents as a big String.
        /// </summary>
        public static string ReadCompleteFile(String address)
        {
            StreamReader stream = GetInputStream(address);   // open file

            // read each letter into a buffer
            StringBuilder buffer = new StringBuilder();
            while (true)
            {
                int ch = stream.Read();
                if (ch < 0)
                {
                    break;
                }

                buffer.Append((char)ch);
            }

            return buffer.ToString();
        }
    }

}
