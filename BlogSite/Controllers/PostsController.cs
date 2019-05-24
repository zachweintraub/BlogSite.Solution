using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using BlogSite.Models;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Web;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BlogSite.Controllers
{
  public class PostsController : Controller
  {

    [HttpGet("/posts/search")]
    public ActionResult Search()
    {
      List<Post> list = new List <Post> {};
      ViewBag.Title = "Find That Which You Seek";
       return View(list);
    }

    [HttpPost("/posts/search")]
    public ActionResult Search(string search)
    {

      List<Post> searchResult = Post.PostSearch(search);
      return View(searchResult);
    }


    [HttpGet("/blogs/{blogId}/posts/new")]
      public ActionResult New(int blogId)
      {
        ViewBag.Title = "Compose your masterpiece";
        return View(blogId);
      }



      [HttpPost("/blogs/{id}/posts/new")]
      public async Task <ActionResult> Create(string title, string content, IFormFile file, int id)
      {
        string imgName = null;
        if (file != null) 
        {
          long size = file.Length;
          if (file.Length > 0)
          {
          // 1. Find path where to store image 
          var fileName = DateTime.Now.Ticks + file.FileName;
          var filePath = Directory.GetCurrentDirectory() + "/wwwroot/uploads/" + fileName;
          // 2. Write image to file
              using (var stream = new FileStream(filePath, FileMode.Create))
              {
                  await file.CopyToAsync(stream);
              }
              imgName = fileName;
          }
        }
        Post myPost = new Post(title, content, imgName, id);
        myPost.Save();
        return RedirectToAction("Show", new{blogId = myPost.GetBlogId(), postId = myPost.GetId()});
      }


      [HttpPost("/blogs/{blogId}/posts/{postId}/update")]
      public async Task <ActionResult> Update(int blogId, int postId, string title, string content, IFormFile file)
      {
        string imgName = null;
        if (file != null) 
        {
          long size = file.Length;
          if (file.Length > 0)
          {
          var fileName = DateTime.Now.Ticks + file.FileName;
          var filePath = Directory.GetCurrentDirectory() + "/wwwroot/uploads/" + fileName;
              using (var stream = new FileStream(filePath, FileMode.Create))
              {
                  await file.CopyToAsync(stream);
              }
              imgName = fileName;
          }
        }
        Post editPost = Post.Find(postId);
        editPost.Edit(title, content, imgName);
        return RedirectToAction("Show", new{blogId = blogId, postId = postId});
      }


    [HttpGet("/blogs/{blogId}/posts/{postId}")]
    public ActionResult Show(int blogId, int postId)
    {
      Post myPost = Post.Find(postId);
      ViewBag.Title = Blog.FindById(blogId).GetTitle();
      return View(myPost);
    }

    [HttpGet("/blogs/{blogId}/posts/{postId}/edit")]
    public ActionResult Edit(int blogId, int postId)
    {
      Post editPost = Post.Find(postId);
      ViewBag.Title = "Change Things Up";
      return View(editPost);
    }


    [HttpPost("/blogs/{blogId}/posts/{postId}/delete")]
      public ActionResult Destroy(int blogId, int postId)
      {
        Post deletePost = Post.Find(postId);
        deletePost.Delete();
        return RedirectToAction("Show", "Blogs", new{id = blogId});
      }
  }
}