using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System;
using System.Web;

namespace _MyConnection
{
    /// <summary>
    /// Provides access to sql database.
    /// </summary>
    public class SqlHelper
    {

        public DataTable Dt_Comissions
        {
            get { if (HttpContext.Current.Cache[LoginInfo.GetUserID + "_dtComm"] != null) { return (DataTable)HttpContext.Current.Cache[LoginInfo.GetUserID + "_dtComm"]; } else { return (DataTable)HttpContext.Current.Cache[LoginInfo.GetUserID + "_dtComm"]; } }
        }

        static SqlConnection CONNECTION = new SqlConnection(ConfigurationManager.AppSettings["_CONSTR"].ToString());


        /// <summary>
        /// Reset the connection.
        /// </summary>
        static void ResetConnection()
        {
            if (CONNECTION.State != ConnectionState.Open)
            {
                CONNECTION.Open();
            }
        }
        public SqlConnection GetConnection { get { return CONNECTION; } }

        protected MEMBERS.SQLReturnValue ExecuteProcedureWithDatatable(string ProcedureName, DataTable dtExamAnswer, string TableParamName)
        {
            SqlCommand COMMAND = new SqlCommand();
            COMMAND.CommandText = ProcedureName;
            COMMAND.Connection = CONNECTION;
            COMMAND.CommandTimeout = 0;
            COMMAND.CommandType = CommandType.StoredProcedure;
            //SqlParameter[] param = new SqlParameter[ParamValue.GetUpperBound(0) + 1];
            //for (int i = 0; i < param.Length; i++)
            //{
            //    param[i] = new SqlParameter("@" + ParamValue[i, 0].ToString(), ParamValue[i, 1].ToString());
            //}
           // COMMAND.Parameters.AddRange(param);
            if (dtExamAnswer != null)
            {
                SqlParameter ParamTb = new SqlParameter("@" + TableParamName, dtExamAnswer);
                ParamTb.SqlDbType = SqlDbType.Structured;
                COMMAND.Parameters.Add(ParamTb);
            }


            COMMAND.Parameters.Add("@OUTVAL", SqlDbType.Int).Direction = ParameterDirection.Output;
            COMMAND.Parameters.Add("@OUTMESSAGE", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

            if (CONNECTION.State != ConnectionState.Open) { CONNECTION.Open(); }
            COMMAND.ExecuteNonQuery();
            CONNECTION.Close();
            MEMBERS.SQLReturnValue M = new MEMBERS.SQLReturnValue();
            M.ValueFromSQL = int.Parse(COMMAND.Parameters["@OUTVAL"].Value.ToString());
            M.MessageFromSQL = COMMAND.Parameters["@OUTMESSAGE"].Value.ToString();
            return M;


        }
        ///<summary>
        /// Execute the give procedure with provided parameter collection.
        /// </summary>
        /// <param name="ProcedureName">String name of the procedure.</param>
        /// <param name="param">Collection of the SQL Parameter</param>
        /// <param name="AddOutputParameters">Optionally add default output parameters to current parameter collection.</param>
        /// <returns>Returns the associated Sql Command object.</returns>    
        protected static SqlCommand ExecuteProcedure(string ProcedureName, SqlParameter[] param, bool AddOutputParameters)
        {
            try
            {
                SqlCommand COMMAND = new SqlCommand();
                COMMAND.CommandText = ProcedureName;
                SqlConnection MYCON = new SqlConnection(CONNECTION.ConnectionString);
                COMMAND.Connection = MYCON;
                COMMAND.CommandType = CommandType.StoredProcedure;
                COMMAND.Parameters.AddRange(param);
                if (AddOutputParameters == true)
                {
                    COMMAND.Parameters.Add("@OUTVAL", SqlDbType.Int).Direction = ParameterDirection.Output;
                    COMMAND.Parameters.Add("@OUTMESSAGE", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                }
                if (MYCON.State != ConnectionState.Open) { MYCON.Open(); }
                COMMAND.CommandTimeout = 0;
                COMMAND.ExecuteNonQuery();
                MYCON.Close();
                return COMMAND;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///<summary>
        /// Execute the give procedure with provided parameter collection.
        /// </summary>
        /// <param name="ProcedureName">String name of the procedure.</param>
        /// <param name="ParamValue">Collection of the SQL Parameter as two dimensional array.</param>
        /// <param name="AddOutputParameters">Optionally add default output parameters to current parameter collection.</param>
        /// <returns>Returns the associated Sql Command object.</returns>    
        protected static SqlCommand ExecuteProcedure(string ProcedureName, string[,] ParamValue, bool AddOutputParameters)
        {
            try
            {
                SqlCommand COMMAND = new SqlCommand();
                COMMAND.CommandText = ProcedureName;
                SqlConnection MYCON = new SqlConnection(CONNECTION.ConnectionString);
                COMMAND.Connection = MYCON;
                COMMAND.CommandType = CommandType.StoredProcedure;
                SqlParameter[] param = new SqlParameter[ParamValue.GetUpperBound(0) + 1];
                for (int i = 0; i < param.Length; i++)
                {
                    param[i] = new SqlParameter("@" + ParamValue[i, 0].ToString(), ParamValue[i, 1].ToString());
                }
                COMMAND.Parameters.AddRange(param);
                if (AddOutputParameters == true)
                {
                    COMMAND.Parameters.Add("@OUTVAL", SqlDbType.Int).Direction = ParameterDirection.Output;
                    COMMAND.Parameters.Add("@OUTMESSAGE", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                }
                if (MYCON.State != ConnectionState.Open) { MYCON.Open(); }
                COMMAND.ExecuteNonQuery();
                MYCON.Close();
                return COMMAND;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static MEMBERS.SQLReturnValue ExecuteProcWithMessage(string ProcedureName, string[,] ParamValue, bool AddOutputParameters)
        {
            try
            {
                SqlCommand COMMAND = new SqlCommand();
                COMMAND.CommandText = ProcedureName;
                SqlConnection MYCON = new SqlConnection(CONNECTION.ConnectionString);
                COMMAND.Connection = MYCON;
                COMMAND.CommandType = CommandType.StoredProcedure;
                SqlParameter[] param = new SqlParameter[ParamValue.GetUpperBound(0) + 1];
                for (int i = 0; i < param.Length; i++)
                {
                    param[i] = new SqlParameter("@" + ParamValue[i, 0].ToString(), ParamValue[i, 1].ToString());
                }
                COMMAND.Parameters.AddRange(param);
                if (AddOutputParameters == true)
                {
                    COMMAND.Parameters.Add("@OUTVAL", SqlDbType.Int).Direction = ParameterDirection.Output;
                    COMMAND.Parameters.Add("@OUTMESSAGE", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                }
                if (MYCON.State != ConnectionState.Open) { MYCON.Open(); }
                COMMAND.ExecuteNonQuery();
                MYCON.Close();
                MEMBERS.SQLReturnValue M = new MEMBERS.SQLReturnValue();
                M.MessageFromSQL = COMMAND.Parameters["@OUTMESSAGE"].Value.ToString();
                M.ValueFromSQL = int.Parse(COMMAND.Parameters["@OUTVAL"].Value.ToString());
                return M;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static MEMBERS.SQLReturnValue ExecuteProcWithMessageValue(string ProcedureName, string[,] ParamValue, bool AddOutputParameters)
        {
            try
            {
                SqlCommand COMMAND = new SqlCommand();
                COMMAND.CommandText = ProcedureName;
                SqlConnection MYCON = new SqlConnection(CONNECTION.ConnectionString);
                COMMAND.Connection = MYCON;
                COMMAND.CommandType = CommandType.StoredProcedure;
                SqlParameter[] param = new SqlParameter[ParamValue.GetUpperBound(0) + 1];
                for (int i = 0; i < param.Length; i++)
                {
                    param[i] = new SqlParameter("@" + ParamValue[i, 0].ToString(), ParamValue[i, 1].ToString());
                }
                COMMAND.Parameters.AddRange(param);
                if (AddOutputParameters == true)
                {

                    COMMAND.Parameters.Add("@OUTMESSAGE", SqlDbType.VarChar, 5000).Direction = ParameterDirection.Output;
                    COMMAND.Parameters.Add("@OUTVAL", SqlDbType.Int).Direction = ParameterDirection.Output;
                }
                if (MYCON.State != ConnectionState.Open) { MYCON.Open(); }
                COMMAND.ExecuteNonQuery();
                MYCON.Close();
                MEMBERS.SQLReturnValue M = new MEMBERS.SQLReturnValue();
                M.MessageFromSQL = COMMAND.Parameters["@OUTMESSAGE"].Value.ToString();
                M.ValueFromSQL = int.Parse(COMMAND.Parameters["@OUTVAL"].Value.ToString());
                return M;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static MEMBERS.SQLReturnValue ExecuteProcWithMessageValue2(string ProcedureName, string[,] ParamValue, bool AddOutputParameters)
        {
            try
            {
                SqlCommand COMMAND = new SqlCommand();
                COMMAND.CommandText = ProcedureName;
                SqlConnection MYCON = new SqlConnection(CONNECTION.ConnectionString);
                COMMAND.Connection = MYCON;
                COMMAND.CommandType = CommandType.StoredProcedure;
                SqlParameter[] param = new SqlParameter[ParamValue.GetUpperBound(0) + 1];
                for (int i = 0; i < param.Length; i++)
                {
                    param[i] = new SqlParameter("@" + ParamValue[i, 0].ToString(), ParamValue[i, 1].ToString());
                }
                COMMAND.Parameters.AddRange(param);
                if (AddOutputParameters == true)
                {
                    COMMAND.Parameters.Add("@OUTMESSAGE1", SqlDbType.VarChar, 5000).Direction = ParameterDirection.Output;
                    COMMAND.Parameters.Add("@OUTMESSAGE", SqlDbType.VarChar, 5000).Direction = ParameterDirection.Output;
                    COMMAND.Parameters.Add("@OUTVAL", SqlDbType.Int).Direction = ParameterDirection.Output;
                }
                if (MYCON.State != ConnectionState.Open) { MYCON.Open(); }
                COMMAND.ExecuteNonQuery();
                MYCON.Close();
                MEMBERS.SQLReturnValue M = new MEMBERS.SQLReturnValue();
                M.MessageFromSQL1 = COMMAND.Parameters["@OUTMESSAGE1"].Value.ToString();
                M.MessageFromSQL = COMMAND.Parameters["@OUTMESSAGE"].Value.ToString();
                M.ValueFromSQL = int.Parse(COMMAND.Parameters["@OUTVAL"].Value.ToString());
                return M;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executes the procedure which contains the data from the sql table.
        /// </summary>
        /// <param name="ProcedureName">String Name of the procedure.</param>
        /// <param name="param">Collection of parameters.</param>
        /// <returns>Datatable with data from server.</returns>
        protected static DataTable ExecuteProcedure(string ProcedureName, SqlParameter[] param)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(ExecuteProcedure(ProcedureName, param, false));
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Executes the procedure and return output value and message from sql server.
        /// </summary>
        /// <param name="ProcedureName">Name of procedure</param>
        /// <param name="param">Collection of sql parameters</param>
        /// <returns></returns>
        protected static MEMBERS.SQLReturnValue ExecuteProcedureReturnValue(string ProcedureName, SqlParameter[] param)
        {
            try
            {
                MEMBERS.SQLReturnValue returnval = new MEMBERS.SQLReturnValue();
                SqlCommand COMMAND = new SqlCommand();
                COMMAND.CommandText = ProcedureName;
                SqlConnection MYCON = new SqlConnection(CONNECTION.ConnectionString);
                COMMAND.Connection = MYCON;
                COMMAND.CommandType = CommandType.StoredProcedure;
                COMMAND.Parameters.AddRange(param);
                ///Adds the output parameter
                COMMAND.Parameters.Add("@OUTVAL", SqlDbType.Int).Direction = ParameterDirection.Output;
                COMMAND.Parameters.Add("@OUTMESSAGE", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                if (MYCON.State != ConnectionState.Open) { MYCON.Open(); }
                COMMAND.ExecuteNonQuery();
                MYCON.Close();
                ///Retrive value from output parameters to return value structure.
                returnval.MessageFromSQL = COMMAND.Parameters["@OUTMESSAGE"].Value.ToString();
                returnval.ValueFromSQL = int.Parse(COMMAND.Parameters["@OUTVAL"].Value.ToString());


                return returnval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executes the procedure and return output value and message from sql server.
        /// </summary>
        /// <param name="ProcedureName">Name of procedure</param>
        /// <param name="ParamValue">Collection of sql parameters as two dimentional array.</param>
        /// <returns>Returns Return values from sql procedure.</returns>
        protected static MEMBERS.SQLReturnValue ExecuteProcedureReturnValue(string ProcedureName, string[,] ParamValue)
        {
            try
            {
                MEMBERS.SQLReturnValue returnval = new MEMBERS.SQLReturnValue();
                SqlCommand COMMAND = new SqlCommand();
                COMMAND.CommandText = ProcedureName;
                SqlConnection MYCON = new SqlConnection(CONNECTION.ConnectionString);
                COMMAND.Connection = MYCON;
                COMMAND.CommandType = CommandType.StoredProcedure;
                SqlParameter[] param = new SqlParameter[ParamValue.GetUpperBound(0) + 1];
                for (int i = 0; i < param.Length; i++)
                {
                    param[i] = new SqlParameter("@" + ParamValue[i, 0].ToString(), ParamValue[i, 1].ToString());
                }
                COMMAND.Parameters.AddRange(param);
                ///Adds the output parameter
                COMMAND.Parameters.Add("@OUTVAL", SqlDbType.Int).Direction = ParameterDirection.Output;
                COMMAND.Parameters.Add("@OUTMESSAGE", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                if (MYCON.State != ConnectionState.Open) { MYCON.Open(); }
                COMMAND.ExecuteNonQuery();
                MYCON.Close();
                ///Retrive value from output parameters to return value structure.
                returnval.MessageFromSQL = COMMAND.Parameters["@OUTMESSAGE"].Value.ToString();
                returnval.ValueFromSQL = int.Parse(COMMAND.Parameters["@OUTVAL"].Value.ToString());

                return returnval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executes the procedure and return output value and message from sql server.
        /// </summary>
        /// <param name="ProcedureName">Name of procedure</param>
        /// <param name="param">Collection of sql parameters</param>
        /// <returns></returns>
        protected static MEMBERS.SQlReturnInteger ExecuteProcedureReturnInteger(string ProcedureName, SqlParameter[] param)
        {
            try
            {
                MEMBERS.SQlReturnInteger returnval = new MEMBERS.SQlReturnInteger();
                SqlCommand COMMAND = new SqlCommand();
                COMMAND.CommandText = ProcedureName;
                SqlConnection MYCON = new SqlConnection(CONNECTION.ConnectionString);
                COMMAND.Connection = MYCON;
                COMMAND.CommandType = CommandType.StoredProcedure;
                COMMAND.Parameters.AddRange(param);
                ///Adds the output parameter
                COMMAND.Parameters.Add("@OUTVAL", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (MYCON.State != ConnectionState.Open) { MYCON.Open(); }
                COMMAND.ExecuteNonQuery();
                MYCON.Close();
                ///Retrive value from output parameters to return value structure.
                returnval.ValueFromSQL = int.Parse(COMMAND.Parameters["@OUTVAL"].Value.ToString());


                return returnval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Executes the procedure and return output value and message from sql server.
        /// </summary>
        /// <param name="ProcedureName">Name of procedure</param>
        /// <param name="ParamValue">Collection of sql parameters as two dimentional array.</param>
        /// <returns>Returns Return values from sql procedure.</returns>
        protected static MEMBERS.SQlReturnInteger ExecuteProcedureReturnInteger(string ProcedureName, string[,] ParamValue)
        {
            try
            {
                MEMBERS.SQlReturnInteger returnval = new MEMBERS.SQlReturnInteger();
                SqlCommand COMMAND = new SqlCommand();
                COMMAND.CommandText = ProcedureName;
                SqlConnection MYCON = new SqlConnection(CONNECTION.ConnectionString);
                COMMAND.Connection = MYCON;
                COMMAND.CommandType = CommandType.StoredProcedure;
                SqlParameter[] param = new SqlParameter[ParamValue.GetUpperBound(0) + 1];
                for (int i = 0; i < param.Length; i++)
                {
                    param[i] = new SqlParameter("@" + ParamValue[i, 0].ToString(), ParamValue[i, 1].ToString());
                }
                COMMAND.Parameters.AddRange(param);
                ///Adds the output parameter
                COMMAND.Parameters.Add("@OUTVAL", SqlDbType.Int).Direction = ParameterDirection.Output;

                if (MYCON.State != ConnectionState.Open) { MYCON.Open(); }
                COMMAND.ExecuteNonQuery();
                MYCON.Close();
                ///Retrive value from output parameters to return value structure.
                returnval.ValueFromSQL = int.Parse(COMMAND.Parameters["@OUTVAL"].Value.ToString());

                return returnval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Executes SQL Query.
        /// </summary>
        /// <param name="Query">String Sql Statement</param>
        /// <returns>Returns datatable associated with the SQL Query.</returns>
        protected static DataTable ExecuteQuery(string Query)
        {
            try
            {
                SqlConnection MYCON = new SqlConnection(CONNECTION.ConnectionString);

                SqlDataAdapter da = new SqlDataAdapter(Query, MYCON);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetDataUsingQuery(string Query)
        {
            try
            {
                SqlConnection MYCON = new SqlConnection(CONNECTION.ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter(Query, MYCON);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Adds Parameter and its value to SqlParameter Array collection.
        /// </summary>
        /// <param name="ParamValue">Two dimentional array with First => Parameter, Second => Value.</param>
        /// <returns>Returns Sql Parameter collection containing parameter and its values.</returns>
        protected static SqlParameter[] AddParameterAndExecute(string[,] ParamValue)
        {
            SqlParameter[] param = new SqlParameter[ParamValue.GetUpperBound(0) + 1];
            for (int i = 0; i < param.Length; i++)
            {
                param[i] = new SqlParameter("@" + ParamValue[i, 0].ToString(), ParamValue[i, 1].ToString());
            }
            return param;
        }
        protected static DataTable ExecuteProcedureWithReturnDatatable(string ProcedureName, SqlParameter[] param)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(ExecuteProcedure(ProcedureName, param, false));
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int DML(string q)
        {
            int i = 0;
            try
            {
                SqlConnection MYCON = new SqlConnection(CONNECTION.ConnectionString);
                if (MYCON.State != ConnectionState.Open) { MYCON.Open(); }
                SqlCommand cmd = new SqlCommand(q, MYCON);
                i = cmd.ExecuteNonQuery();
                MYCON.Close();

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return i;
        }

        public double ExecuteProc(string ProcName, SqlCommand command)
        {
            SqlHelper sqlhelp = new SqlHelper();
            SqlConnection sn = new SqlConnection("Data source=184.173.167.8,2433;Database=useradmin_shreenavrang;Uid=newnavrang;Password=34mEol?5;");
            //SqlConnection con = sn.GetConnection;
            //if (sn.GetConnection.State != ConnectionState.Open)
            //{ sn.GetConnection.Open(); }
            sn.Open();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = ProcName;
            command.Connection = sn;
            double ret = command.ExecuteNonQuery();
            sn.Close();
            return ret;
        }

        #region Pagging Related
        public static DataTable PaggingData(Int32 PageNo, Int32 PageSize, ref Int32 TotalRecords, string ProcedureName)
        {
            SqlConnection MYCON = new SqlConnection(CONNECTION.ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = ProcedureName;
            cmd.Connection = MYCON;

            cmd.Parameters.AddWithValue("@PageIndex", PageNo);
            cmd.Parameters.AddWithValue("@PageSize", PageSize);

            //SqlParameter[] param = new SqlParameter[ParamValue.GetUpperBound(0) + 1];
            //for (int i = 0; i < param.Length; i++)
            //{
            //    param[i] = new SqlParameter("@" + ParamValue[i, 0].ToString(), ParamValue[i, 1].ToString());
            //}
            //cmd.Parameters.AddRange(param);

            cmd.Parameters.Add("@RecordCount", SqlDbType.Int).Direction = ParameterDirection.Output;

            if (MYCON.State != ConnectionState.Open) { MYCON.Open(); }
            SqlDataAdapter dGet = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dGet.Fill(dt);
            TotalRecords = int.Parse(cmd.Parameters["@RecordCount"].Value.ToString());
            MYCON.Close();
            return dt;
        }

        public static DataSet PaggingDataDS(Int32 PageNo, Int32 PageSize, ref Int32 TotalRecords, string ProcedureName, string[,] ParamValue)
        {
            SqlConnection MYCON = new SqlConnection(CONNECTION.ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = ProcedureName;
            cmd.Connection = MYCON;

            cmd.Parameters.AddWithValue("@PageIndex", PageNo);
            cmd.Parameters.AddWithValue("@PageSize", PageSize);

            SqlParameter[] param = new SqlParameter[ParamValue.GetUpperBound(0) + 1];
            for (int i = 0; i < param.Length; i++)
            {
                param[i] = new SqlParameter("@" + ParamValue[i, 0].ToString(), ParamValue[i, 1].ToString());
            }
            cmd.Parameters.AddRange(param);

            cmd.Parameters.Add("@RecordCount", SqlDbType.Int).Direction = ParameterDirection.Output;

            if (MYCON.State != ConnectionState.Open) { MYCON.Open(); }
            SqlDataAdapter dGet = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            dGet.Fill(ds);
            TotalRecords = int.Parse(cmd.Parameters["@RecordCount"].Value.ToString());
            MYCON.Close();
            return ds;
        }
        #endregion Pagging Related
    }
}