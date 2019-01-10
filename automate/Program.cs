using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using automate.Models;
using System.IO;
using Newtonsoft.Json;

namespace AutomateStoredProcedures
{
    public class Automate
    {
        public static void Main(string[] args)
        {
            List<DbConnectionWithCommands> DbConnectionWithCommandsList = CommandsToRun();

            for (int i = 0; i < DbConnectionWithCommandsList.Count; i++)
            {
                DbConnectionWithCommands dbCommands = DbConnectionWithCommandsList.ElementAt(i);

                for (int j = 0; j < dbCommands.Commands.Length; j++)
                {
                    try
                    {
                        using (var sqlConnection1 = new SqlConnection(dbCommands.DbConnection))
                        {
                            using (var cmd = new SqlCommand()
                            {
                                CommandText = dbCommands.Commands[j],
                                CommandType = CommandType.StoredProcedure,
                                Connection = sqlConnection1
                            })
                            {
                                sqlConnection1.Open();
                                cmd.ExecuteNonQuery();
                                Console.WriteLine("Command " + dbCommands.Commands[j] + " ran Successfully");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        addErrorLogs(e.ToString(), dbCommands.Commands[j]);
                        Console.WriteLine(e.ToString());
                    }
                }
            }
            Console.WriteLine("\n");
            Console.WriteLine("====>> exiting in 2 seconds <<====");
            System.Threading.Thread.Sleep(2000);
        }

        private static List<DbConnectionWithCommands> CommandsToRun()
        {
            try
            {
                using (StreamReader r = new StreamReader("config.json"))
                {
                    try
                    {
                        string json = r.ReadToEnd();
                        List<DbConnectionWithCommands> items = JsonConvert.DeserializeObject<List<DbConnectionWithCommands>>(json);
                        return items;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        addErrorLogs(e.ToString(), "");
                        return new List<DbConnectionWithCommands>();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error with config.json file");
                addErrorLogs(e.ToString(), "");
                return new List<DbConnectionWithCommands>();
            }
        }


        private static void addErrorLogs(string msg, string procedure)
        {
            DateTime date = DateTime.Now;
            string file = @"Logs\" + date.Day + "_" + date.Month + "_" + date.Year + ".txt";
            using (StreamWriter w = File.AppendText(file))
            {
                if (procedure.Length > 0)
                {
                    w.WriteLine("Date : " + DateTime.Now + Environment.NewLine + "Procedure : " + procedure + Environment.NewLine + "Error : " + msg + Environment.NewLine);
                }
                else
                {
                    w.WriteLine("Date : " + DateTime.Now + Environment.NewLine + "Error : " + msg + Environment.NewLine);
                }

            }
        }
    }
}
