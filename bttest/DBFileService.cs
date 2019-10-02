using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bttest
{
    class DBFileService
    {
        private char[] SpecialChars = new char[] { ',', '"', '\r', '\n' };
        public static string[] tableNames = new string[] { "tbblackbatch", "tbbom", "tbcode", "tbequipment", "tbequipmentspindlelog", "tbfaultcode", "tbkits", "tbkitslog", "tbmaintenancetype", "tbmajorparts", "tbmajorpartslog", "tbmanufacturer", "tborder", "tborderdetail", "tbparts", "tbpartsapply", "tbpartsapplydetail", "tbpartslog", "tbpartswarehouse", "tbpartswarehouselog", "tbrebuilt", "tbrebuiltlist", "tbreplace", "tbtesting", "tbtestingtype", "tbtestingval", "tbtestparam", "tbtesttype", "tbuser" };
        public bool exportDBdata2Txt(string directory)
        {

            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "hhg3";
            if (dbCon.IsConnect())
            {

                //suppose col0 and col1 are defined as VARCHAR in the DB
                //  string query = "SELECT user,password FROM user";
                //  string query = "SELECT User, Password INTO OUTFILE '/mysqlfiles/mysql_user1.txt' FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '\"' ESCAPED BY '\\' LINES TERMINATED BY '\n' FROM mysql.user";
                //string query = "SELECT User, Password INTO OUTFILE '/mysqlfiles/mysql_user2.txt' FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '{0}' LINES TERMINATED BY '\n' FROM user; ";
                if (directory == null || directory.Equals(""))
                {
                    directory = "/mysqlfiles/";
                }
                else
                {
                    directory = "/bt/";
                }
                foreach (string tbName in tableNames)
                {
                    executeTableQuery(directory, tbName, dbCon.Connection);
                }
                // string str = @"";
                //string parameter = Regex.Unescape(query);
                //   string parameter = query.Replace("\\", "");
               
                //var reader = cmd.ExecuteReader();
                //while (reader.Read())
                //{
                //    string someStringFromColumnZero = reader.GetString(0);
                //    string someStringFromColumnOne = reader.GetString(1);
                //    Console.WriteLine(someStringFromColumnZero + "," + someStringFromColumnOne);
                //}
               // dbCon.Close();
            }
            return true;
        }

        private void executeTableQuery(string directory, string tbName, MySqlConnection connection) {
            try
            {
                string syncTime = getSyncDBTime(connection);
                //syncTime = "2019-05-15 20:16:07";
                //syncTime = "";
                string formatedStr = "";
                if (syncTime == "")
                {
                    string query = "SELECT  * FROM {0}  INTO OUTFILE '{1}{2}.txt' FIELDS TERMINATED BY ';' OPTIONALLY ENCLOSED BY '{3}' LINES TERMINATED BY '\n' ; ";
                    formatedStr = String.Format(query, tbName, directory, tbName, SpecialChars[1]);
                }
                else
                {
                    string query = "SELECT  * FROM {0} where updatedate >= '{1}'  INTO OUTFILE '{2}{3}.txt' FIELDS TERMINATED BY ';' OPTIONALLY ENCLOSED BY '{4}' LINES TERMINATED BY '\n' ; ";
                    formatedStr = String.Format(query, tbName, syncTime, directory, tbName, SpecialChars[1]);
                }
                var cmd = new MySqlCommand(formatedStr, connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ErrorLog.WriteLog(e);
            }
            finally
            {
            }
        }

        internal void insertSyncDBTime()
        {
            MySqlConnection connection =  null;
            try
            {
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = "hhg3";
                if (dbCon.IsConnect())
                {
                    connection = dbCon.Connection;
                    string querySql = "select max(id) from tbuploadhis";
                    var cmd = new MySqlCommand(querySql, connection);
                    int id = 1;
                    using (var Reader = cmd.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            id = Reader.IsDBNull(0) ? 1 : Reader.GetInt32(0)+1;
                        }
                    }
                    var uploaddate = ConvertDateTimeToLong(DateTime.Now).ToString();
                    string insertSql = "insert into tbuploadhis(id, uploaddate, uploadnote, compid, departid, reserve1, reserve2, reserve3, reserve4, reserve5, status, createuser, createdate, updateuser, updatedate) values({0},{1},'local',0,0,'','','','','',1,'local',NOW(),'local',NOW())";
                    var formatedStr = String.Format(insertSql, id, uploaddate);
                    var insertCmd = new MySqlCommand(formatedStr, connection);
                    insertCmd.ExecuteNonQuery();
                }
            }catch(Exception e)
            {
                ErrorLog.WriteLog(e);
            }
            finally
            {
            }
            
        }
        private long ConvertDateTimeToLong(DateTime dt)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = dt.Subtract(dtStart);
            long timeStamp = toNow.Ticks;
            timeStamp = long.Parse(timeStamp.ToString().Substring(0, timeStamp.ToString().Length - 4));
            return timeStamp;
        }

        internal string getSyncDBTime(MySqlConnection connection)
        {
            try { 
                string query = "SELECT a.uploaddate FROM tbuploadhis a RIGHT OUTER JOIN ( SELECT max(id) as maxid FROM tbuploadhis T WHERE T.status = 1 ) b ON a.id = b.maxid";
                var cmd = new MySqlCommand(query, connection);
                string syncTime = "";
                using (var Reader = cmd.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        syncTime = Reader.IsDBNull(0) ? "" : Reader.GetString(0);
                    }
                }
                if (syncTime == "")
                {
                    return "";
                }
                else
                {
                    DateTime syncDateTime = ConvertLongToDateTime(long.Parse(syncTime));
                    return  syncDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            catch (Exception e)
            {
                ErrorLog.WriteLog(e);
            }
            finally
            {
            }
            return "";
        }

        private DateTime ConvertLongToDateTime(long d)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(d + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult;
        }
    }
}
