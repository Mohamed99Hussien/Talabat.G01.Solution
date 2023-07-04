﻿using Talabat.APIs.DTOs;

namespace Talabat.APIs.Helpers
{
    //when endpoint return all data <T> . make object from pagination
    public class Pagination<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }

        public IEnumerable<T> Data { get; set; }
        public Pagination(int pageIndex, int pageSize,int count, IEnumerable<T> data) 
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Data = data;
            Count= count;
        }
        
    }
}
