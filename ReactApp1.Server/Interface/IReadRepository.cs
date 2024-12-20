﻿using ReactApp1.Server.Models.Abstractions;

namespace ReactApp1.Server.Interface
{
    public interface IReadRepository<T> where T : class 
    {
        T GetById(int id);
        IQueryable<T> GetAll();
    }
}
