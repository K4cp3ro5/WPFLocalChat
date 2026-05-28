using System;
using SQLite;

namespace WPF_LocalChatSqlite.Models
{
    public class ChatMessage
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Nickname { get; set; }

        public string Text { get; set; }

        public DateTime Timestamp { get; set; }
        [Ignore]
        public bool IsOwnMessage { get; set; }
    }
}
