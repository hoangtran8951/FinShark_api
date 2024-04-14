using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiSTockapi.Helpers;
using apiSTockapi.Models;
using Microsoft.EntityFrameworkCore;

namespace apiSTockapi.Interfaces
{
    public interface ICommentRepository
    {
        public Task<CommentModel> CreateAsync(CommentModel comment); // create new stock
        public Task<CommentModel?> UpdateAsync(CommentModel comment); // Update existing Stock
        public Task<List<CommentModel>> GetAllAsync(CommentQueryObject query); // Get All the existed Stocks
        public Task<CommentModel?> GetById(int id); // Get stock by id
        public Task<bool> DeleteById(int id);
    }
}