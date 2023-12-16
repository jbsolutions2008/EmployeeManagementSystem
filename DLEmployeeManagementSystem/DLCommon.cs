using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeManagementSystem.DataAccess
{
    public class DLCommon
    {

        /* Set the Keys for Encryption */
        private static string _sCryptoPass = "EmployeeManagementSystem";
        private byte[] _byKey = { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static byte[] _byVector = { 65, 110, 68, 26, 69, 178, 200, 219 };

        public IConfiguration Configuration { get; set; }

        public DLCommon()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("AppSettings.json");
            Configuration = builder.Build();
        }

        //Get the connection string from appsetting.json file
        protected string connectionString
        {
            get
            {
                return Configuration.GetConnectionString("DefaultConnection");
            }
        }

        //Connection object for SQL Server
        protected SqlConnection connectionObject
        {
            get
            {
                SqlConnection SQLConn = new SqlConnection();
                SQLConn.ConnectionString = connectionString;
                return SQLConn;
            }
        }

        //Enum for Command Type
        public enum CommandTypes
        {
            TableDirect = 0,
            Text = 1,
            StoredProcedure = 4
        }

        //Enum for Return Type
        public enum ReturnTypes
        {
            None = 0,           //DataReader
            ReturnValue = 1     //Scalar
        }

        //Enum for ReaderTypes
        public enum ReaderTypes : int
        {
            DataReader = 0, //Datareader
            DataScalar = 1//datareader scalar
        }

        /* Set the SQL command type */
        public SqlCommand GetSQLCommand(int dbCommandType, string dbCommand)
        {
            SqlCommand SQLComm = new SqlCommand();
            try
            {
                SQLComm.CommandText = dbCommand;
                switch (dbCommandType)
                {
                    case (int)CommandType.TableDirect:
                        SQLComm.CommandType = CommandType.TableDirect;
                        break;
                    case (int)CommandType.Text:
                        SQLComm.CommandType = CommandType.Text;
                        break;
                    case (int)CommandType.StoredProcedure:
                        SQLComm.CommandType = CommandType.StoredProcedure;
                        break;
                }
            }
            catch (Exception ex)
            {
                SQLComm = null;
                throw ex;
            }

            return SQLComm;
        }

        public SqlCommand GetSQLCommand(string dbCommand)
        {
            SqlCommand SQLComm = new SqlCommand();
            try
            {
                SQLComm.CommandText = dbCommand;
                SQLComm.CommandType = CommandType.StoredProcedure;
            }
            catch (Exception ex)
            {
                SQLComm = null;
                throw ex;
            }

            return SQLComm;
        }

        /* Execute the Data Reader */
        protected SqlDataReader GetDataReader(int dbCommandType, string dbCommand, int ReaderType)
        {
            SqlDataReader SQLdtr = null;
            try
            {
                SqlConnection SQLConn = new SqlConnection();
                SqlCommand SQLcomm = new SqlCommand();
                SQLConn = connectionObject;
                SQLcomm = GetSQLCommand(dbCommandType, dbCommand);
                SQLcomm.Connection = SQLConn;
                SQLConn.Open();

                switch (ReaderType)
                {
                    case (int)ReaderTypes.DataReader:
                        SQLdtr = SQLcomm.ExecuteReader(CommandBehavior.CloseConnection);
                        break;
                    case (int)ReaderTypes.DataScalar:
                        SQLdtr = (SqlDataReader)SQLcomm.ExecuteScalar();
                        break;
                }
                return SQLdtr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if ((SQLdtr != null))
                    SQLdtr = null;
            }
        }

        //Function for get data from DataBase
        public DataSet GetDataSet(string dsTable, string dsSQL)
        {
            DataSet ds = new DataSet();
            DataSet dsReturn;
            SqlConnection SQLConn = new SqlConnection();
            SqlDataAdapter SQLdAdpt;
            int iPos;

            try
            {
                SQLConn = connectionObject;
                SQLdAdpt = new SqlDataAdapter(dsSQL, SQLConn);
                SQLdAdpt.Fill(ds);

                string[] arrTable = dsTable.Split(',');
                for (iPos = 0; iPos <= arrTable.Length - 1; iPos++)
                {
                    ds.Tables[iPos].TableName = arrTable[iPos];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                SQLdAdpt = null;
                if ((SQLConn != null))
                    SQLConn.Close();
                dsReturn = ds;
                ds.Dispose();
                ds = null;
            }
            return dsReturn;
        }

        public DataSet GetDataSetSave(string dsTable, SqlCommand SQLComm)
        {
            DataSet ds = new DataSet();
            DataSet dsReturn;
            SqlConnection SQLConn = new SqlConnection();
            SqlDataAdapter SQLdAdpt;
            int iPos;

            try
            {
                SQLConn = connectionObject;
                SQLComm.Connection = SQLConn;
                SQLdAdpt = new SqlDataAdapter(SQLComm);
                SQLdAdpt.Fill(ds);

                string[] arrTable = dsTable.Split(',');
                for (iPos = 0; iPos <= arrTable.Length - 1; iPos++)
                {
                    ds.Tables[iPos].TableName = arrTable[iPos];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                SQLdAdpt = null;
                if ((SQLConn != null))
                    SQLConn.Close();
                dsReturn = ds;
                ds.Dispose();
                ds = null;
            }
            return dsReturn;
        }

        public DataSet GetDataSetSave(SqlCommand SQLComm)
        {
            DataSet ds = new DataSet();
            DataSet dsReturn;
            SqlConnection SQLConn = new SqlConnection();
            SqlDataAdapter SQLdAdpt;
            int iPos;

            try
            {
                SQLConn = connectionObject;
                SQLComm.Connection = SQLConn;
                SQLdAdpt = new SqlDataAdapter(SQLComm);
                SQLdAdpt.Fill(ds);

                for (iPos = 0; iPos < ds.Tables.Count; iPos++)
                {
                    if (ds.Tables[iPos].Rows.Count > 0)
                    {
                        ds.Tables[iPos].TableName = ds.Tables[iPos].Rows[0]["TableName"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                SQLdAdpt = null;
                if ((SQLConn != null))
                    SQLConn.Close();
                dsReturn = ds;
                ds.Dispose();
                ds = null;
            }
            return dsReturn;
        }

        //Function for execute the query
        public int DataNonQuery(int dbCommandType, string dbCommand, int dbReturnType, string dbConnection)
        {
            SqlConnection SQLConn = new SqlConnection();
            SqlCommand SQLComm = new SqlCommand();
            int iReturn;

            try
            {
                SQLConn = connectionObject;
                SQLComm = GetSQLCommand(dbCommandType, dbCommand);

                if (dbReturnType == (int)(ReturnTypes.ReturnValue))
                {
                    //Add return parameter
                    SqlParameter paramReturnValue = new SqlParameter();
                    paramReturnValue.ParameterName = "@ReturnValue";
                    paramReturnValue.SqlDbType = SqlDbType.Int;
                    paramReturnValue.Direction = ParameterDirection.ReturnValue;
                    //Set timeout to 0
                    SQLComm.CommandTimeout = 0;
                    SQLComm.Parameters.Add(paramReturnValue);

                    SQLConn.Open();
                    SQLComm.ExecuteNonQuery();

                    iReturn = Convert.ToInt32(paramReturnValue.Value);
                    return iReturn;
                }
                else
                {
                    SQLConn.Open();
                    iReturn = SQLComm.ExecuteNonQuery();
                    return iReturn;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SQLComm != null)
                    SQLComm.Dispose();
                SQLComm = null;
                if (SQLConn != null)
                    SQLConn.Close();
                SQLConn.Dispose();
                SQLConn = null;
            }
        }


    }
}