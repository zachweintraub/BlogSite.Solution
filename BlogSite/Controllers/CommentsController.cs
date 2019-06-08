using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using BlogSite.Models;
using System.Linq;
using MySql.Data.MySqlClient;

namespace BlogSite.Controllers
{
  public class CommentsController : Controller
  {
      [HttpPost("/blogs/{blogId}/posts/{postId}/comments/new")]
      public ActionResult Create(int blogId, int postId, int commenterId, string commentContent)
      {
        Comment myComment = new Comment(postId, commenterId, commentContent);
        myComment.Save();
        return RedirectToAction("Show", "Posts", new{blogId = blogId, postId = postId});
      }

     
  }
}