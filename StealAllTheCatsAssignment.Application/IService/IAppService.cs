﻿using StealAllTheCatsAssignment.Application.DTOs;
using StealAllTheCatsAssignment.Domain.Models;

namespace StealAllTheCatsAssignment.Application.IService
{
    public interface IAppService
    {
        public Task<ResponseDto> DeserializeAndStoreInDb();
        public Task<Cat?> GetCatById(int id);
        public Task<IEnumerable<Cat>?> GetCatsByTag(string tag);
        public Task<IEnumerable<Cat>?> GetAllCats();
    }
}
