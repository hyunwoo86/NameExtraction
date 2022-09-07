using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NameExtraction
{
    class NameExtraction
    {
        string file_panth = "";
        public NameExtraction()
        {

        }

        public void GetPath(string path)
        {
            file_panth = path;
            Console.WriteLine("path: " + file_panth);
            FileLoad();
        }

        private void FileLoad()
        {
            if (file_panth.Length > 0)
            {
                FileInfo fileInfo = new FileInfo(file_panth);

                if (fileInfo.Exists)
                {
                    Console.WriteLine("exists file");
                    ReadingFile();
                }
                else
                {
                    Console.WriteLine("None");
                }
            }
        }

        private void ReadingFile()
        {
            Console.WriteLine("reading file");

            string[] codeLine = File.ReadAllLines(file_panth);

            Console.WriteLine("");
            Console.WriteLine("");

            foreach (var code in codeLine)
            {
                Console.WriteLine(code.Trim());
                GetClassName(code.Trim());
            }

            Console.WriteLine("");
            Console.WriteLine("");
        }

        string ClassName = "";
        private void GetClassName(string code)
        {
            List<string> list_class = code.Split(' ').ToList();

            if (list_class[0].Equals("class"))
            {
                ClassName = list_class[1].Trim();
                Console.WriteLine("Class Name: " + ClassName);
            }

            GetFunctionName(code);

        }

        string FunctionName = "";
        string ReturnValue = "";
        List<string> ListReturnValue = new List<string>() { { "void" }, { "string" }, { "double" }, { "int" }, { "string" } };
        string ParameterValue = "";

        private void GetFunctionName(string code)
        {
            FunctionName = "";
            ReturnValue = "";
            ParameterValue = "";

            // public 만 추출
            List<string> list_function = code.Split(' ').ToList();

            if (list_function[0] == "public")
            {
                int start_Index = code.IndexOf("(");
                int last_Index = code.IndexOf(")");

                if (start_Index != -1 && last_Index != -1)
                {
                    if (list_function[1].Trim().Contains(ClassName))
                    {
                        // 생성자
                        FunctionName = list_function[1].Split('(')[0];
                    }
                    else
                    {
                        ParameterValue = code.Substring(start_Index + 1, last_Index - start_Index - 1);

                        int cnt = 0;
                        bool checkReturn = false;
                        foreach (var name in list_function)
                        {
                            if (cnt > 0)
                            {
                                if (name.Contains("("))
                                {
                                    checkReturn = true;
                                    FunctionName = name.Split('(')[0];
                                }
                                else if (checkReturn == false) ReturnValue += name + " ";
                            }
                            cnt++;
                        }
                    }

                    WriteCSV();
                }
            }
        }

        private void WriteCSV()
        {
            List<string> name = file_panth.Split('\\').ToList();
            string fileName = name[name.Count - 1];
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(
                $"{fileName}.txt",
                true,
                System.Text.Encoding.GetEncoding("utf-8")))
            {
                file.WriteLine("{0}@{1}@{2}@{3}", ClassName, FunctionName, ReturnValue, ParameterValue);
            }
        }
    }
}
