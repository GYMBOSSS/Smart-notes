using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace WpfApp1
{
    internal class DB_API
    {

        static string connectionString = "Server = 127.0.0.1;Port=11111;Database=SmartNotesDB;UserId=postgres;Password=sp1DVAGON";
        const string sqlSELECT = @"
            SELECT
                n.id,
                n.title,
                n.details,
                n.created_at,
                n.make_up_to,
                n.is_completed,
                t.id AS tag_id
            FROM notes n
            LEFT JOIN tags t on n.tag_id = t.id";

        const string sqlINSERTnote = @"
            INSERT INTO notes (title,details,tag_id,created_at,make_up_to,is_completed)
            VALUES (@title, @details, @tag_id, @created_at, @make_up_to, @is_completed)";
        
        const string sqlINSERTtag = @"
            INSERT INTO tags (tag_priority, tag_name, color)
            VALUES (@tag_priority, @tag_name, @color)";
        async public static Task LoadNotesAndTagsFromDB_Async(MainWindow mainWindow){
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(sqlSELECT, conn);
                var reader = cmd.ExecuteReader();
                int idIndex = reader.GetOrdinal("id");
                int detIndex = reader.GetOrdinal("details");
                int mainIndex = reader.GetOrdinal("title");
                int cr_atIndex = reader.GetOrdinal("created_at");
                int makeIndex = reader.GetOrdinal("make_up_to");
                int isCompIndex = reader.GetOrdinal("is_completed");
                
                int? tagIndex = reader.GetOrdinal("tag_id");
                int? tagPrioIndex = reader.GetOrdinal("tag_priority");
                int? tagNameIndex = reader.GetOrdinal("tag_name");
                int? colorIndex = reader.GetOrdinal("color");
                while (await reader.ReadAsync())
                {
                    var note = new Note
                    {
                        ID = reader.GetInt32(idIndex),
                        main = reader.GetString(mainIndex),
                        details = reader.IsDBNull(detIndex) ? null : reader.GetString(detIndex),
                        createDateTime = reader.GetDateTime(cr_atIndex),
                        makeUpToDateTime = reader.GetDateTime(makeIndex),
                        IsCompleeted = reader.GetBoolean(isCompIndex)
                    };
                    if (!reader.IsDBNull(tagIndex.Value))
                    {
                        var tag = new NoteTag
                        {
                            ID = reader.GetInt32(tagIndex.Value),
                            tagPriority = reader.GetInt32(tagPrioIndex.Value),
                            tagName = reader.GetString(tagNameIndex.Value),
                            tagColorHex = reader.GetString(colorIndex.Value)
                        };
                        note.tag = tag;
                        mainWindow.AddTagToList(tag);
                    }
                    mainWindow.AddNoteToPanel(note);
                    mainWindow.AddNoteToList(note);
                }
            }
        }
        async public static Task LoadNoteToDB(Note note) 
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(sqlINSERTnote,conn);
                cmd.Parameters.AddWithValue("id",note.ID);
                cmd.Parameters.AddWithValue("title", note.main);
                cmd.Parameters.AddWithValue("details", note.details);
                cmd.Parameters.AddWithValue("tag_id", note.tag.ID);
                cmd.Parameters.AddWithValue("created_at", note.createDateTime);
                cmd.Parameters.AddWithValue("make_up_to", note.makeUpToDateTime);
                cmd.Parameters.AddWithValue("is_completed", note.IsCompleeted);
                cmd.ExecuteNonQuery();
            }
        }
        async public static Task LoadTagToDB(NoteTag tag)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(sqlINSERTtag,conn);
                cmd.Parameters.AddWithValue("id", tag.ID);
                cmd.Parameters.AddWithValue("tag_priority", tag.tagPriority);
                cmd.Parameters.AddWithValue("tag_name", tag.tagName);
                cmd.Parameters.AddWithValue("color",tag.tagColorHex);
                cmd.ExecuteNonQuery();
            }
        }
        async public static Task DeleteNoteFromDB(int note_id)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                const string sqlDELETEnote = @"DELETE FROM notes WHERE id = @id";
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(sqlDELETEnote,conn);
                cmd.Parameters.AddWithValue("id", note_id);
                cmd.ExecuteNonQuery();
            }
        }
        async public static Task DeleteTagFromDB(int tag_id)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                const string sqlDELETEtag = @"DELETE FROM tags WHERE id = @id";
                const string sqlDELETEnote = @"DELETE FROM notes WHERE tag_id = @id";
                conn.Open();
                NpgsqlCommand cmd1 = new NpgsqlCommand(sqlDELETEnote, conn);
                cmd1.Parameters.AddWithValue("id", tag_id);
                cmd1.ExecuteNonQuery();
                NpgsqlCommand cmd2 = new NpgsqlCommand(sqlDELETEtag, conn);
                cmd2.Parameters.AddWithValue("id", tag_id);
                cmd2.ExecuteNonQuery();
            }
        }
    }
}
