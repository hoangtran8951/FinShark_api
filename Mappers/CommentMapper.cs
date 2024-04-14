using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiSTockapi.Dto.Comment;
using apiSTockapi.Models;

namespace apiSTockapi.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto FromModelToDto(this CommentModel commentModel){
            return new CommentDto
            {
                Title = commentModel.Title,
                content = commentModel.content,
                Createdon = commentModel.Createdon,
                StockId = commentModel.StockId,
                Id = commentModel.Id,
                CreatedBy = commentModel.AppUser.UserName
            };
        }

        public static CommentModel FromCreateDtoToModel(this CreateCommentDto createCommentDto, int StockId)
        {
            return new CommentModel 
            {
                Title = createCommentDto.Title,
                content = createCommentDto.content,
                Createdon = DateTime.UtcNow,
                StockId = StockId
            };
        }
        public static CommentModel FromUpdateDtoToModel(this UpdateCommentDto updateCommentDto, int id){
            return new CommentModel
            {
                Title = updateCommentDto.Title,
                content = updateCommentDto.content,
                Id = id,
            };
        }
    }
}