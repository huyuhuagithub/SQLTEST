using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace SQLTEST
{
    public class SQLHelper
    {
        static string connstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;

        /// <summary>
        /// 获取实体单行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetT<T>(int id)
        {
            Type type = typeof(T);
            object obj = Activator.CreateInstance(type);
            string colums = string.Join(",", type.GetProperties().Select(p => string.Format($"[{p.Name}]")));
            string sql = string.Format($"SELECT {colums} FROM {type.Name} where id={id}");
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    foreach (var item in type.GetProperties())
                    {
                        if (reader[item.Name] is DBNull)
                        {
                            item.SetValue(obj, null);
                        }
                        item.SetValue(obj, reader[item.Name]);
                    }
                }
            }
            return (T)obj;
        }

        /// <summary>
        /// 获取实体列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetEntitylist<T>() where T : BaseModel
        {
            Type type = typeof(T);
            string cloumsstring = string.Join(",", type.GetProperties().Select(p => string.Format($"[{p.Name}]")));
            string sqlString = string.Format($"select {cloumsstring} from {type.Name}");
            List<T> datalist = new List<T>();
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sqlString, conn);
                SqlDataReader read = command.ExecuteReader();
                while (read.Read())
                {
                    var t = Activator.CreateInstance(type);
                    foreach (var item in type.GetProperties())
                    {
                        if (read[item.Name] is DBNull)
                        {
                            item.SetValue(t, null);
                        }
                        item.SetValue(t, read[item.Name]);
                    }
                    datalist.Add(t as T);
                }
            }
            return datalist;
        }

        /// <summary>
        /// 参数化插入实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool InsertEntity<T>(T t) where T : BaseModel
        {

            Type type = typeof(T);
            string columnString = string.Join(",", type.GetProperties()/*.Where(p=>p.Name!="id")*/.Select(p => string.Format($"[{p.Name}]")));
            //string valueString = string.Join(",", type.GetProperties()/*.Where(p => p.Name != "id")*/.Select(p => string.Format($"[{p.GetValue(t)}]")));
            string valueString = string.Join(",", type.GetProperties()/*.Where(p=>p.Name!="id")*/.Select(p => string.Format($"@{p.Name}")));


            string sqlText = $"insert into [{type.Name}] ({columnString}) values({valueString})";
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sqlText, conn);
                SqlParameter[] sqlParameter = type.GetProperties()/*.Where(p => !"id".Equals(p.Name))*/.
                    Select(p => new SqlParameter(string.Format($"@{p.Name}"), p.GetValue(t) ?? DBNull.Value)).ToArray();
                command.Parameters.AddRange(sqlParameter);
                return command.ExecuteNonQuery() > 0;
            }

        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteEntity<T>(int id)
        {
            Type type = typeof(T);
            string sqlText = string.Format($"Delete from [{type.Name}] where id={id}");
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                SqlCommand sqlCommand = new SqlCommand(sqlText, conn);
                return sqlCommand.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool UpdateEntity<T>(T t) where T : BaseModel
        {
            Type type = typeof(T);
            var propArray = type.GetProperties();

            string columnString = string.Join(",", type.GetProperties()/*.Where(p=>p.Name!="id")*/.Select(p => string.Format($"[{p.Name}]=@{p.Name}")));
            //string valueString = string.Join(",", type.GetProperties()/*.Where(p => p.Name != "id")*/.Select(p => string.Format($"[{p.GetValue(t)}]")));
            var parameters = propArray.Select(p => new SqlParameter($"@{p.Name}", p.GetValue(t) ?? DBNull.Value)).ToArray();
            string sql = string.Format($"Update [{type.Name}] set {columnString} where id={t.id}");

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddRange(parameters);
                conn.Open();
                int iResult = command.ExecuteNonQuery();
                if (iResult == 0)
                    throw new Exception("Update数据不存在");
                return true;

            }
        }

        private W Dbcommand<T, W>(string strSql, Func<IDbCommand, W> func) where T : BaseModel
        {
            using (IDbConnection conn = new SqlConnection(connstring))
            {
                //1:打开数据库连接
                conn.Open();
                //2：创建数据库命令
                IDbCommand com = conn.CreateCommand();
                com.CommandText = strSql;
                com.CommandType = CommandType.Text;
                //3:执行链接后的方法

                return func(com);
            }
        }

    }
}
