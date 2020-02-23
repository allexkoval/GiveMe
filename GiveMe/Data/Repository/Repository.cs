using GiveMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveMe.Data.Repository
{
    public class Repository : IRepo
    {
        private Data.IRepository _ctx;

        public Repository(Data.IRepository ctx)
        {
            _ctx = ctx;
        }
        public void AddPost(Project post)
        {
            _ctx.Posts.Add(post);
        }

        public List<Project> GetAllPosts()
        {
            return _ctx.Posts.ToList();
        }

        public Project GetPost(int id)
        {
            return _ctx.Posts.FirstOrDefault(p => p.Id == id);
        }

        public void RemovePost(int id)
        {
            _ctx.Posts.Remove(GetPost(id));
        }

        public void UpdatePost(Project post)
        {
            _ctx.Posts.Update(post);
        }

        public async Task<bool> SaveChangesAsync()
        {
            if(await _ctx.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
