using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;


namespace BackEndCallLogNS
{
    public class BackEndCallLog
    {
        // Connection String
        private const string connectionString = "Data Source=TapasweeDixit\\SQLEXPRESS;" +
            "Trusted_Connection=true;" +
            "Initial Catalog=CallerLog;" +
            "Connection timeout=60";

        // SqlConnection object
        static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        //Change database data

        static public void UpdateData(string sqlInstruction)
        {
            using (SqlConnection myConnection = GetConnection())
            {

                if (myConnection == null)
                {
                    Console.WriteLine("Cannot connect to the database {0}", sqlInstruction);
                    return;
                }

                try
                {
                    SqlCommand cmd = new SqlCommand(sqlInstruction, myConnection);

                    myConnection.Open();
                    cmd.ExecuteNonQuery();
                    myConnection.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(sqlInstruction+ ex.ToString);
                }
                finally
                {
                    if (myConnection != null)
                    {
                        myConnection.Close();
                    }
                }
            }
        }

        static public DataSet? ReadData(string sqlQuery)
        {
            using (SqlConnection? myConnection = GetConnection())
            {
                if (myConnection == null)
                {
                    Console.Error.WriteLine("Cannot connect to the database: {0}", sqlQuery);
                    return null;
                }

                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(sqlQuery, myConnection);
                    DataSet ds = new DataSet();

                    myConnection.Open();
                    da.Fill(ds);
                    myConnection.Close();

                    return ds;
                }
                catch (Exception ex)
                {
                    MessageBoxResult mesgBoxResult = System.Windows.MessageBox.Show
                        (sqlQuery + "-" + ex.Message, "Exception",
                            System.Windows.MessageBoxButton.OK);
                    return null;
                }
                /*finally
                {
                    if (myConnection != null)
                    {
                        myConnection.Close();
                        return null;
                    }
                } */
            }
        }


        // Table users methods
        //

        public static bool IsValidUser(string username, string password)
        {
            string sql = "SELECT * FROM [Users] WHERE Username = '" + username + "'  AND Password= '" + password + "'";

            DataSet? ds = ReadData(sql);
            if (ds != null)
            {
                return (ds.Tables[0].Rows.Count > 0 ? true : false);
            }
            else
            {
                return false;
            }
           
        }

        public static bool IsADMIN(string username, string password)
        {


            string sql = "SELECT isAdmin FROM[Users] WHERE Username = '" + username + "'  AND Password= '" + password + "' AND isAdmin = 1";

            DataSet? ds = ReadData(sql);
            if (ds != null)
            {
                return (ds.Tables[0].Rows.Count > 0 ? true : false);
            }
            else
            {
                return false;
            }

        }
        static public DataSet? RetrieveUser(string name)
        {
            string query = "SELECT * FROM [Users] WHERE username = '" + name + "';";
            DataSet? ds = ReadData(query);
            return ds;
        }
        static public void InsertUser(string username, string fullName, string password, bool? isAdmin)
        {
            string sqlInstruction = "INSERT INTO [Users] (username, fullname, password, isAdmin, isActive) VALUES (";
            sqlInstruction += "'" + username + "', ";
            sqlInstruction += "'" + fullName + "', ";
            sqlInstruction += "'" + password + "', ";
            sqlInstruction += "'" + (isAdmin == true ? "true" : "false") + "', 'false')";

            UpdateData(sqlInstruction);
        }

        static public void DeleteUser(string id)
        {
            string sqlInstruction = "DELETE FROM [Users] WHERE id = " + id;
            UpdateData(sqlInstruction);
        }

        static public void UpdateUser(string id, string username, string fullName, string password, bool? isAdmin)
        {
            string sqlInstruction = "UPDATE [Users] SET ";
            sqlInstruction += "username = '" + username + "', ";
            sqlInstruction += "fullname = '" + fullName + "', ";
            sqlInstruction += "password = '" + password + "', ";
            sqlInstruction += "isAdmin = '" + (isAdmin == true ? "true" : "false") + "' ";
            sqlInstruction += " WHERE id = " + id;

            UpdateData(sqlInstruction);
        }

        static public bool CheckLogin(string username, string password)
        {

            return true;
        }

        // Directory Table Methods
        //
        static public DataSet? RetrieveDirectory(string telephone)
        {
            string query = "";
            if (telephone != "")
            {
                query = "SELECT * FROM [Directory] WHERE telephoneNumber = '" + telephone + "';";
            }
            else
            {
                query = "SELECT * FROM [Directory] ORDER BY name";
            }

            DataSet? ds = ReadData(query);
            return ds;
        }

        static public bool validateContactID(string idContact)
        {
            string query = "SELECT * FROM [Directory] WHERE idContact = '" + idContact + "';";

            DataSet? ds = ReadData(query);
            if (ds != null)
            {
                return (ds.Tables[0].Rows.Count > 0 ? true : false);
            }
            else
            {
                return false;
            }
        }

        static public bool validateTelephone(string telephone)
        {
            string query = "SELECT * FROM [Directory] WHERE telephoneNumber = '" + telephone + "';";

            DataSet? ds = ReadData(query);
            if (ds != null)
            {
                return (ds.Tables[0].Rows.Count > 0 ? true : false);
            }
            else
            {
                return false;
            }
        }
        static public bool validateContactCallLog(string idContact)
        {
            string query = "SELECT * FROM [CallLOg] WHERE idContact = '" + idContact + "';";

            DataSet? ds = ReadData(query);
            if (ds != null)
            {
                return (ds.Tables[0].Rows.Count > 0 ? true : false);
            }
            else
            {
                return false;
            }
        }
        static public void InsertDirectory(string name, string telephone)
        {
            string sqlInstruction = "INSERT INTO [Directory] (name, telephoneNumber) VALUES (";
            sqlInstruction += "'" + name + "', ";
            sqlInstruction += "'" + telephone + "')";

            UpdateData(sqlInstruction);
        }

        static public void DeleteDirectory(string id)
        {
            string sqlInstruction = "DELETE FROM [Directory] WHERE idContact = " + id;
            Console.WriteLine(sqlInstruction);
            UpdateData(sqlInstruction);
        }

        static public void UpdateDirectory(string id, string name, string telephone)
        {
            string sqlInstruction = "UPDATE [Directory] SET ";
            sqlInstruction += "name = '" + name + "', ";
            sqlInstruction += "telephoneNumber = '" + telephone + "' ";
            sqlInstruction += " WHERE idContact = " + id;

            UpdateData(sqlInstruction);
        }

        // Caller Logs Table Methods
        //
        static public DataSet? RetrieveCallLog(string callLogID)
        {
            string query = "SELECT  CallLog.idCall, CallLog.idContact, Directory.name, Directory.telephoneNumber, ";
            query += "CallLog.typeOfCall, CallLog.dateTime, CallLog.duration ";
            query += "FROM [CallLog], [Directory] ";
            if (callLogID != "")
            {
                query += "WHERE idCall = " + callLogID + " AND ";
                query += "CallLog.idContact = Directory.idContact";
            }
            else
            {
                query += "WHERE CallLog.idContact = Directory.idContact";
            }

            DataSet? ds = ReadData(query);
            return ds;
        }

        static public DataSet? QueryCallLogByTelephone(string search)
        {

            string query = "SELECT  CallLog.idCall, CallLog.idContact, Directory.name, Directory.telephoneNumber, ";
            query += "CallLog.typeOfCall, CallLog.dateTime, CallLog.duration ";
            query += "FROM [CallLog], [Directory] ";
            query += "WHERE Directory.telephoneNumber LIKE '" + search + "%' AND ";
            query += "CallLog.idContact = Directory.idContact";
        
            DataSet? ds = ReadData(query);
            return ds;
        }

        static public DataSet? QueryCallLogByName(string search)
        {

            string query = "SELECT  CallLog.idCall, CallLog.idContact, Directory.name, Directory.telephoneNumber, ";
            query += "CallLog.typeOfCall, CallLog.dateTime, CallLog.duration ";
            query += "FROM [CallLog], [Directory] ";
            query += "WHERE Directory.name LIKE '" + search + "%' AND ";
            query += "CallLog.idContact = Directory.idContact";
            DataSet? ds = ReadData(query);
            return ds;
        }

        static public void InsertCallLog(string contact, string typeOfCall, string dateTime, string duration)
        {
            string sqlInstruction = "INSERT INTO [CallLog] (idContact,  typeOfCall, dateTime, duration) VALUES (";
            sqlInstruction += "'" + contact + "', ";
            sqlInstruction += "'" + typeOfCall + "', ";
            sqlInstruction += "'" + dateTime + "', ";
            sqlInstruction += "'" + duration + "')";
            UpdateData(sqlInstruction);
        }

        static public void DeleteCallLog(string id)
        {
            string sqlInstruction = "DELETE FROM [CallLog] WHERE idCall = " + id;
            UpdateData(sqlInstruction);
        }

        static public void UpdateCallLog(string id, string contactID, string typeOfCall, string dateTime, string duration)
        {
            string sqlInstruction = "UPDATE [CallLog] SET ";
            sqlInstruction += "idContact = '" + contactID + "', ";
            sqlInstruction += "typeOfCall = '" + typeOfCall + "', ";
            sqlInstruction += "dateTime = '" + dateTime + "', ";
            sqlInstruction += "duration = '" + duration + "' ";
            sqlInstruction += " WHERE idCall = '" + id + "'";
            UpdateData(sqlInstruction);
        }
    }
}
