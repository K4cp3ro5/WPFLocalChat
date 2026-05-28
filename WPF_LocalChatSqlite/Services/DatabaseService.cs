using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite;
using WPF_LocalChatSqlite.Models;

namespace WPF_LocalChatSqlite.Services
{
    public static class DatabaseService
    {
        private static readonly string DbPath;
        private static readonly SQLiteConnection Db;

        static DatabaseService()
        {
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WPFLocalChat");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            DbPath = Path.Combine(folder, "chat.db");

            Db = new SQLiteConnection(DbPath);
            Db.CreateTable<ChatMessage>();
        }

        public static void SaveMessage(string nickname, string text)
        {
            var msg = new ChatMessage
            {
                Nickname = nickname,
                Text = text,
                Timestamp = DateTime.Now
            };
            Db.Insert(msg);
        }

        public static List<ChatMessage> LoadMessages(DateTime? since = null)
        {
            var query = Db.Table<ChatMessage>().OrderBy(m => m.Timestamp);
            if (since.HasValue)
            {
                return query.Where(m => m.Timestamp >= since.Value).ToList();
            }

            return query.ToList();
        }
    }
}
