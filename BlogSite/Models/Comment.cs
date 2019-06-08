using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace BlogSite.Models
{
  public class Comment
  {
    private int _id;
    private int _postId;
    private int _authorId;
    private string _content;
    private DateTime _timestamp;

    public int GetId()
    {
        return _id;
    }
    public int GetPostId()
    {
        return _postId;
    }
    public int GetAuthorId()
    {
        return _authorId;
    }
    public string GetContent()
    {
        return _content;
    }
    public DateTime GetTimestamp()
    {
        return _timestamp;
    }

    public Comment(int postId, int authorId, string content, int id=0)
    {
        _postId = postId;
        _authorId = authorId;
        _content = content;
        _id = id;
        _timestamp = DateTime.Now;
    }

    public void Save()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO comments (author_id, post_id, timestamp, content) VALUES (@author_id, @post_id, @timestamp, @content);";
        
        MySqlParameter authorId = new MySqlParameter();
        authorId.ParameterName = "@author_id";
        authorId.Value = this._authorId;
        cmd.Parameters.Add(authorId);

        MySqlParameter postId = new MySqlParameter();
        postId.ParameterName = "@post_id";
        postId.Value = this._postId;
        cmd.Parameters.Add(postId);

        MySqlParameter timestamp = new MySqlParameter();
        timestamp.ParameterName = "@timestamp";
        timestamp.Value = this._timestamp;
        cmd.Parameters.Add(timestamp);

        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId;
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
    
  }
}
    