using System;
using System.Collections.Generic;
using System.Data.OleDb;



namespace Examproject
{
    class Database
    {
        //private List<Patient> patients;
        //public List<Patient> Patients { get => patients; set => patients = value; }
        private List<Visit> visits;
        public List<Visit> Visits { get => visits; set => visits = value; }

        public Database()
        {
            Visits = new List<Visit>();
        }
        public Database(string DBCmd, string filePath)
        {
            Visits = new List<Visit>();
            getFromDB(DBCmd, filePath);
        }
        public void getFromDB(string DBCmd, string filePath)
        {
            string con =
                   @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ".xls;" +
                    @"Extended Properties='Excel 8.0;HDR=Yes;'";
            using (OleDbConnection connection = new OleDbConnection(con))
            {
                Visit temp;
                connection.Open();
                OleDbCommand command = new OleDbCommand(DBCmd, connection);
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        temp = new Visit();
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            temp.Attributes.Add(dr[i]);
                        }
                        Visits.Add(temp);
                    }
                }
            }
        }
    }
}
