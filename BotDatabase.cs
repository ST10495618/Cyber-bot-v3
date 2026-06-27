using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    class BotDatabase
    {
        private string connectionString =
           "server=localhost;database=cyberbot_tasks;uid=root;pwd=Innocent_23;";

        public void AddTask(string title, string description)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"INSERT INTO tasks
                               (title, description)
                               VALUES
                               (@title, @description)";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@description", description);

                cmd.ExecuteNonQuery();
            }
        }
        public string GetTasks()
        {
            string result = "";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM tasks";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result += $"ID: {reader["tasks_id"]} | " +
                              $"Task: {reader["title"]} | " +
                              $"Completed: {reader["is_completed"]}\n";
                }
            }

            return result;
        }
        public bool CompleteTask(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string sql = "UPDATE tasks SET is_completed = 1 WHERE tasks_id = @id";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool DeleteTask(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string sql = "DELETE FROM tasks WHERE tasks_id = @id";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public void SetReminder(int id, DateTime reminderDate)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string sql =
                    @"UPDATE tasks
              SET reminder_date = @date
              WHERE tasks_id = @id";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@date", reminderDate);

                cmd.ExecuteNonQuery();
            }
        }
        public int GetLatestTaskId()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string sql =
                    "SELECT MAX(tasks_id) FROM tasks";

                MySqlCommand cmd =
                    new MySqlCommand(sql, conn);

                object result = cmd.ExecuteScalar();

                return Convert.ToInt32(result);
            }
        }
    }
}
