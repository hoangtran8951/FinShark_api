using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiSTockapi.Data;
using apiSTockapi.Helpers;
using apiSTockapi.Interfaces;
using apiSTockapi.Models;
using Microsoft.EntityFrameworkCore;

namespace apiSTockapi.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IStockRepository _stockRepo;
        public CommentRepository(ApplicationDbContext context, IStockRepository stockRepository)
        {
            _context = context;
            _stockRepo = stockRepository;
        }

        public async Task<CommentModel> CreateAsync(CommentModel comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteById(int id)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(item => item.Id == id);
            if(commentModel == null)
                return false;
            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CommentModel>> GetAllAsync(CommentQueryObject query)
        {
            var commentModel = _context.Comments.Include(a => a.AppUser).AsQueryable();
            if(!string.IsNullOrEmpty(query.Symbol))
                commentModel = commentModel.Where(c => c.Stock.Symbol.ToLower().Contains(query.Symbol.ToLower()));
            commentModel = query.IsDescending ? commentModel.OrderByDescending(c => c.Stock.Symbol) : commentModel.OrderBy(c => c.Stock.Symbol);
            return await commentModel.ToListAsync();
        }

        public async Task<CommentModel?> GetById(int id)
        {
            return await _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CommentModel?> UpdateAsync(CommentModel comment)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(c => c.Id == comment.Id);
            if(commentModel == null)
                return null;
                
            commentModel.Title = comment.Title;
            commentModel.content = comment.content;
            await _context.SaveChangesAsync();
            return commentModel;
        }
    }
}